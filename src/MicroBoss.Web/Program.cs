using MicroBoss.Application;
using MicroBoss.Infrastructure;
using MicroBoss.Infrastructure.Identity;
using MicroBoss.Web.Components;
using MicroBoss.Web.Middleware;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/microboss-.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.Host.UseSerilog();
    builder.Services.AddDevExpressBlazor();
    builder.Services.AddRazorComponents()
        .AddInteractiveServerComponents();
    builder.Services.AddControllers();
    builder.Services.AddCascadingAuthenticationState();
    builder.Services
        .AddApplication()
        .AddInfrastructure(builder.Configuration);

    var app = builder.Build();

    using (var scope = app.Services.CreateScope())
    {
        await IdentitySetup.SeedRolesAsync(scope.ServiceProvider);
        await IdentitySetup.SeedDefaultAdminAsync(scope.ServiceProvider);
    }

    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error", createScopeForErrors: true);
        app.UseHsts();
    }

    app.UseMiddleware<GlobalExceptionMiddleware>();
    app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseAntiforgery();
    app.MapStaticAssets();
    app.MapControllers();
    app.MapRazorComponents<App>()
        .AddInteractiveServerRenderMode();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
