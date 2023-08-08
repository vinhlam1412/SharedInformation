using Blazorise;
using DevExpress.Blazor;
using HQSOFT.SharedInformation.ReasonCodes;
using HQSOFT.eBiz.GeneralLedger.Accounts;
using HQSOFT.SharedInformation.Permissions;
using HQSOFT.eBiz.GeneralLedger.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
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
using DevExpress.Pdf.Native.BouncyCastle.Asn1.X509;

namespace HQSOFT.SharedInformation.Blazor.Pages.SharedInformation.ReasonCode
{
    public partial class ReasonCodes
    {
        protected List<Volo.Abp.BlazoriseUI.BreadcrumbItem> BreadcrumbItems = new List<Volo.Abp.BlazoriseUI.BreadcrumbItem>();
        protected PageToolbar Toolbar { get; } = new PageToolbar();
        private int PageSize { get; } = LimitedResultRequestDto.DefaultMaxResultCount;
        private int MaxCount { get; } = 1000;
        private bool CanCreateReasonCode { get; set; }
        private bool CanEditReasonCode { get; set; }
        private bool CanDeleteReasonCode { get; set; }
        private bool CanCreateReasonCodeLine { get; set; }
        private bool CanEditReasonCodeLine { get; set; }
        private bool CanDeleteReasonCodeLine { get; set; }

        private ReasonCodeDto EditingReasonCode { get; set; } //Current editting Order 
        private Guid EditingReasonCodeId { get; set; } //Current edditing Order Id
        private EditForm EditFormMain { get; set; } //Id of Main form
        private bool IsDataEntryChanged { get; set; } //keep value to indicate data has been changed or not
        private float TotalAmount { get; set; } //keep total amount of sales order
        private IReadOnlyList<AccountDto> AccountsCollection { get; set; } = new List<AccountDto>();
        private IReadOnlyList<ReasonCodeTypeList> ReasonCodeTypeCollection { get; set; } = new List<ReasonCodeTypeList>();

        private readonly IUiMessageService _uiMessageService; //Injected UIMessage
 
        [Parameter]
        public string Id { get; set; }



        //==================================Initialize Section===================================
        #region
        public ReasonCodes(IUiMessageService uiMessageService)
        {

            EditingReasonCode = new ReasonCodeDto
            {
                ConcurrencyStamp = string.Empty,

            };

            _uiMessageService = uiMessageService;
        }

        protected override async Task OnInitializedAsync()
        {
            await SetPermissionsAsync();
            await SetToolbarItemsAsync();
            await SetBreadcrumbItemsAsync();
            await GetReasonCodeTypeCollectionLookupAsync();
            await GetAccountCollectionLookupAsync();

            EditingReasonCodeId = Guid.Parse(Id);
            await LoadDataAsync(EditingReasonCodeId);

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
            CanCreateReasonCode = await AuthorizationService.IsGrantedAsync(SharedInformationPermissions.ReasonCodes.Create);
            CanEditReasonCode = await AuthorizationService.IsGrantedAsync(SharedInformationPermissions.ReasonCodes.Edit);
            CanDeleteReasonCode = await AuthorizationService.IsGrantedAsync(SharedInformationPermissions.ReasonCodes.Delete);

        }

        protected virtual ValueTask SetBreadcrumbItemsAsync()
        {
            BreadcrumbItems.Add(new Volo.Abp.BlazoriseUI.BreadcrumbItem(L["Menu:ReasonCodes"]));
            return ValueTask.CompletedTask;
        }

        protected virtual ValueTask SetToolbarItemsAsync()
        {
            Toolbar.AddButton(L["Back"], async () =>
            {
                NavigationManager.NavigateTo($"/SharedInformation/ReasonCodes");
            },
            IconName.Undo,
            Color.Secondary);

            Toolbar.AddButton(L["Save"], async () =>
            {
                await SaveReasonCodeAsync(false);
            },
            IconName.Save,
            Color.Primary,
            requiredPolicyName: SharedInformationPermissions.ReasonCodes.Edit, disabled: !CanEditReasonCode);

            Toolbar.AddButton(L["New"], async () =>
            {
                await SaveReasonCodeAsync(true);
            }, IconName.Add,
            Color.Primary,
            requiredPolicyName: SharedInformationPermissions.ReasonCodes.Create, disabled: !CanCreateReasonCode);

            Toolbar.AddButton(L["Delete"], DeleteReasonCode,
            IconName.Delete,
            Color.Danger,
            requiredPolicyName: SharedInformationPermissions.ReasonCodes.Delete, disabled: !CanDeleteReasonCode);

            return ValueTask.CompletedTask;
        }
        #endregion

        //======================Load Data Source for ListView & Others===========================
        #region
        private async Task GetAccountCollectionLookupAsync()
        {
           AccountsCollection = (await AccountsAppService.GetListAsync(new GetAccountsInput { MaxResultCount = MaxCount, })).Items;
        }

        private async Task GetReasonCodeTypeCollectionLookupAsync()
        {
            ReasonCodeTypeCollection = Enum.GetValues(typeof(ReasonCodeType))
            .OfType<ReasonCodeType>()
            .Select(t => new ReasonCodeTypeList()
            {
                Value = t.ToString(),
                DisplayName = L["ReasonCodeType." + t.ToString()],
            }).ToList();
        }
        #endregion

        //======================CRUD & Load Main Data Source Section=============================
        #region
        private async Task LoadDataAsync(Guid orderId)
        {
            if (orderId != Guid.Empty)
            {
                EditingReasonCode = await ReasonCodesAppService.GetAsync(orderId);
            }
        }
        private void NewOrder()
        {
            EditingReasonCode = new ReasonCodeDto
            {
                ConcurrencyStamp = string.Empty,

            };
            EditingReasonCodeId = Guid.Empty;
            IsDataEntryChanged = false;
            NavigationManager.NavigateTo($"/SharedInformation/ReasonCodes/{Guid.Empty}");
        }
        private async Task SaveReasonCodeAsync(bool IsNewNext)
        {
            try
            {
                if (!EditFormMain.EditContext.Validate())
                {
                    return;
                }

                if (EditingReasonCodeId == Guid.Empty)
                {
                    var order = await ReasonCodesAppService.CreateAsync(ObjectMapper.Map<ReasonCodeDto, ReasonCodeCreateDto>(EditingReasonCode));
                    EditingReasonCodeId = order.Id;
                }
                else
                {
                    EditingReasonCode = await ReasonCodesAppService.UpdateAsync(EditingReasonCodeId, ObjectMapper.Map<ReasonCodeDto, ReasonCodeUpdateDto>(EditingReasonCode));
                    //EditingReasonCode = await ReasonCodesAppService.GetAsync(EditingReasonCodeId);
                }

                if (IsNewNext)
                    NewOrder();
                else
                {
                    IsDataEntryChanged = false;
                    NavigationManager.NavigateTo($"/SharedInformation/ReasonCodes/{EditingReasonCodeId}");
                }
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
            }

        }

        private async Task DeleteReasonCode()
        {
            var confirmed = await _uiMessageService.Confirm(L["DeleteConfirmationMessage"]);
            if (confirmed)
            {
                await ReasonCodesAppService.DeleteAsync(EditingReasonCodeId);
                NavigationManager.NavigateTo("/SharedInformation/ReasonCodes");
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
 
        #endregion

        //============================Application Functions======================================
        #region

        #endregion
    }
}
