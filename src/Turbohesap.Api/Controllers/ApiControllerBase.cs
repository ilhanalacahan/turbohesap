using Microsoft.AspNetCore.Mvc;
using Turbohesap.Shared.Security;
using Wolverine;

namespace Turbohesap.Api.Controllers;

/// <summary>
/// Tüm API controller'larının ortak temeli. Standart rota (<c>api/v{version}/[controller]</c>),
/// JSON üretimi ve Wolverine mesaj veriyoluna erişim sağlar (req 6, 11, 12).
/// </summary>
[ApiController]
[Produces("application/json")]
[Route(ApiVersions.RoutePrefix + "/[controller]")]
public abstract class ApiControllerBase(IMessageBus bus) : ControllerBase
{
    /// <summary>CQRS komut/sorgu göndermek için Wolverine veriyolu.</summary>
    protected IMessageBus Bus { get; } = bus;
}
