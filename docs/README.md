# BlazOrchard

This repo gives an example of how to set up a solution to run an OrchardCore Decoupled CMS, with a Blazor Web App front-end.

## Reproduction Steps

This is the easiest way I've found to get started:

1. Create a .NET Blazor Web App, with whatever interactivity you want.

2. In a separate window, create a new OrchardCore CMS App.

3. Copy `NLog.config` from the Orchard site, into the Blazor site.

4. Copy the settings from the Orchard site project file, into the Blazor site:

```xml
	<PropertyGroup>
		<AspNetCoreHostingModel>InProcess</AspNetCoreHostingModel>
	</PropertyGroup>
```
```xml
	<ItemGroup>
		<Watch 
		    Include="**\*.cs"
		    Exclude="Recipes\**;Assets\**;node_modules\**\*;**\*.js.map;obj\**\*;bin\**\*" />
	</ItemGroup>
```
```xml
	<ItemGroup>
		<PackageReference Include="OrchardCore.Logging.NLog" Version="1.8.2" />
		<PackageReference Include="OrchardCore.Application.Cms.Targets" Version="1.8.2" />
	</ItemGroup>
```

5. Add OrchardCore to the builder in  `Program.cs`, and **move** (*not copy*) your Blazor services and pipelines into the Orchard configuration.

```csharp
builder.Host.UseNLogHost();
```
```csharp
builder.Services
    .AddOrchardCms()
    .ConfigureServices(services =>
    {
        services.AddRazorComponents()
            .AddInteractiveServerComponents()
            .AddInteractiveWebAssemblyComponents();
    })
    .Configure((app, routes, _) =>
    {
        app.UseStaticFiles();
        app.UseAntiforgery();
        //routes.MapRazorComponents<App>()
        //    .AddInteractiveServerRenderMode()
        //    .AddInteractiveWebAssemblyRenderMode()
        //    .AddAdditionalAssemblies(typeof(BlazOrchard.BlazorWebApp.Client._Imports).Assembly);
    })
    ;  
```

This is the most counter-intuitive step. It's Orchard that will be handling, serving, and routing the pipelines, not Blazor. It uses the mappings within its own router, configured here. We've left it commented out here, so we can first set up OrchardCore.

6. Add the OrchardCore middleware, last in the chain, before running the app:  
  
```csharp
app.UseOrchardCore();
app.Run();
```

7. Run the application, and you'll see the familiar OrchardCore setup screen. Go through setup as normal. You can choose any recipe you like, as the front-end will be re-routed anyway. It's common to choose Headless CMS here, so that no front-end theme is added at all.

8. Uncomment the Blazor mappings from the Orchard router, and launch your app. You should see a fully working Blazor sample app, with working counter, and weather pages.

9. Head to `/admin` to log into the OrchardCore backend.