using Blazorise;
using HQSOFT.SharedInformation.Companies;
using HQSOFT.SharedInformation.Countries;
using HQSOFT.SharedInformation.Permissions;
using HQSOFT.SharedInformation.Provinces;
using HQSOFT.SharedInformation.States;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Components.Messages;
using Volo.Abp.AspNetCore.Components.Web.Theming.PageToolbars;
using Volo.Abp.AspNetCore.Components.Web.Theming.Toolbars;
using Volo.Abp.BlazoriseUI;
using Volo.Abp.ObjectMapping;
using static DevExpress.Drawing.Printing.Internal.DXPageSizeInfo;

namespace HQSOFT.SharedInformation.Blazor.Pages.SharedInformation.Company
{
    public partial class Companies
    {
        protected List<Volo.Abp.BlazoriseUI.BreadcrumbItem> BreadcrumbItems = new List<Volo.Abp.BlazoriseUI.BreadcrumbItem>();
        protected PageToolbar Toolbar { get; } = new PageToolbar();

        private bool CanCreateCompany { get; set; }
        private bool CanEditCompany { get; set; }
        private bool CanDeleteCompany { get; set; }
        private IReadOnlyList<CompanyDto> CompanyList { get; set; }
        private IReadOnlyList<CompanyDto> ParentCompaniesCollection { get; set; } = new List<CompanyDto>();

        private int PageSize { get; } = LimitedResultRequestDto.DefaultMaxResultCount;
        private int CurrentPage { get; set; } = 1;
        private int MaxCount { get; } = 1000;
        private string CurrentSorting { get; set; } = string.Empty;

        private EditForm EditFormMain { get; set; } //Id of Main form
        private CompanyDto EditingCompany { get; set; }
        private bool IsDataEntryChanged { get; set; } //keep value to indicate data has been changed or not
        private Guid EditingCompanyId { get; set; }
        private Guid ParentCompanyId { get; set; }

        private GetCompaniesInput Filter { get; set; }
        private GetCountriesInput FilterCountry { get; set; }
        private GetProvincesInput FilterProvince{ get; set; }

        private IReadOnlyCollection<CountryDto> CountriesCollection { get; set; } = new List<CountryDto>();
        private IReadOnlyCollection<StateDto> StatesCollection { get; set; } = new List<StateDto>();
        private IReadOnlyCollection<ProvinceDto> ProvincesCollection { get; set; } = new List<ProvinceDto>();

        private CountryDto SelectedCountry { get; set; }
        private ProvinceDto SelectedProvince { get; set; }
        private StateDto SelectedState { get; set; }

        private List<DefaultCurrency> DefaultCurrencies { get; set; }

        [Parameter]
        public string Id { get; set; }

        private readonly IUiMessageService _uiMessageService;

        private string param1Value { get; set; }


        //==================================Initialize Section===================================
        #region
        public Companies(IUiMessageService uiMessageService)
        {
            EditingCompany = new CompanyDto();
            Filter = new GetCompaniesInput
            {
                MaxResultCount = PageSize,
                SkipCount = (CurrentPage - 1) * PageSize,
                Sorting = CurrentSorting
            };

            CompanyList = new List<CompanyDto>();
            ParentCompaniesCollection = new List<CompanyDto>();

            FilterCountry = new GetCountriesInput
            {
                MaxResultCount = MaxCount,
            };

            FilterProvince = new GetProvincesInput
            {
                MaxResultCount = MaxCount,
            };

            DefaultCurrencies = new List<DefaultCurrency>();
            DefaultCurrencies.Add(new DefaultCurrency(Guid.Parse("3a0ca779-452f-daa9-4713-aff7c836b749"),"VND"));
            DefaultCurrencies.Add(new DefaultCurrency(Guid.Parse("3a0ca779-4567-70f1-fe4d-8f603e8b50c9"), "USD"));


            CountriesCollection = new List<CountryDto>();
            ProvincesCollection = new List<ProvinceDto>();
            StatesCollection = new List<StateDto>();

            _uiMessageService = uiMessageService;
        }

        protected override async Task OnInitializedAsync()
        {
            ParentCompaniesCollection = new List<CompanyDto>();

            var uri = new Uri(NavigationManager.Uri);
            var param1 = HttpUtility.ParseQueryString(uri.Query).Get("ParentCompany");
            param1Value = param1;
            if(param1Value != null)
            {
                ParentCompanyId = Guid.Parse(param1Value);
            }

            EditingCompany.ParentCompany =  ParentCompanyId;
            //param1Value = NavigationManager.GetQueryParameter(param1Value);
            EditingCompanyId = Guid.Parse(Id);
            await LoadDataAsync(EditingCompanyId);
            await GetCountryCollectionLookupAsync();
            await GetParentCompanyCollectionLookupAsync();
            await SetBreadcrumbItemsAsync();
            await SetToolbarItemsAsync();
            await SetPermissionsAsync();
        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            base.OnAfterRender(firstRender);
            await JSRuntime.InvokeVoidAsync("AssignGotFocus");
        }

        private async Task OnBeforeInternalNavigation(LocationChangingContext context)
        {
            bool checkSaving = await SavingConfirmAsync();
            if (!checkSaving)
                context.PreventNavigation();
        }
        protected virtual ValueTask SetBreadcrumbItemsAsync()
        {
            BreadcrumbItems.Add(new Volo.Abp.BlazoriseUI.BreadcrumbItem(L["Menu:Companies"]));
            return ValueTask.CompletedTask;
        }

        protected virtual ValueTask SetToolbarItemsAsync()
        {
            Toolbar.AddButton(L["Back"], async () =>
            {
                NavigationManager.NavigateTo($"/SharedInformation/Companies");
            },
           IconName.Undo,
           Color.Secondary);


            Toolbar.AddButton(L["Save"], async () => {
                await SaveCompanyAsync(false);
            }, IconName.Save, requiredPolicyName:SharedInformationPermissions.Companies.Create);

            Toolbar.AddButton(L["New"], async () =>
            {
                await SaveCompanyAsync(true);
            }, IconName.Add,
           Color.Primary,
           requiredPolicyName: SharedInformationPermissions.Companies.Create );

            Toolbar.AddButton(L["Delete"], DeleteCompany,
           IconName.Delete, 
           Color.Danger,
           requiredPolicyName: SharedInformationPermissions.Companies.Delete );

            return ValueTask.CompletedTask;
        }

        private async Task SetPermissionsAsync()
        {
            CanCreateCompany = await AuthorizationService
                .IsGrantedAsync(SharedInformationPermissions.Companies.Create);
            CanEditCompany = await AuthorizationService
                            .IsGrantedAsync(SharedInformationPermissions.Companies.Edit);
            CanDeleteCompany = await AuthorizationService
                            .IsGrantedAsync(SharedInformationPermissions.Companies.Delete);
        }
        #endregion



        //======================Load Data Source for ListView & Others===========================
        #region

       
        private async Task GetCountryCollectionLookupAsync()
        {
            CountriesCollection = (await CountriesAppService.GetListAsync(new GetCountriesInput { MaxResultCount = MaxCount, })).Items;
        }
        private async Task GetProvinceCollectionLookupAsync()
        {
            ProvincesCollection = (await ProvincesAppService.GetListAsync(new GetProvincesInput { MaxResultCount = MaxCount, CountryId = SelectedCountry.Id })).Items;
        }

        private async Task GetStateCollectionLookupAsync()
        {
            StatesCollection = (await StatesAppService.GetListAsync(new GetStatesInput { MaxResultCount = MaxCount, CountryId = SelectedCountry.Id })).Items;
        }
        
        private async Task GetParentCompanyCollectionLookupAsync()
        {
            ParentCompaniesCollection = (await CompaniesAppService.GetListAsync(new GetCompaniesInput { MaxResultCount = MaxCount, IsGroup = true})).Items;
        }
        #endregion


        //======================CRUD & Load Main Data Source Section=============================
        #region

        private async Task LoadDataAsync(Guid companyId)
        {
            if (companyId != Guid.Empty)
            {            
                EditingCompany = await CompaniesAppService.GetAsync(companyId);              
            }
        }
        private void CreateNewCompany()
        {
            EditingCompany = new CompanyDto
            {
                ConcurrencyStamp = string.Empty,

            };
            IsDataEntryChanged = false;
            ParentCompaniesCollection = new List<CompanyDto>();
            EditingCompanyId = Guid.Empty;
            NavigationManager.NavigateTo($"/SharedInformation/Companies/edit/{Guid.Empty}");
        }
        public async Task SaveCompanyAsync(bool isNewNext)
        {
            try
            {
                if (!EditFormMain.EditContext.Validate())
                {
                    return;
                }

                if (EditingCompanyId == Guid.Empty)
                {
                    var company = await CompaniesAppService.CreateAsync(ObjectMapper.Map<CompanyDto, CompanyCreateDto>(EditingCompany));
                    EditingCompanyId = company.Id;              
                }
                else
                {
                    EditingCompany =  await CompaniesAppService.UpdateAsync(EditingCompanyId, ObjectMapper.Map<CompanyDto, CompanyUpdateDto>(EditingCompany));
                }

                if (isNewNext)
                    CreateNewCompany();
                else
                {
                    IsDataEntryChanged = false;
                    NavigationManager.NavigateTo($"/SharedInformation/Companies/edit/{EditingCompanyId}");
                }
            }
           
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
            }
        }
       
        private async Task DeleteCompany()
        {
            var confirmed = await _uiMessageService.Confirm(L["DeleteConfirmationMessage"]);
            if (confirmed)
            {
                await Task.CompletedTask;
                await DeleteCompanyAsync(EditingCompanyId);
            }
        }
        private async Task DeleteCompanyAsync(Guid Id)
        {
            await CompaniesAppService.DeleteAsync(Id);
            NavigationManager.NavigateTo("/SharedInformation/Companies");
        }

        #endregion

        //=====================================Validations=======================================
        #region
        private async Task<bool> SavingConfirmAsync()
        {
            if (IsDataEntryChanged)
            {
                var confirmed = await _uiMessageService.Confirm(L["SavingConfirmationMessage"]);
                if (confirmed)
                    return true;
                else
                    return false;
            }
            else
                return true;
        }
        #endregion
        //============================Controls triggers/events===================================
        #region
        async Task SelectedCountryChangedAsync(CountryDto country)
        {
            SelectedCountry = country;
            await GetProvinceCollectionLookupAsync();
            await GetStateCollectionLookupAsync();
            SelectedProvince = ProvincesCollection.FirstOrDefault();
            SelectedState = StatesCollection.FirstOrDefault();
            IsDataEntryChanged = true;
        }
        async Task SelectedProvinceChangedAsync(ProvinceDto province)
        {
            SelectedProvince = province;
            EditingCompany.ProvinceId= province.Id;
            IsDataEntryChanged = true;

        }
        async Task SelectedStateChangedAsync(StateDto state)
        {
            SelectedState = state;
            EditingCompany.StateId = state.Id;
            IsDataEntryChanged = true;

        }
        #endregion
    }
}
