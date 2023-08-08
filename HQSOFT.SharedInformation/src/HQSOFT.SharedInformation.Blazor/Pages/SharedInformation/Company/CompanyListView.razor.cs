using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazorise;
using Blazorise.DataGrid;
using Volo.Abp.BlazoriseUI.Components;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Components.Web.Theming.PageToolbars;
using HQSOFT.SharedInformation.Countries;
using HQSOFT.SharedInformation.Districts;
using Volo.Abp.AspNetCore.Components.Messages;
using DevExpress.Blazor;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Components.Web;
using System.ComponentModel.Design;
using System.ComponentModel;
using DevExpress.XtraEditors.Controls;
using HQSOFT.SharedInformation.Companies;
using HQSOFT.SharedInformation.Permissions;

namespace HQSOFT.SharedInformation.Blazor.Pages.SharedInformation.Company
{
    public partial class CompanyListView
    {
        protected List<Volo.Abp.BlazoriseUI.BreadcrumbItem> BreadcrumbItems = new List<Volo.Abp.BlazoriseUI.BreadcrumbItem>();
        protected PageToolbar Toolbar {get;} = new PageToolbar();
        private IReadOnlyList<CompanyDto> CompanyList { get; set; }
        private IReadOnlyList<CompanyDto> CompaniesCollection { get; set; } = new List<CompanyDto>();
        private IReadOnlyList<CountryDto> CountriesCollection { get; set; } = new List<CountryDto>();
        private int MaxCount { get; } = 1000;
        private int PageSize { get; } = LimitedResultRequestDto.DefaultMaxResultCount;
        private int CurrentPage { get; set; } = 1;
        private string CurrentSorting { get; set; } = string.Empty;
        private int TotalCount { get; set; }   
        private bool CanCreateCompany { get; set; }
        private bool CanEditCompany { get; set; }
        private bool CanDeleteCompany { get; set; }
        private GetCompaniesInput Filter { get; set; }
        private List<CompanyDto> SelectedCompanies { get; set; }
        private readonly IUiMessageService _uiMessageService;

        private List<string> ViewMode { get; set; } = new List<string>();
        private string SelectedViewMode { get; set; }

        private CompanyDto selectedCompany { get; set; } = new CompanyDto();
       
        private CompanyDto companyContext { get;set; } = new CompanyDto();
        
        DxContextMenu ContextMenu { get; set; }
        DxContextMenu ContextMenu2 { get; set; }
        bool showFirstItem = false;
        object ClickedItem { get; set; }

        DxTreeView SampleTreeView;

        private bool visible;


        //==================================Initialize Section===================================
        #region
        public CompanyListView(IUiMessageService uiMessageService)
        {
            Filter = new GetCompaniesInput
            {
                MaxResultCount = PageSize,
                SkipCount = (CurrentPage - 1) * PageSize,
                Sorting = CurrentSorting
            };

            CompanyList = new List<CompanyDto>();
            SelectedCompanies = new List<CompanyDto>();
            _uiMessageService = uiMessageService;
        }

        protected override async Task OnInitializedAsync()
        {
            await SetToolbarItemsAsync();
            await SetBreadcrumbItemsAsync();
            await SetPermissionsAsync();
            ViewMode = new List<string> {"List View", "Tree View"};
            SelectedViewMode = "List View";
            await GetCountryCollectionLookupAsync();
            await GetCompanyCollectionLookupAsync();

        }

        protected virtual ValueTask SetBreadcrumbItemsAsync()
        {
            BreadcrumbItems.Add(new Volo.Abp.BlazoriseUI.BreadcrumbItem(L["Menu:Companies"]));
            return ValueTask.CompletedTask;
        }

        protected virtual ValueTask SetToolbarItemsAsync()
        {
            Toolbar.AddButton(L["ExportToExcel"], async () =>{ await DownloadAsExcelAsync(); }, IconName.Download);

            Toolbar.AddButton(L["Add Company"], () => {
                GoToEditPage();
                return Task.CompletedTask;
            }, IconName.Add, requiredPolicyName: SharedInformationPermissions.Companies.Create);


            Toolbar.AddButton(L["Delete"], async () =>
            {
                if (SelectedCompanies.Count > 0)
                {
                    var confirmed = await _uiMessageService.Confirm(L["DeleteConfirmationMessage"]);
                    if (confirmed)
                    {
                        foreach (var selectedCompany in SelectedCompanies)
                        {
                            await CompaniesAppService.DeleteAsync(selectedCompany.Id);
                        }
                        await GetCompaniesAsync();
                    }

                }
            }, IconName.Delete,
           Color.Danger,
           requiredPolicyName: SharedInformationPermissions.Companies.Delete);

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

        private async Task GetCompanyCollectionLookupAsync()
        {
            CompaniesCollection = (await CompaniesAppService.GetListAsync(new GetCompaniesInput { MaxResultCount = MaxCount, })).Items;
        }

        private async Task GetCompaniesAsync()
        {
            Filter.MaxResultCount = PageSize;
            Filter.SkipCount = (CurrentPage - 1) * PageSize;
            Filter.Sorting = CurrentSorting;

            var result = await CompaniesAppService.GetListAsync(Filter);
            CompanyList = result.Items;
            TotalCount = (int)result.TotalCount;
        }

        protected virtual async Task SearchAsync()
        {
            CurrentPage = 1;
            await GetCompaniesAsync();
            await InvokeAsync(StateHasChanged);
        }

        private  async Task DownloadAsExcelAsync()
        {
            var token = (await CompaniesAppService.GetDownloadTokenAsync()).Token;
            var remoteService = await RemoteServiceConfigurationProvider.GetConfigurationOrDefaultOrNullAsync("Configuration") ??
            await RemoteServiceConfigurationProvider.GetConfigurationOrDefaultOrNullAsync("Default");
            NavigationManager.NavigateTo($"{remoteService?.BaseUrl.EnsureEndsWith('/') ?? string.Empty}api/shared-information/companies/as-excel-file?DownloadToken={token}&FilterText={Filter.FilterText}", forceLoad: true);
        }
        #endregion

        //============================Controls triggers/events===================================
        #region
        private async Task OnDataGridReadAsync(DataGridReadDataEventArgs<CompanyDto> e)
        {
            CurrentSorting = e.Columns
                .Where(c => c.SortDirection != SortDirection.Default)
                .Select(c => c.Field + (c.SortDirection == SortDirection.Descending ? " DESC" : ""))
                .JoinAsString(",");
            CurrentPage = e.Page;
            await GetCompaniesAsync();
            await InvokeAsync(StateHasChanged);
        }

        void SelectionChanged(TreeViewNodeEventArgs e)
        {
            selectedCompany = (CompanyDto)e.NodeInfo.DataItem; /// lấy ra dto đang chọn
            SampleTreeView.SelectNode((n) => n.Text == e.NodeInfo.Text);
        }


        async void OnItemClick(ContextMenuItemClickEventArgs args)
        {
           ClickedItem = args.ItemInfo.Text;
           if(args.ItemInfo.Text == "Add")
            {
                //Add node con vào node cha, truyền id của node cha sang node con để làm parent company
                var company = await CompaniesAppService.GetAsync(companyContext.Id);
                NavigationManager.NavigateTo($"/SharedInformation/Companies/edit/{Guid.Empty}?ParentCompany={company.Id}");
            }
            if (args.ItemInfo.Text == "Edit")
            {
                GoToEditPage(companyContext);
            }

            if (args.ItemInfo.Text == "Delete")
            {
                var company = await CompaniesAppService.GetAsync(companyContext.Id);

                //Tìm Node đang chọn, nếu ID của node tồn tại trong cột ParentCompany => Node đó là node cha của 1 node nào đó và có tồn tại node con
                var isParentCompany = CompaniesCollection.Where(x => x.ParentCompany == companyContext.Id).FirstOrDefault();

                //Tồn tại node còn nên không thể xóa
                if (isParentCompany != null)
                {
                    await UiMessageService.Error(L[$"Cannot delete {company.CompanyName} as it has child nodes"]);
                }

                //Xóa nếu không có node con
                else
                {
                    await DeleteCompnayButton(companyContext.Id);
                }
           
            }         

        }

        Task ToggleIcon()
        {
            visible = !visible;

            return Task.CompletedTask;
        }

        public async Task OnContextMenu(MouseEventArgs e, CompanyDto companyDto)
        {
            companyContext = companyDto;

            //Nếu là node cha thì hiện context menu có chức năng add nút con
            if(companyContext.IsGroup == true)
            {
                await ContextMenu.ShowAsync(e);
            }

            //Hiện context menu không có add node con
            else
            {
                await ContextMenu2.ShowAsync(e);
            }        
            
        }

        private void SelectedViewModeChangedAsync(string viewMode)
        {
            SelectedViewMode = viewMode;
            StateHasChanged();
        }
        private async Task DeleteCompnayButton(Guid id)
        {
            await CompaniesAppService.DeleteAsync(id);
            await GetCompaniesAsync();
        }
        #endregion

        //======================CRUD & Load Main Data Source Section=============================
        #region

      
        #endregion


        private void GoToEditPage()
        {
            NavigationManager.NavigateTo($"/SharedInformation/Companies/edit/{Guid.Empty}");
        }

        private void GoToEditPage(CompanyDto company)
        {
            NavigationManager.NavigateTo($"/SharedInformation/Companies/edit/{company.Id}");
        }

    }
}
