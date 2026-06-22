using Turbohesap.Shared.Contracts.Diagnostics;
using Turbohesap.Shared.Services;
using Wolverine.Attributes;

namespace Turbohesap.Api.Features.Diagnostics;

/// <summary>
/// Tanılama (diagnostics) CQRS handler'ları. Web istemci hatasını ince bir sınır olarak
/// <see cref="IWebErrorLogService"/>'e delege eder; iş mantığı serviste kalır (req 6, 10).
/// </summary>
[WolverineHandler]
public sealed class DiagnosticsHandlers
{
    public static Task<bool> Handle(
        LogWebErrorCommand command, IWebErrorLogService service, CancellationToken cancellationToken)
        => service.LogAsync(command, cancellationToken);
}
