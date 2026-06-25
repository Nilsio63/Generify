using Blazored.LocalStorage;
using Generify.Components;
using Generify.External.Extensions.DependencyInjection;
using Generify.External.Settings;
using Generify.Repositories.Extensions.DependencyInjection;
using Generify.Services;
using Generify.Services.Abstractions.Management;
using Generify.Services.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add MudBlazor services
builder.Services.AddMudServices();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddControllers();

// Add generify services
builder.Services.AddGenerifyRepos(dbOptions =>
{
    string? connectionString = builder.Configuration.GetConnectionString("Default");

    if (string.IsNullOrWhiteSpace(connectionString))
    {
        throw new ArgumentException("MySql DB configuration is missing in configuration");
    }

    dbOptions.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

builder.Services.AddGenerifyServices();

builder.Services.AddExternalServices(s =>
{
    HttpContext httpContext = s.GetRequiredService<IHttpContextAccessor>().HttpContext!;

    string hostAddress = httpContext.Request.Host.ToString().Replace("https://", "").Replace("localhost", "127.0.0.1").TrimEnd('/');

    string? clientId = builder.Configuration
        .GetSection("Generify")
        .GetSection("External")
        .GetValue<string>("ClientId");

    string? clientSecret = builder.Configuration
        .GetSection("Generify")
        .GetSection("External")
        .GetValue<string>("ClientSecret");

    if (string.IsNullOrWhiteSpace(clientId) || string.IsNullOrWhiteSpace(clientSecret))
    {
        throw new ArgumentException("Client ID or Client Secret missing in configuration");
    }

    return new ExternalAuthSettings(clientId, clientSecret, $"https://{hostAddress}/api/auth/authCallback");
});

// Add web dependencies
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddHttpContextAccessor();

// Add custom authentication
//builder.Services.AddAuthorization();
//builder.Services.AddCascadingAuthenticationState();
builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/login";
        options.LogoutPath = "/logout";
    });
builder.Services.AddTransient<IUserAuthService, UserAuthService>();
builder.Services.AddTransient<IUserContextAccessor, UserAuthService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();
app.MapControllers();

app.Run();
