using Mapster;
using MapsterMapper;

namespace Turbohesap.Api.Mapping;

/// <summary>
/// Mapster eşleme sisteminin ana giriş noktası (req 8). Her entity'nin <see cref="IRegister"/>
/// uygulaması bu assembly'den taranıp tek bir <see cref="TypeAdapterConfig"/>'e uygulanır;
/// ardından <see cref="IMapper"/> (ServiceMapper) DI'a eklenir.
/// </summary>
public static class MappingRegistry
{
    public static IServiceCollection AddApplicationMapping(this IServiceCollection services)
    {
        var config = TypeAdapterConfig.GlobalSettings;

        // Her entity için ayrı IRegister mapping dosyası taranır (req 8).
        config.Scan(typeof(MappingRegistry).Assembly);

        services.AddSingleton(config);
        services.AddScoped<IMapper, ServiceMapper>();

        return services;
    }
}
