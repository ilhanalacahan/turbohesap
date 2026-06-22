using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Turbohesap.Shared.Common;
using Turbohesap.Shared.Contracts.Auth;
using Turbohesap.Shared.Security;
using Wolverine;

namespace Turbohesap.Api.Controllers.V1;

/// <summary>Kimlik doğrulama uç noktaları.</summary>
[ApiVersion(ApiVersions.V1)]
public sealed class AuthController(IMessageBus bus) : ApiControllerBase(bus)
{
    /// <summary>E-posta/parola ile giriş yapar ve JWT erişim token'ı döner.</summary>
    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType<LoginResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetail>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetail>(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<LoginResponse>> Login(
        [FromBody] LoginRequest request, CancellationToken cancellationToken)
        => Ok(await Bus.InvokeAsync<LoginResponse>(request, cancellationToken));
}
