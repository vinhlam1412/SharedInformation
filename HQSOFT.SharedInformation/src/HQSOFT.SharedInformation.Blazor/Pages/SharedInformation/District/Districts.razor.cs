using Blazorise;
using DevExpress.Blazor;
using HQSOFT.SharedInformation.Countries;
using HQSOFT.SharedInformation.Provinces;
using HQSOFT.SharedInformation.Districts;
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

namespace HQSOFT.SharedInformation.Blazor.Pages.SharedInformation.District
{
    public partial class Districts
    {
        protected List<Volo.Abp.BlazoriseUI.BreadcrumbItem> BreadcrumbItems = new List<Volo.Abp.BlazoriseUI.BreadcrumbItem>();
        protected PageToolbar Toolbar { get; } = new PageToolbar();
        private int PageSize { get; } = LimitedResultRequestDto.DefaultMaxResultCount;
        private int MaxCount { get; } = 1000;
        private bool CanCreateDistrict { get; set; }
        private bool CanEditDistrict { get; set; }
        private bool CanDeleteDistrict { get; set; }
        private DistrictDto EditingDistrict { get; set; } = new DistrictDto();  //Editing row on grid
        private EditForm EditFormDistrict { get; set; } //Id of Main form
        private Guid EditingDistrictId { get; set; } = Guid.Empty; //Editing District Id on grid
        private List<DistrictDto> DistrictList { get; set; } = new List<DistrictDto>(); //Data source used to bind to grid
        private IReadOnlyList<object> SelectedDistricts { get; set; } = new List<DistrictDto>(); //Selected rows on grid
        private GetDistrictsInput Filter { get; set; } //Used for Search box
        private IGrid GridDistrict { get; set; } //Id of District grid control name
        private bool IsDataEntryChanged { get; set; } //keep value to indicate data has been changed or not
        private IReadOnlyList<ProvinceDto> ProvincesCollection { get; set; } = new List<ProvinceDto>();
        private ProvinceDto SelectedProvince { get; set; } = new ProvinceDto();
        private IReadOnlyList<CountryDto> CountriesCollection { get; set; } = new List<CountryDto>();
        private CountryDto SelectedCountry { get; set; } = new CountryDto();
        string FocusedColumn { get; set; }
        private EditContext _GridDistrictEditContext { get; set; } //Injected Editcontext of District grid

        private readonly IUiMessageService _uiMessageService; //Injected UIMessage

        //==================================Initialize Section===================================
        #region
        public Districts(IUiMessageService uiMessageService)
        {
            Filter = new GetDistrictsInput
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
            await GetDistrictsAsync();
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
            CanCreateDistrict = await AuthorizationService.IsGrantedAsync(SharedInformationPermissions.Districts.Create);
            CanEditDistrict = await AuthorizationService.IsGrantedAsync(SharedInformationPermissions.Districts.Edit);
            CanDeleteDistrict = await AuthorizationService.IsGrantedAsync(SharedInformationPermissions.Districts.Delete);

        }

        protected virtual ValueTask SetBreadcrumbItemsAsync()
        {
            BreadcrumbItems.Add(new Volo.Abp.BlazoriseUI.BreadcrumbItem(L["Menu:GeographicalSubdivisions"]));
            BreadcrumbItems.Add(new Volo.Abp.BlazoriseUI.BreadcrumbItem(L["Menu:Districts"]));
            return ValueTask.CompletedTask;
        }

        protected virtual ValueTask SetToolbarItemsAsync()
        {
            Toolbar.AddButton(L["Save"], async () =>
            {
                await SaveDistrictAsync();
            },
            IconName.Save,
            Color.Primary,
            requiredPolicyName: SharedInformationPermissions.Districts.Edit, disabled: !CanEditDistrict);

            Toolbar.AddButton(L["Delete"], DeleteDistrict,
            IconName.Delete,
            Color.Danger,
            requiredPolicyName: SharedInformationPermissions.Districts.Delete, disabled: !CanDeleteDistrict);

            return ValueTask.CompletedTask;
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
        #endregion

        //======================CRUD & Load Main Data Source Section=============================
        #region
        private async Task GetDistrictsAsync()
        {
            if (SelectedProvince != null)
                Filter.ProvinceId = SelectedProvince.Id;
            else
                Filter.ProvinceId = Guid.Empty;

            Filter.MaxResultCount = MaxCount;
            var result = await DistrictsAppService.GetListAsync(Filter);

            DistrictList = (List<DistrictDto>)result.Items;
            IsDataEntryChanged = false;
            await InvokeAsync(StateHasChanged);
        }

        private async Task SaveDistrictAsync()
        {
            try
            {
                await GridDistrict.SaveChangesAsync();

                foreach (var District in DistrictList)
                {
                    if (District.ConcurrencyStamp == string.Empty)
                        await DistrictsAppService.CreateAsync(ObjectMapper.Map<DistrictDto, DistrictCreateDto>(District));
                    else if (District.IsChanged)
                        await DistrictsAppService.UpdateAsync(District.Id, ObjectMapper.Map<DistrictDto, DistrictUpdateDto>(District));
                }

                await GetDistrictsAsync();
                await InvokeAsync(StateHasChanged);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
            }
        }
        private async Task DeleteDistrict()
        {
            var confirmed = await _uiMessageService.Confirm(L["DeleteConfirmationMessage"]);
            if (confirmed)
            {
                if (SelectedDistricts != null)
                {
                    foreach (DistrictDto row in SelectedDistricts)
                    {
                        await DistrictsAppService.DeleteAsync(row.Id);
                        DistrictList.Remove(row);
                    }
                }
                GridDistrict.Reload();
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
            await GetProvinceCollectionLookupAsync();
            SelectedProvince = ProvincesCollection.FirstOrDefault();
            await GetDistrictsAsync();
        }
        async Task SelectedProvinceChangedAsync(ProvinceDto province)
        {
            SelectedProvince = province;
            await GetDistrictsAsync();
        }
        EditContext GridDistrictEditContext
        {
            get { return GridDistrict.IsEditing() ? _GridDistrictEditContext : null; }
            set { _GridDistrictEditContext = value; }
        }
        private async Task GridDistrict_OnFocusedRowChanged(GridFocusedRowChangedEventArgs e)
        {
            if (GridDistrict.IsEditing() && GridDistrictEditContext.IsModified())
            {
                await GridDistrict.SaveChangesAsync();
                IsDataEntryChanged = true;
            }
            else
                await GridDistrict.CancelEditAsync();
        }

        private async Task GridDistrict_OnRowDoubleClick(GridRowClickEventArgs e)
        {
            if (CanEditDistrict)
            {
                FocusedColumn = e.Column.Name;
                await e.Grid.StartEditRowAsync(e.VisibleIndex);
                EditingDistrict = (DistrictDto)e.Grid.GetFocusedDataItem();
                EditingDistrictId = EditingDistrict.Id;
            }
        }
        private async Task GridDistrict_OnCustomizeEditModel(GridCustomizeEditModelEventArgs e)
        {
            if (e.IsNew)
            {
                if (SelectedProvince == null || SelectedProvince.Id == Guid.Empty)
                    throw new UserFriendlyException(L["ProvinceSelect"]);
         
                var newRow = (DistrictDto)e.EditModel;

                newRow.ProvinceId = SelectedProvince.Id;
                newRow.Id = Guid.Empty;
                newRow.ConcurrencyStamp = string.Empty;

                if (GridDistrict.GetVisibleRowCount() > 0)
                    newRow.Idx = DistrictList.Max(x => x.Idx) + 1;
                else
                    newRow.Idx = 1;
                   

                
            }
        }

        private void GridDistrict_EditModelSaving(GridEditModelSavingEventArgs e)
        {
            DistrictDto editModel = (DistrictDto)e.EditModel;
            DistrictDto dataItem = e.IsNew ? new DistrictDto() : DistrictList.Find(item => item.Idx == editModel.Idx);

            if (editModel != null && !e.IsNew)
            {
                editModel.IsChanged = true;
                DistrictList.Remove(dataItem);
                DistrictList.Add(editModel);
            }
            if (editModel != null && e.IsNew)
            {
                editModel.IsChanged = true;
                DistrictList.Add(editModel);
            }
        }

        private async Task GridDistrict_OnKeyDown(KeyboardEventArgs e)
        {
            if (e.Key == "F2" && CanEditDistrict)
                await GridDistrict.StartEditRowAsync(GridDistrict.GetFocusedRowIndex());
        }

        void GridDistrict_CustomizeDataRowEditor(GridCustomizeDataRowEditorEventArgs e)
        {
            ((ITextEditSettings)e.EditSettings).ShowValidationIcon = true;
        }
        private async Task BtnAdd_GridDistrict_OnClick()
        {
            await GridDistrict.SaveChangesAsync();
            await GridDistrict.StartEditNewRowAsync();
        }

        private async Task BtnDelete_GridDistrict_OnClick()
        {
            await DeleteDistrict();
        }

        #endregion

        //============================Application Functions======================================
        #region

        #endregion
    }
}
