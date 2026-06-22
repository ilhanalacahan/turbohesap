using Wolverine.Attributes;

namespace Turbohesap.Api.Features.Customers;

/// <summary>Müşteri oluşturulduğunda yayınlanan olay (Wolverine veriyolu — req 6).</summary>
public sealed record CustomerCreatedEvent(Guid Id, string Code, string Name);

/// <summary>Müşteri güncellendiğinde yayınlanan olay.</summary>
public sealed record CustomerUpdatedEvent(Guid Id, string Code, string Name);

/// <summary>Müşteri silindiğinde yayınlanan olay.</summary>
public sealed record CustomerDeletedEvent(Guid Id, string Code);

/// <summary>
/// Müşteri olaylarının dinleyicisi. Wolverine, <c>Handle</c> metotlarını sözleşmeyle keşfeder.
/// Burada yalnızca loglanır; gerçek projede e-posta, cache temizleme vb. tetiklenebilir.
/// </summary>
[WolverineHandler]
public sealed class CustomerEventHandlers
{
    public void Handle(CustomerCreatedEvent @event, ILogger<CustomerEventHandlers> logger)
        => logger.LogInformation("Müşteri oluşturuldu: {Code} - {Name} ({Id})", @event.Code, @event.Name, @event.Id);

    public void Handle(CustomerUpdatedEvent @event, ILogger<CustomerEventHandlers> logger)
        => logger.LogInformation("Müşteri güncellendi: {Code} - {Name} ({Id})", @event.Code, @event.Name, @event.Id);

    public void Handle(CustomerDeletedEvent @event, ILogger<CustomerEventHandlers> logger)
        => logger.LogInformation("Müşteri silindi: {Code} ({Id})", @event.Code, @event.Id);
}
