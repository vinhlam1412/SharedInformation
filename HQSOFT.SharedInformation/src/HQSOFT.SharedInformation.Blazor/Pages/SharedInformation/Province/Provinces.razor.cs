using Blazorise;
using DevExpress.Blazor;
using HQSOFT.SharedInformation.Countries;
using HQSOFT.SharedInformation.Provinces;
using HQSOFT.SharedInformation.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Components.Messages;
using Volo.Abp.AspNetCore.Components.Web.Theming.PageToolbars;
using Volo.Abp;

namespace HQSOFT.SharedInformation.Blazor.Pages.SharedInformation.Province
{
    public partial class Provinces
    {
        protected List<Volo.Abp.BlazoriseUI.BreadcrumbItem> BreadcrumbItems = new List<Volo.Abp.BlazoriseUI.BreadcrumbItem>();
        protected PageToolbar Toolbar { get; } = new PageToolbar();
        private int PageSize { get; } = LimitedResultRequestDto.DefaultMaxResultCount;
        private int MaxCount { get; } = 1000;
        private bool CanCreateProvince { get; set; }
        private bool CanEditProvince { get; set; }
        private bool CanDeleteProvince { get; set; }
        private ProvinceDto EditingProvince { get; set; } = new ProvinceDto();  //Editing row on grid
        private Guid EditingProvinceId { get; set; } = Guid.Empty; //Editing Province Id on grid
        private List<ProvinceDto> ProvinceList { get; set; } = new List<ProvinceDto>(); //Data source used to bind to grid
        private IReadOnlyList<object> SelectedProvinces { get; set; } = new List<ProvinceDto>(); //Selected rows on grid
        private GetProvincesInput Filter { get; set; } //Used for Search box
        private IGrid GridProvince { get; set; } //Id of Province grid control name
        private bool IsDataEntryChanged { get; set; } //keep value to indicate data has been changed or not
        private IReadOnlyList<CountryDto> CountriesCollection { get; set; } = new List<CountryDto>();
        private CountryDto SelectedCountry { get; set; } = new CountryDto();
        string FocusedColumn { get; set; }
        private EditContext _GridProvinceEditContext { get; set; } //Injected Editcontext of Province grid

        private readonly IUiMessageService _uiMessageService; //Injected UIMessage

        //==================================Initialize Section===================================
        #region
        public Provinces(IUiMessageService uiMessageService)
        {
            Filter = new GetProvincesInput
            {
                MaxResultCount = MaxCount,
            };
            _uiMessageService = uiMessageService;
        }

        protected override async Task OnInitializedAsync()
        {

            await SetPermissionsAsync();
            await SetBreadcrumbItemsAsync();
            await SetToolbarItemsAsync();
            await GetProvincesAsync();
            await GetCountryCollectionLookupAsync();
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

        private async Task SetPermissionsAsync()
        {
            CanCreateProvince = await AuthorizationService.IsGrantedAsync(SharedInformationPermissions.Provinces.Create);
            CanEditProvince = await AuthorizationService.IsGrantedAsync(SharedInformationPermissions.Provinces.Edit);
            CanDeleteProvince = await AuthorizationService.IsGrantedAsync(SharedInformationPermissions.Provinces.Delete);

        }

        protected virtual ValueTask SetBreadcrumbItemsAsync()
        {
            BreadcrumbItems.Add(new Volo.Abp.BlazoriseUI.BreadcrumbItem(L["Menu:GeographicalSubdivisions"]));
            BreadcrumbItems.Add(new Volo.Abp.BlazoriseUI.BreadcrumbItem(L["Menu:Provinces"]));
            return ValueTask.CompletedTask;
        }

        protected virtual ValueTask SetToolbarItemsAsync()
        {
            Toolbar.AddButton(L["Save"], async () =>
            {
                await SaveProvinceAsync();
            },
            IconName.Save,
            Color.Primary,
            requiredPolicyName: SharedInformationPermissions.Provinces.Edit, disabled: !CanEditProvince);

            Toolbar.AddButton(L["Delete"], DeleteProvince,
            IconName.Delete,
            Color.Danger,
            requiredPolicyName: SharedInformationPermissions.Provinces.Delete, disabled: !CanDeleteProvince);

            return ValueTask.CompletedTask;
        }
        #endregion

        //======================Load Data Source for ListView & Others===========================
        #region
        private async Task GetCountryCollectionLookupAsync()
        {
            CountriesCollection = (await CountriesAppService.GetListAsync(new GetCountriesInput { MaxResultCount = MaxCount, })).Items;
            //SelectedCountry = CountriesCollection.FirstOrDefault();
        }
        #endregion

        //======================CRUD & Load Main Data Source Section=============================
        #region
        private async Task GetProvincesAsync()
        {
            Filter.CountryId = SelectedCountry.Id;
            Filter.MaxResultCount = MaxCount;
            var result = await ProvincesAppService.GetListAsync(Filter);

            ProvinceList = (List<ProvinceDto>)result.Items;
            IsDataEntryChanged = false;
            await InvokeAsync(StateHasChanged);
        }

        private async Task SaveProvinceAsync()
        {
            try
            {
                await GridProvince.SaveChangesAsync();

                foreach (var Province in ProvinceList)
                {
                    if (Province.ConcurrencyStamp == string.Empty)
                        await ProvincesAppService.CreateAsync(ObjectMapper.Map<ProvinceDto, ProvinceCreateDto>(Province));
                    else if (Province.IsChanged)
                        await ProvincesAppService.UpdateAsync(Province.Id, ObjectMapper.Map<ProvinceDto, ProvinceUpdateDto>(Province));
                }

                await GetProvincesAsync();
                await InvokeAsync(StateHasChanged);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
            }
        }
        private async Task DeleteProvince()
        {
            var confirmed = await _uiMessageService.Confirm(L["DeleteConfirmationMessage"]);
            if (confirmed)
            {
                if (SelectedProvinces != null)
                {
                    foreach (ProvinceDto row in SelectedProvinces)
                    {
                        await ProvincesAppService.DeleteAsync(row.Id);
                        ProvinceList.Remove(row);
                    }
                }
                GridProvince.Reload();
                await InvokeAsync(StateHasChanged);
            }
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
            await GetProvincesAsync();
        }
        EditContext GridProvinceEditContext
        {
            get { return GridProvince.IsEditing() ? _GridProvinceEditContext : null; }
            set { _GridProvinceEditContext = value; }
        }
        private async Task GridProvince_OnFocusedRowChanged(GridFocusedRowChangedEventArgs e)
        {
            if (GridProvince.IsEditing() && GridProvinceEditContext.IsModified())
            {
                await GridProvince.SaveChangesAsync();
                IsDataEntryChanged = true;
            }
            else
                await GridProvince.CancelEditAsync();
        }

        private async Task GridProvince_OnRowDoubleClick(GridRowClickEventArgs e)
        {
            if (CanEditProvince)
            {
                FocusedColumn = e.Column.Name;
                await e.Grid.StartEditRowAsync(e.VisibleIndex);
                EditingProvince = (ProvinceDto)e.Grid.GetFocusedDataItem();
                EditingProvinceId = EditingProvince.Id;
            }
        }
        private void GridProvince_OnCustomizeEditModel(GridCustomizeEditModelEventArgs e)
        {
            if (e.IsNew)
            {
                if (SelectedCountry == null || SelectedCountry.Id == Guid.Empty)
                    throw new UserFriendlyException(L["CountrySelect"]);

                var newRow = (ProvinceDto)e.EditModel;

                newRow.CountryId = SelectedCountry.Id;
                newRow.Id = Guid.Empty;
                newRow.ConcurrencyStamp = string.Empty;

                if (GridProvince.GetVisibleRowCount() > 0)
                    newRow.Idx = ProvinceList.Max(x => x.Idx) + 1;
                else
                    newRow.Idx = 1;
            }
        }

        private void GridProvince_EditModelSaving(GridEditModelSavingEventArgs e)
        {
            ProvinceDto editModel = (ProvinceDto)e.EditModel;
            ProvinceDto dataItem = e.IsNew ? new ProvinceDto() : ProvinceList.Find(item => item.Idx == editModel.Idx);

            if (editModel != null && !e.IsNew)
            {
                editModel.IsChanged = true;
                ProvinceList.Remove(dataItem);
                ProvinceList.Add(editModel);
            }
            if (editModel != null && e.IsNew)
            {
                editModel.IsChanged = true;
                ProvinceList.Add(editModel);
            }
        }

        private async Task GridProvince_OnKeyDown(KeyboardEventArgs e)
        {
            if (e.Key == "F2" && CanEditProvince)
            {
                await GridProvince.StartEditRowAsync(GridProvince.GetFocusedRowIndex());
            }
        }

        private async Task BtnAdd_GridProvince_OnClick()
        {
            await GridProvince.SaveChangesAsync();
            await GridProvince.StartEditNewRowAsync();
        }

        void GridProvince_CustomizeDataRowEditor(GridCustomizeDataRowEditorEventArgs e)
        {
            ((ITextEditSettings)e.EditSettings).ShowValidationIcon = true;
        }

        private async Task BtnDelete_GridProvince_OnClick()
        {
            await DeleteProvince();
        }

        #endregion

        //============================Application Functions======================================
        #region

        #endregion
    }
}
