using Blazorise;
using DevExpress.Blazor;
using HQSOFT.SharedInformation.States;
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
using HQSOFT.SharedInformation.Countries;
using Volo.Abp;

namespace HQSOFT.SharedInformation.Blazor.Pages.SharedInformation.State
{
    public partial class States
    {
        protected List<Volo.Abp.BlazoriseUI.BreadcrumbItem> BreadcrumbItems = new List<Volo.Abp.BlazoriseUI.BreadcrumbItem>();
        protected PageToolbar Toolbar { get; } = new PageToolbar();
        private int PageSize { get; } = LimitedResultRequestDto.DefaultMaxResultCount;
        private int MaxCount { get; } = 1000;
        private bool CanCreateState { get; set; }
        private bool CanEditState { get; set; }
        private bool CanDeleteState { get; set; }
        private StateDto EditingState { get; set; } = new StateDto();  //Editing row on grid
        private Guid EditingStateId { get; set; } = Guid.Empty; //Editing State Id on grid
        private List<StateDto> StateList { get; set; } = new List<StateDto>(); //Data source used to bind to grid
        private IReadOnlyList<object> SelectedStates { get; set; } = new List<StateDto>(); //Selected rows on grid
        private GetStatesInput Filter { get; set; } //Used for Search box
        private IGrid GridState { get; set; } //Id of State grid control name
        private bool IsDataEntryChanged { get; set; } //keep value to indicate data has been changed or not
        private IReadOnlyList<CountryDto> CountriesCollection { get; set; } = new List<CountryDto>();
        private CountryDto SelectedCountry { get; set; } = new CountryDto();
        string FocusedColumn { get; set; }
        private EditContext _GridStateEditContext { get; set; } //Injected Editcontext of State grid

        private readonly IUiMessageService _uiMessageService; //Injected UIMessage

        //==================================Initialize Section===================================
        #region
        public States(IUiMessageService uiMessageService)
        {
            Filter = new GetStatesInput
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
            await GetStatesAsync();
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
            CanCreateState = await AuthorizationService.IsGrantedAsync(SharedInformationPermissions.States.Create);
            CanEditState = await AuthorizationService.IsGrantedAsync(SharedInformationPermissions.States.Edit);
            CanDeleteState = await AuthorizationService.IsGrantedAsync(SharedInformationPermissions.States.Delete);

        }

        protected virtual ValueTask SetBreadcrumbItemsAsync()
        {
            BreadcrumbItems.Add(new Volo.Abp.BlazoriseUI.BreadcrumbItem(L["Menu:GeographicalSubdivisions"]));
            BreadcrumbItems.Add(new Volo.Abp.BlazoriseUI.BreadcrumbItem(L["Menu:States"]));
            return ValueTask.CompletedTask;
        }

        protected virtual ValueTask SetToolbarItemsAsync()
        {
            Toolbar.AddButton(L["Save"], async () =>
            {
                await SaveStateAsync();
            },
            IconName.Save,
            Color.Primary,
            requiredPolicyName: SharedInformationPermissions.States.Edit, disabled: !CanEditState);

            Toolbar.AddButton(L["Delete"], DeleteState,
            IconName.Delete,
            Color.Danger,
            requiredPolicyName: SharedInformationPermissions.States.Delete, disabled: !CanDeleteState);

            return ValueTask.CompletedTask;
        }
        #endregion

        //======================Load Data Source for ListView & Others===========================
        #region
        private async Task GetCountryCollectionLookupAsync()
        {
            CountriesCollection = (await CountriesAppService.GetListAsync(new GetCountriesInput { MaxResultCount = MaxCount, })).Items;
        }
        #endregion

        //======================CRUD & Load Main Data Source Section=============================
        #region
        private async Task GetStatesAsync()
        {
            Filter.CountryId = SelectedCountry.Id;
            Filter.MaxResultCount = MaxCount;
            var result = await StatesAppService.GetListAsync(Filter);

            StateList = (List<StateDto>)result.Items;
            IsDataEntryChanged = false;
            await InvokeAsync(StateHasChanged);
        }

        private async Task SaveStateAsync()
        {
            try
            {
                await GridState.SaveChangesAsync();

                foreach (var State in StateList)
                {
                    if (State.ConcurrencyStamp == string.Empty)
                        await StatesAppService.CreateAsync(ObjectMapper.Map<StateDto, StateCreateDto>(State));
                    else if (State.IsChanged)
                        await StatesAppService.UpdateAsync(State.Id, ObjectMapper.Map<StateDto, StateUpdateDto>(State));
                }

                await GetStatesAsync();
                await InvokeAsync(StateHasChanged);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
            }
        }
        private async Task DeleteState()
        {
            var confirmed = await _uiMessageService.Confirm(L["DeleteConfirmationMessage"]);
            if (confirmed)
            {
                if (SelectedStates != null)
                {
                    foreach (StateDto row in SelectedStates)
                    {
                        await StatesAppService.DeleteAsync(row.Id);
                        StateList.Remove(row);
                    }
                }
                GridState.Reload();
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

        async Task SelectedCountryChanged(CountryDto country)
        {
            SelectedCountry = country;
            await GetStatesAsync();
        }
        EditContext GridStateEditContext
        {
            get { return GridState.IsEditing() ? _GridStateEditContext : null; }
            set { _GridStateEditContext = value; }
        }
        private async Task GridState_OnFocusedRowChanged(GridFocusedRowChangedEventArgs e)
        {
            if (GridState.IsEditing() && GridStateEditContext.IsModified())
            {
                await GridState.SaveChangesAsync();
                IsDataEntryChanged = true;
            }
            else
                await GridState.CancelEditAsync();
        }

        private async Task GridState_OnRowDoubleClick(GridRowClickEventArgs e)
        {
            if (CanEditState)
            {
                FocusedColumn = e.Column.Name;
                await e.Grid.StartEditRowAsync(e.VisibleIndex);
                EditingState = (StateDto)e.Grid.GetFocusedDataItem();
                EditingStateId = EditingState.Id;
            }
        }
        private void GridState_OnCustomizeEditModel(GridCustomizeEditModelEventArgs e)
        {
            if (e.IsNew)
            {
                if (SelectedCountry == null || SelectedCountry.Id == Guid.Empty)
                    throw new UserFriendlyException(L["CountrySelect"]);

                var newRow = (StateDto)e.EditModel;

                newRow.CountryId = SelectedCountry.Id;
                newRow.Id = Guid.Empty;
                newRow.ConcurrencyStamp = string.Empty;

                if (GridState.GetVisibleRowCount() > 0)
                    newRow.Idx = StateList.Max(x => x.Idx) + 1;
                else
                    newRow.Idx = 1;
            }
        }

        private void GridState_EditModelSaving(GridEditModelSavingEventArgs e)
        {
            StateDto editModel = (StateDto)e.EditModel;
            StateDto dataItem = e.IsNew ? new StateDto() : StateList.Find(item => item.Idx == editModel.Idx);

            if (editModel != null && !e.IsNew)
            {
                editModel.IsChanged = true;
                StateList.Remove(dataItem);
                StateList.Add(editModel);
            }
            if (editModel != null && e.IsNew)
            {
                editModel.IsChanged = true;
                StateList.Add(editModel);
            }
        }
        private async Task GridState_OnKeyDown(KeyboardEventArgs e)
        {
            if (e.Key == "F2" && CanEditState)
                await GridState.StartEditRowAsync(GridState.GetFocusedRowIndex());
        }

        void GridState_CustomizeDataRowEditor(GridCustomizeDataRowEditorEventArgs e)
        {
            ((ITextEditSettings)e.EditSettings).ShowValidationIcon = true;
        }
        private async Task BtnAdd_GridState_OnClick()
        {
            await GridState.SaveChangesAsync();
            await GridState.StartEditNewRowAsync();
        }

        private async Task BtnDelete_GridState_OnClick()
        {
            await DeleteState();
        }

        #endregion

        //============================Application Functions======================================
        #region

        #endregion
    }
}
