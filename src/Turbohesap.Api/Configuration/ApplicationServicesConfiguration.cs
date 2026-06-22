using FluentValidation;
using Turbohesap.Api.Authentication;
using Turbohesap.Api.Common;
using Turbohesap.Api.Mapping;
using Turbohesap.Api.Services;
using Turbohesap.Shared.Contracts.Auth;
using Turbohesap.Shared.Services;

namespace Turbohesap.Api.Configuration;

/// <summary>
/// Uygulama servisleri, eşleme (Mapster) ve doğrulayıcıların (FluentValidation) DI kaydı.
/// Doğrulayıcılar Shared'de tutulduğundan iki proje aynı kuralları paylaşır (req 5, 8, 14).
/// </summary>
public static class ApplicationServicesConfiguration
{
    public static WebApplicationBuilder AddApplicationServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddHttpContextAccessor();

        // İstek bağlamı
        builder.Services.AddScoped<ICurrentUser, CurrentUser>();

        // İş servisleri (transactional mantık — req 10)
        builder.Services.AddScoped<ICustomerService, CustomerService>();
        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddSingleton<JwtTokenService>();

        // Mapster eşleme sistemi (req 8)
        builder.Services.AddApplicationMapping();

        // FluentValidation: kuralların yaşadığı Shared assembly'sini tara (req 5, 14)
        builder.Services.AddValidatorsFromAssemblyContaining<LoginRequestValidator>();

        return builder;
    }
}
