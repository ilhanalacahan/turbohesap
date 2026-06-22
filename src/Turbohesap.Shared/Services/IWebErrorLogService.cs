using Turbohesap.Shared.Contracts.Diagnostics;

namespace Turbohesap.Shared.Services;

/// <summary>
/// Web (istemci) hatalarını kalıcılaştırma sözleşmesi. Uygulama API'dedir; sunucu hatalarıyla
/// aynı <c>error_logs</c> tablosunu kullanır ve aynı hash mantığıyla tekrarları birleştirir.
/// </summary>
public interface IWebErrorLogService
{
    Task<bool> LogAsync(LogWebErrorCommand command, CancellationToken cancellationToken = default);
}
