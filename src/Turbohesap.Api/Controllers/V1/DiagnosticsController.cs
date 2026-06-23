using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Turbohesap.Shared.Contracts.Diagnostics;
using Turbohesap.Shared.Security;
using Wolverine;

namespace Turbohesap.Api.Controllers.V1;

/// <summary>
/// Tanılama uç noktaları. Web (Blazor) tarafında yakalanan istemci hatalarını alır ve
/// sunucu hatalarıyla aynı <c>error_logs</c> tablosuna işler. Hata raporlaması oturum
/// gerektirmez (kullanıcı giriş yapmamışken de hata oluşabilir).
/// </summary>
[ApiVersion(ApiVersions.V1)]
public sealed class DiagnosticsController(IMessageBus bus) : ApiControllerBase(bus)
{
    /// <summary>Web istemci hatasını kaydeder (Source = "Web").</summary>
    [HttpPost("client-errors")]
    [AllowAnonymous]
    [ProducesResponseType<bool>(StatusCodes.Status200OK)]
    public async Task<ActionResult<bool>> LogClientError(
        [FromBody] WebErrorReport report, CancellationToken cancellationToken)
    {
        var command = new LogWebErrorCommand
        {
            Message = report.Message,
            ExceptionType = report.ExceptionType,
            StackTrace = report.StackTrace,
            Source = report.Source,
            Path = report.Path,
            IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
            UserAgent = HttpContext.Request.Headers.UserAgent.ToString()
        };

        return Ok(await Bus.InvokeAsync<bool>(command, cancellationToken));
    }
}
