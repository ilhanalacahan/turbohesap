using FluentValidation;

namespace Turbohesap.Shared.Contracts.Customers;

/// <summary>Müşteri oluşturma doğrulaması (Shared — iki tarafta da geçerli).</summary>
public sealed class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
{
    public CreateCustomerCommandValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Müşteri kodu zorunludur.")
            .MaximumLength(32).WithMessage("Müşteri kodu en fazla 32 karakter olabilir.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Müşteri adı zorunludur.")
            .MaximumLength(200).WithMessage("Müşteri adı en fazla 200 karakter olabilir.");

        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("Geçerli bir e-posta adresi giriniz.")
            .MaximumLength(256)
            .When(x => !string.IsNullOrWhiteSpace(x.Email));

        RuleFor(x => x.PhoneNumber)
            .MaximumLength(32).WithMessage("Telefon en fazla 32 karakter olabilir.")
            .When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber));

        RuleFor(x => x.TaxNumber)
            .MaximumLength(32).WithMessage("Vergi numarası en fazla 32 karakter olabilir.")
            .When(x => !string.IsNullOrWhiteSpace(x.TaxNumber));
    }
}
