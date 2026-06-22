using Serilog;
using Turbohesap.Api.Configuration;
using Turbohesap.Api.Middleware;
using Turbohesap.Api.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Çapraz kesen tüm yapılandırmalar uzantı metotlarına taşındı (req 15).
builder
    .AddSerilogLogging()      // Serilog (req 7)
    .AddPersistence()         // PostgreSQL + EF Core + snake_case (req 1, 4)
    .AddAuditing()            // Audit.NET, EF seviyesinde (req 3)
    .AddApplicationServices() // Servisler + Mapster + FluentValidation (req 5, 8, 10, 14)
    .AddMessaging()           // Wolverine CQRS + olay veriyolu (req 6)
    .AddJwtAuthentication()   // JWT Bearer (req 11)
    .AddApiDocumentation()    // Controller + sürümleme + Scalar (req 12)
    .AddTelemetry();          // OpenTelemetry (req 9)

var app = builder.Build();

app.UseMiddleware<GlobalExceptionMiddleware>(); // Küresel hata yakalama (req 2)
app.UseSerilogRequestLogging();
app.UseAuthentication();
app.UseAuthorization();
app.UseAuditing();                              // Denetim için HTTP bağlamını bağlar
app.UseApiDocumentation();                      // /openapi/v1.json + /scalar
app.MapControllers();

await DbSeeder.SeedAsync(app.Services);

app.Run();
