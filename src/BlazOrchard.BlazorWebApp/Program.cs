using BlazOrchard.BlazorWebApp.Components;
using OrchardCore.Logging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Host.UseNLogHost();
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
        routes.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode()
            .AddInteractiveWebAssemblyRenderMode()
            .AddAdditionalAssemblies(typeof(BlazOrchard.BlazorWebApp.Client._Imports).Assembly);
    })
    ;

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseOrchardCore();
app.Run();