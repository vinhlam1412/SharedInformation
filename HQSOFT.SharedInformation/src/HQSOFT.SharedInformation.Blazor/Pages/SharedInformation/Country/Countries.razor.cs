using Blazorise;
using DevExpress.Blazor;
using HQSOFT.SharedInformation.Countries;
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

namespace HQSOFT.SharedInformation.Blazor.Pages.SharedInformation.Country
{
    public partial class Countries
    {
        protected List<Volo.Abp.BlazoriseUI.BreadcrumbItem> BreadcrumbItems = new List<Volo.Abp.BlazoriseUI.BreadcrumbItem>();
        protected PageToolbar Toolbar { get; } = new PageToolbar();
        private int PageSize { get; } = LimitedResultRequestDto.DefaultMaxResultCount;
        private int MaxCount { get; } = 1000;
        private bool CanCreateCountry { get; set; }
        private bool CanEditCountry { get; set; }
        private bool CanDeleteCountry { get; set; }
        private CountryDto EditingCountry { get; set; } = new CountryDto();  //Editing row on grid
        private Guid EditingCountryId { get; set; } = Guid.Empty; //Editing Country Id on grid
        private List<CountryDto> CountryList { get; set; } = new List<CountryDto>(); //Data source used to bind to grid
        private IReadOnlyList<object> SelectedCountries { get; set; } = new List<CountryDto>(); //Selected rows on grid
        private GetCountriesInput Filter { get; set; } //Used for Search box
        private IGrid GridCountry { get; set; } //Id of Country grid control name
        private bool IsDataEntryChanged { get; set; } //keep value to indicate data has been changed or not
        string FocusedColumn { get; set; }
        private List<string> ViewMode { get; set; } = new List<string> { "List", "Tree View" };   
        private EditContext _gridCountryEditContext { get; set; } //Injected Editcontext of Country grid

        private readonly IUiMessageService _uiMessageService; //Injected UIMessage

        //==================================Initialize Section===================================
        #region
        public Countries(IUiMessageService uiMessageService)
        {
            Filter = new GetCountriesInput
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
            await GetCountriesAsync();

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
            CanCreateCountry = await AuthorizationService.IsGrantedAsync(SharedInformationPermissions.Countries.Create);
            CanEditCountry = await AuthorizationService.IsGrantedAsync(SharedInformationPermissions.Countries.Edit);
            CanDeleteCountry = await AuthorizationService.IsGrantedAsync(SharedInformationPermissions.Countries.Delete);

        }

        protected virtual ValueTask SetBreadcrumbItemsAsync()
        {
            BreadcrumbItems.Add(new Volo.Abp.BlazoriseUI.BreadcrumbItem(L["Menu:GeographicalSubdivisions"]));
            BreadcrumbItems.Add(new Volo.Abp.BlazoriseUI.BreadcrumbItem(L["Menu:Countries"]));
            return ValueTask.CompletedTask;
        }

        protected virtual ValueTask SetToolbarItemsAsync()
        {
            Toolbar.AddButton(L["Save"], async () =>
            {
                await SaveCountryAsync();
            },
            IconName.Save,
            Color.Primary,
            requiredPolicyName: SharedInformationPermissions.Countries.Edit, disabled: !CanEditCountry);

            Toolbar.AddButton(L["Delete"], DeleteCountry,
            IconName.Delete,
            Color.Danger,
            requiredPolicyName: SharedInformationPermissions.Countries.Delete, disabled: !CanDeleteCountry);

            return ValueTask.CompletedTask;
        }
        #endregion

        //======================Load Data Source for ListView & Others===========================
        #region

        #endregion

        //======================CRUD & Load Main Data Source Section=============================
        #region
        private async Task GetCountriesAsync()
        {
            Filter.MaxResultCount = MaxCount;
            var result = await CountriesAppService.GetListAsync(Filter);

            CountryList = (List<CountryDto>)result.Items;
            IsDataEntryChanged = false;
            await InvokeAsync(StateHasChanged);
        }

        private async Task SaveCountryAsync()
        {
            try
            {
                await GridCountry.SaveChangesAsync();

                foreach (var country in CountryList)
                {
                    if (country.ConcurrencyStamp == string.Empty)
                        await CountriesAppService.CreateAsync(ObjectMapper.Map<CountryDto, CountryCreateDto>(country));
                    else if (country.IsChanged)
                        await CountriesAppService.UpdateAsync(country.Id, ObjectMapper.Map<CountryDto, CountryUpdateDto>(country));
                }

                await GetCountriesAsync();
                await InvokeAsync(StateHasChanged);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
            }
        }
        private async Task DeleteCountry()
        {
            var confirmed = await _uiMessageService.Confirm(L["DeleteConfirmationMessage"]);
            if (confirmed)
            {
                if (SelectedCountries != null)
                {
                    foreach (CountryDto row in SelectedCountries)
                    {
                        await CountriesAppService.DeleteAsync(row.Id);
                        CountryList.Remove(row);
                    }
                }
                GridCountry.Reload();
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

        EditContext GridCountryEditContext
        {
            get { return GridCountry.IsEditing() ? _gridCountryEditContext : null; }
            set { _gridCountryEditContext = value; }
        }
        private async Task GridCountry_OnFocusedRowChanged(GridFocusedRowChangedEventArgs e)
        {
            if (GridCountry.IsEditing() && GridCountryEditContext.IsModified())
            {
                await GridCountry.SaveChangesAsync();
                IsDataEntryChanged = true;
            }
            else
                await GridCountry.CancelEditAsync();
        }

        private void GridCountry_OnRowClick(GridRowClickEventArgs e)
        {
            FocusedColumn = e.Column.Name;
        }

        private async Task GridCountry_OnRowDoubleClick(GridRowClickEventArgs e)
        {
            FocusedColumn = e.Column.Name;
            if (CanEditCountry)
            {
                await e.Grid.StartEditRowAsync(e.VisibleIndex);
                EditingCountry = (CountryDto)e.Grid.GetFocusedDataItem();
                EditingCountryId = EditingCountry.Id;
            }
        }
        private void GridCountry_OnCustomizeEditModel(GridCustomizeEditModelEventArgs e)
        {
            if (e.IsNew)
            {
                var newRow = (CountryDto)e.EditModel;
                newRow.Id = Guid.Empty;
                newRow.ConcurrencyStamp = string.Empty;

                if (GridCountry.GetVisibleRowCount() > 0)
                    newRow.Idx = CountryList.Max(x => x.Idx) + 1;
                else
                    newRow.Idx = 1;
            }
        }

        private void GridCountry_EditModelSaving(GridEditModelSavingEventArgs e)
        {
            CountryDto editModel = (CountryDto)e.EditModel;
            CountryDto dataItem = e.IsNew ? new CountryDto() : CountryList.Find(item => item.Idx == editModel.Idx);

            if (editModel != null && !e.IsNew)
            {
                editModel.IsChanged = true;
                CountryList.Remove(dataItem);
                CountryList.Add(editModel);
            }
            if (editModel != null && e.IsNew)
            {
                editModel.IsChanged = true;
                CountryList.Add(editModel);
            }
        }
        private async Task GridCountry_OnKeyDown(KeyboardEventArgs e)
        {
            if (e.Key == "F2")
                await GridCountry.StartEditRowAsync(GridCountry.GetFocusedRowIndex());
        }

        void GridCountry_CustomizeDataRowEditor(GridCustomizeDataRowEditorEventArgs e)
        {
            ((ITextEditSettings)e.EditSettings).ShowValidationIcon = true;
        }

        private async Task BtnAdd_GridCountry_OnClick()
        {
            await GridCountry.SaveChangesAsync();
            await GridCountry.StartEditNewRowAsync();
        }

        private async Task BtnDelete_GridCountry_OnClick()
        {
            await DeleteCountry();
        }

        #endregion

        //============================Application Functions======================================
        #region

        #endregion
    }
}
