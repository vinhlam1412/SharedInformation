using Blazorise;
using DevExpress.Blazor;
using HQSOFT.SharedInformation.Countries;
using HQSOFT.SharedInformation.Provinces;
using HQSOFT.SharedInformation.Wards;
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
using HQSOFT.SharedInformation.Districts;

namespace HQSOFT.SharedInformation.Blazor.Pages.SharedInformation.Ward
{
    public partial class Wards
    {
        protected List<Volo.Abp.BlazoriseUI.BreadcrumbItem> BreadcrumbItems = new List<Volo.Abp.BlazoriseUI.BreadcrumbItem>();
        protected PageToolbar Toolbar { get; } = new PageToolbar();
        private int PageSize { get; } = LimitedResultRequestDto.DefaultMaxResultCount;
        private int MaxCount { get; } = 1000;
        private bool CanCreateWard { get; set; }
        private bool CanEditWard { get; set; }
        private bool CanDeleteWard { get; set; }
        private WardDto EditingWard { get; set; } = new WardDto();  //Editing row on grid
        private Guid EditingWardId { get; set; } = Guid.Empty; //Editing Ward Id on grid
        private List<WardDto> WardList { get; set; } = new List<WardDto>(); //Data source used to bind to grid
        private IReadOnlyList<object> SelectedWards { get; set; } = new List<WardDto>(); //Selected rows on grid
        private GetWardsInput Filter { get; set; } //Used for Search box
        private IGrid GridWard { get; set; } //Id of Ward grid control name
        private bool IsDataEntryChanged { get; set; } //keep value to indicate data has been changed or not
        private IReadOnlyList<DistrictDto> DistrictsCollection { get; set; } = new List<DistrictDto>();
        private DistrictDto SelectedDistrict { get; set; } = new DistrictDto();
        private IReadOnlyList<ProvinceDto> ProvincesCollection { get; set; } = new List<ProvinceDto>();
        private ProvinceDto SelectedProvince { get; set; } = new ProvinceDto();
        private IReadOnlyList<CountryDto> CountriesCollection { get; set; } = new List<CountryDto>();
        private CountryDto SelectedCountry { get; set; } = new CountryDto();
        string FocusedColumn { get; set; }
        private EditContext _GridWardEditContext { get; set; } //Injected Editcontext of Ward grid

        private readonly IUiMessageService _uiMessageService; //Injected UIMessage

        //==================================Initialize Section===================================
        #region
        public Wards(IUiMessageService uiMessageService)
        {
            Filter = new GetWardsInput
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
            await GetWardsAsync();
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
            CanCreateWard = await AuthorizationService.IsGrantedAsync(SharedInformationPermissions.Wards.Create);
            CanEditWard = await AuthorizationService.IsGrantedAsync(SharedInformationPermissions.Wards.Edit);
            CanDeleteWard = await AuthorizationService.IsGrantedAsync(SharedInformationPermissions.Wards.Delete);

        }

        protected virtual ValueTask SetBreadcrumbItemsAsync()
        {
            BreadcrumbItems.Add(new Volo.Abp.BlazoriseUI.BreadcrumbItem(L["Menu:GeographicalSubdivisions"]));
            BreadcrumbItems.Add(new Volo.Abp.BlazoriseUI.BreadcrumbItem(L["Menu:Wards"]));

            return ValueTask.CompletedTask;
        }

        protected virtual ValueTask SetToolbarItemsAsync()
        {
            Toolbar.AddButton(L["Save"], SaveWardAsync,
            IconName.Save,
            Color.Primary,
            requiredPolicyName: SharedInformationPermissions.Wards.Edit, disabled: !CanEditWard);

            Toolbar.AddButton(L["Delete"], DeleteWard,
            IconName.Delete,
            Color.Danger,
            requiredPolicyName: SharedInformationPermissions.Wards.Delete, disabled: !CanDeleteWard);

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
            ProvincesCollection = (await ProvincesAppService.GetListAsync(new GetProvincesInput { MaxResultCount = MaxCount, CountryId = (SelectedCountry != null) ? SelectedCountry.Id : Guid.Empty })).Items;
        }
        private async Task GetDistrictCollectionLookupAsync()
        {
            DistrictsCollection = (await DistrictsAppService.GetListAsync(new GetDistrictsInput { MaxResultCount = MaxCount, ProvinceId = (SelectedProvince != null) ? SelectedProvince.Id : Guid.Empty })).Items;
        }

        #endregion

        //======================CRUD & Load Main Data Source Section=============================
        #region
        private async Task GetWardsAsync()
        {
            Filter.DistrictId = (SelectedDistrict != null) ? SelectedDistrict.Id : Guid.Empty;
            Filter.MaxResultCount = MaxCount;
            var result = await WardsAppService.GetListAsync(Filter);

            WardList = (List<WardDto>)result.Items;
            IsDataEntryChanged = false;
            await InvokeAsync(StateHasChanged);
        }

        private async Task SaveWardAsync()
        {
            try
            {
                await GridWard.SaveChangesAsync();

                foreach (var Ward in WardList)
                {
                    if (Ward.ConcurrencyStamp == string.Empty)
                        await WardsAppService.CreateAsync(ObjectMapper.Map<WardDto, WardCreateDto>(Ward));
                    else if (Ward.IsChanged)
                        await WardsAppService.UpdateAsync(Ward.Id, ObjectMapper.Map<WardDto, WardUpdateDto>(Ward));
                }

                await GetWardsAsync();
                await InvokeAsync(StateHasChanged);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
            }
        }
        private async Task DeleteWard()
        {
            var confirmed = await _uiMessageService.Confirm(L["DeleteConfirmationMessage"]);
            if (confirmed)
            {
                if (SelectedWards != null)
                {
                    foreach (WardDto row in SelectedWards)
                    {
                        await WardsAppService.DeleteAsync(row.Id);
                        WardList.Remove(row);
                    }
                }
                GridWard.Reload();
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
            await GetDistrictCollectionLookupAsync();
            SelectedDistrict = DistrictsCollection.FirstOrDefault();
            await GetWardsAsync();
        }
        async Task SelectedProvinceChangedAsync(ProvinceDto province)
        {
            SelectedProvince = province;
            await GetDistrictCollectionLookupAsync();
            SelectedDistrict = DistrictsCollection.FirstOrDefault();
            await GetWardsAsync();
        }
        async Task SelectedDistrictChangedAsync(DistrictDto district)
        {
            SelectedDistrict = district;
            await GetWardsAsync();
        }
        EditContext GridWardEditContext
        {
            get { return GridWard.IsEditing() ? _GridWardEditContext : null; }
            set { _GridWardEditContext = value; }
        }
        private async Task GridWard_OnFocusedRowChanged(GridFocusedRowChangedEventArgs e)
        {
            if (GridWard.IsEditing() && GridWardEditContext.IsModified())
            {
                await GridWard.SaveChangesAsync();
                IsDataEntryChanged = true;
            }
            else
                await GridWard.CancelEditAsync();
        }

        private async Task GridWard_OnRowDoubleClick(GridRowClickEventArgs e)
        {
            if (CanEditWard)
            {
                FocusedColumn = e.Column.Name;
                await e.Grid.StartEditRowAsync(e.VisibleIndex);
                EditingWard = (WardDto)e.Grid.GetFocusedDataItem();
                EditingWardId = EditingWard.Id;
            }
        }
        private void GridWard_OnCustomizeEditModel(GridCustomizeEditModelEventArgs e)
        {
            if (e.IsNew)
            {
                if (SelectedDistrict == null || SelectedDistrict.Id == Guid.Empty)
                    throw new UserFriendlyException(L["DistrictSelect"]);

                var newRow = (WardDto)e.EditModel;

                newRow.DistrictId = SelectedDistrict.Id;
                newRow.Id = Guid.Empty;
                newRow.ConcurrencyStamp = string.Empty;

                if (GridWard.GetVisibleRowCount() > 0)
                    newRow.Idx = WardList.Max(x => x.Idx) + 1;
                else
                    newRow.Idx = 1;
            }
        }

        private void GridWard_EditModelSaving(GridEditModelSavingEventArgs e)
        {
            WardDto editModel = (WardDto)e.EditModel;
            WardDto dataItem = e.IsNew ? new WardDto() : WardList.Find(item => item.Idx == editModel.Idx);

            if (editModel != null && !e.IsNew)
            {
                editModel.IsChanged = true;
                WardList.Remove(dataItem);
                WardList.Add(editModel);
            }
            if (editModel != null && e.IsNew)
            {
                editModel.IsChanged = true;
                WardList.Add(editModel);
            }
        }

        private async Task GridWard_OnKeyDown(KeyboardEventArgs e)
        {
            if (e.Key == "F2" && CanEditWard)
                await GridWard.StartEditRowAsync(GridWard.GetFocusedRowIndex());
        }

        void GridWard_CustomizeDataRowEditor(GridCustomizeDataRowEditorEventArgs e)
        {
            ((ITextEditSettings)e.EditSettings).ShowValidationIcon = true;
        }
        private async Task BtnAdd_GridWard_OnClick()
        {
            await GridWard.SaveChangesAsync();
            await GridWard.StartEditNewRowAsync();
        }

        private async Task BtnDelete_GridWard_OnClick()
        {
            await DeleteWard();
        }

        #endregion

        //============================Application Functions======================================
        #region

        #endregion
    }
}
