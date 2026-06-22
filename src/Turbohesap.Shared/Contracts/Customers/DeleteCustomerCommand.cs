using Turbohesap.Shared.Cqrs;

namespace Turbohesap.Shared.Contracts.Customers;

/// <summary>Müşteriyi siler. İşlem başarılıysa true döner.</summary>
public sealed class DeleteCustomerCommand : ICommand<bool>
{
    public Guid Id { get; set; }

    public DeleteCustomerCommand() { }

    public DeleteCustomerCommand(Guid id) => Id = id;
}
