using Microsoft.AspNetCore.Components;
using OrchardCore.ContentManagement;
using OrchardCore.Settings;

namespace BlazOrchard.BlazorWebApp.Components.Abstractions;

public class OrchardComponentBase : ComponentBase
{
    [Inject]
    protected ISiteService SiteService { get; init; }

    [Inject]
    protected IContentHandleManager HandleManager { get; init; }

    [Inject]
    protected IContentManager ContentManager { get; init; }

    protected ISite Site { get; private set; }

    protected override async Task OnInitializedAsync()
    {
        Site = await SiteService.GetSiteSettingsAsync();
    }
}