using Asp.Versioning;
using Scalar.AspNetCore;

namespace Turbohesap.Api.Configuration;

/// <summary>
/// Controller'lar, API sürümleme (req 12) ve OpenAPI/Scalar dokümantasyonu (req 12).
/// Tüm uç noktalar <c>api/v{version}/...</c> kalıbında standardize edilir.
/// </summary>
public static class ApiDocumentationConfiguration
{
    public static WebApplicationBuilder AddApiDocumentation(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();

        builder.Services
            .AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
            })
            .AddMvc()
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

        builder.Services.AddOpenApi("v1", options =>
        {
            options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
            options.AddOperationTransformer<RoleDocumentationTransformer>();
        });

        return builder;
    }

    public static WebApplication UseApiDocumentation(this WebApplication app)
    {
        // OpenAPI JSON: /openapi/v1.json
        app.MapOpenApi();

        // Scalar arayüzü: /scalar
        app.MapScalarApiReference(options =>
        {
            options
                .WithTitle("Turbohesap API")
                .WithTheme(ScalarTheme.Default)
                .EnableDarkMode()
                .AddDocument("v1", "Turbohesap API v1");
        });

        return app;
    }
}
