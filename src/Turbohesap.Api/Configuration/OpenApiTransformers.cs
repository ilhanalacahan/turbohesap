using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace Turbohesap.Api.Configuration;

/// <summary>
/// OpenAPI dokümanına JWT Bearer güvenlik şemasını ekler; Scalar arayüzünde "Authorize"
/// düğmesi görünür hale gelir. (Microsoft.OpenApi v2 API'si.)
/// </summary>
public sealed class BearerSecuritySchemeTransformer : IOpenApiDocumentTransformer
{
    public Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
    {
        document.Components ??= new OpenApiComponents();
        document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();

        document.Components.SecuritySchemes["Bearer"] = new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "JWT erişim token'ını girin."
        };

        document.Security ??= [];
        document.Security.Add(new OpenApiSecurityRequirement
        {
            [new OpenApiSecuritySchemeReference("Bearer", document)] = new List<string>()
        });

        return Task.CompletedTask;
    }
}

/// <summary>
/// Her uç noktanın açıklamasına erişebilen rolleri yazar (req 11, 12). Roller
/// <c>[Authorize(Roles = ...)]</c> meta verisinden okunur.
/// </summary>
public sealed class RoleDocumentationTransformer : IOpenApiOperationTransformer
{
    public Task TransformAsync(OpenApiOperation operation, OpenApiOperationTransformerContext context, CancellationToken cancellationToken)
    {
        var metadata = context.Description.ActionDescriptor.EndpointMetadata;

        var allowAnonymous = metadata.OfType<IAllowAnonymous>().Any();
        var roles = metadata.OfType<AuthorizeAttribute>()
            .Where(a => !string.IsNullOrWhiteSpace(a.Roles))
            .SelectMany(a => a.Roles!.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
            .Distinct()
            .ToList();

        string note;
        if (allowAnonymous)
        {
            note = "**Erişim:** Anonim (kimlik doğrulama gerektirmez).";
        }
        else if (roles.Count > 0)
        {
            note = $"**Erişebilen roller:** {string.Join(", ", roles)}";
        }
        else if (metadata.OfType<AuthorizeAttribute>().Any())
        {
            note = "**Erişim:** Kimliği doğrulanmış tüm kullanıcılar (rol kısıtı yok).";
        }
        else
        {
            note = "**Erişim:** Tanımsız (controller varsayılanı geçerlidir).";
        }

        operation.Description = string.IsNullOrWhiteSpace(operation.Description)
            ? note
            : $"{operation.Description}\n\n{note}";

        return Task.CompletedTask;
    }
}
