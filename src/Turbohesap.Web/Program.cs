using FluentValidation;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Turbohesap.Shared.Contracts.Auth;
using Turbohesap.Web.Components;
using Turbohesap.Web.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// API HTTP istemcisi (fabrika) + Polly dayanıklılık (req 7, 27).
var apiBaseUrl = builder.Configuration["Api:BaseUrl"] ?? "http://localhost:5080";
builder.Services.AddHttpClient(ApiClient.ClientName, client => client.BaseAddress = new Uri(apiBaseUrl))
    .AddStandardResilienceHandler();

// Kimlik / oturum (req 28).
builder.Services.AddDataProtection();
builder.Services.AddScoped<ProtectedLocalStorage>();
builder.Services.AddScoped<TokenStore>();
builder.Services.AddScoped<TurbohesapAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<TurbohesapAuthStateProvider>());
builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();

// Uygulama servisleri.
builder.Services.AddScoped<ApiClient>();
builder.Services.AddScoped<AuthClientService>();
builder.Services.AddScoped<CustomerApiService>();
builder.Services.AddScoped<ThemeInterop>();
builder.Services.AddScoped<ToastService>();
builder.Services.AddScoped<TabService>();
builder.Services.AddScoped<LayoutState>();
builder.Services.AddSingleton<AppNavigation>();

// FluentValidation: kurallar Shared'de; Blazored.FluentValidation DI'dan çözer (req 5, 14).
builder.Services.AddValidatorsFromAssemblyContaining<LoginRequestValidator>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseAntiforgery();
app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
