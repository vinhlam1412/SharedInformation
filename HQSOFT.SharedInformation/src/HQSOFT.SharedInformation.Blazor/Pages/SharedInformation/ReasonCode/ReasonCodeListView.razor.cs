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
using HQSOFT.SharedInformation.ReasonCodes;
using HQSOFT.SharedInformation.Permissions;
using HQSOFT.SharedInformation.Shared;
using Volo.Abp.AspNetCore.Components.Messages;
using Microsoft.JSInterop;
using HQSOFT.eBiz.GeneralLedger.Accounts;

namespace HQSOFT.SharedInformation.Blazor.Pages.SharedInformation.ReasonCode
{
    public partial class ReasonCodeListView
    {
        protected List<Volo.Abp.BlazoriseUI.BreadcrumbItem> BreadcrumbItems = new List<Volo.Abp.BlazoriseUI.BreadcrumbItem>();
        protected PageToolbar Toolbar { get; } = new PageToolbar();
        //private IReadOnlyList<ReasonCodeWithNavigationPropertiesDto> ReasonCodeList { get; set; }
        //private List<ReasonCodeWithNavigationPropertiesDto> SelectedReasonCodes { get; set; }
        private IReadOnlyList<ReasonCodeDto> ReasonCodeList { get; set; }
        private List<ReasonCodeDto> SelectedReasonCodes { get; set; }
        private int PageSize { get; } = LimitedResultRequestDto.DefaultMaxResultCount;
        private int CurrentPage { get; set; } = 1;
        private string CurrentSorting { get; set; } = string.Empty;
        private int TotalCount { get; set; }
        private bool CanCreateReasonCode { get; set; }
        private bool CanEditReasonCode { get; set; }
        private bool CanDeleteReasonCode { get; set; }
        private GetReasonCodesInput Filter { get; set; }
        private IReadOnlyList<AccountDto> AccountsCollection { get; set; } = new List<AccountDto>();
        private readonly IUiMessageService _uiMessageService;
        private Guid accountId;


        public ReasonCodeListView(IUiMessageService uiMessageService)
        {
            Filter = new GetReasonCodesInput
            {
                MaxResultCount = PageSize,
                SkipCount = (CurrentPage - 1) * PageSize,
                Sorting = CurrentSorting
            };
            _uiMessageService = uiMessageService;   
        }

        protected override async Task OnInitializedAsync()
        {
            await SetToolbarItemsAsync();
            await SetBreadcrumbItemsAsync();
            await SetPermissionsAsync();
            await GetAccountCollectionLookupAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            base.OnAfterRender(firstRender);
            await JSRuntime.InvokeVoidAsync("AssignGotFocus");
        }

        protected virtual ValueTask SetBreadcrumbItemsAsync()
        {
            BreadcrumbItems.Add(new Volo.Abp.BlazoriseUI.BreadcrumbItem(L["Menu:ReasonCodes"]));
            return ValueTask.CompletedTask;
        }

        protected virtual ValueTask SetToolbarItemsAsync()
        {
            Toolbar.AddButton(L["ExportToExcel"], async () => { await DownloadAsExcelAsync(); }, IconName.Download);

            Toolbar.AddButton(L["New"], () =>
            {
                NavigationManager.NavigateTo($"/SharedInformation/ReasonCodes/{Guid.Empty}");
                return Task.CompletedTask;
            }, IconName.Add, requiredPolicyName: SharedInformationPermissions.ReasonCodes.Create);

            Toolbar.AddButton(L["Delete"], async () =>
            {
                if (SelectedReasonCodes.Count > 0)
                {
                    var confirmed = await _uiMessageService.Confirm(L["DeleteConfirmationMessage"]);
                    if (confirmed)
                    {
                        foreach (ReasonCodeDto SelectedReasonCode in SelectedReasonCodes)
                        {
                            await ReasonCodesAppService.DeleteAsync(SelectedReasonCode.Id);
                        }
                        await GetReasonCodesAsync();
                    }

                }
            }, IconName.Delete,
            Color.Danger,
            requiredPolicyName: SharedInformationPermissions.ReasonCodes.Delete);
            return ValueTask.CompletedTask;
        }

        private async Task SetPermissionsAsync()
        {
            CanCreateReasonCode = await AuthorizationService
                .IsGrantedAsync(SharedInformationPermissions.ReasonCodes.Create);
            CanEditReasonCode = await AuthorizationService
                            .IsGrantedAsync(SharedInformationPermissions.ReasonCodes.Edit);
            CanDeleteReasonCode = await AuthorizationService
                            .IsGrantedAsync(SharedInformationPermissions.ReasonCodes.Delete);
        }

        private async Task GetAccountCollectionLookupAsync(string? newValue = null)
        {
            AccountsCollection = (await AccountsAppService.GetListAsync(new GetAccountsInput { FilterText = newValue })).Items;
        }

        private async Task GetReasonCodesAsync()
        {
            Filter.MaxResultCount = PageSize;
            Filter.SkipCount = (CurrentPage - 1) * PageSize;
            Filter.Sorting = CurrentSorting;

            var result = await ReasonCodesAppService.GetListAsync(Filter);
            ReasonCodeList = result.Items;
            TotalCount = (int)result.TotalCount;
        }

        protected virtual async Task SearchAsync()
        {
            CurrentPage = 1;
            await GetReasonCodesAsync();
            await InvokeAsync(StateHasChanged);
        }

        private async Task DownloadAsExcelAsync()
        {
            var token = (await ReasonCodesAppService.GetDownloadTokenAsync()).Token;
            var remoteService = await RemoteServiceConfigurationProvider.GetConfigurationOrDefaultOrNullAsync("SharedInformation") ??
            await RemoteServiceConfigurationProvider.GetConfigurationOrDefaultOrNullAsync("Default");
            NavigationManager.NavigateTo($"{remoteService?.BaseUrl.EnsureEndsWith('/') ?? string.Empty}api/shared-information/reason-codes/as-excel-file?DownloadToken={token}&FilterText={Filter.FilterText}", forceLoad: true);
        }

        private async Task OnDataGridReadAsync(DataGridReadDataEventArgs<ReasonCodeDto> e)
        {
            CurrentSorting = e.Columns
                .Where(c => c.SortDirection != SortDirection.Default)
                .Select(c => c.Field + (c.SortDirection == SortDirection.Descending ? " DESC" : ""))
                .JoinAsString(",");
            CurrentPage = e.Page;
            await GetReasonCodesAsync();
            await InvokeAsync(StateHasChanged);
        }
        private async Task DeleteReasonCodeAsync(ReasonCodeDto input)
        {
            await ReasonCodesAppService.DeleteAsync(input.Id);
            await GetReasonCodesAsync();
        }

        protected void GoToEditPage(ReasonCodeDto context)
        {
            NavigationManager.NavigateTo($"SharedInformation/ReasonCodes/{context.Id}");
        }

    }
}
