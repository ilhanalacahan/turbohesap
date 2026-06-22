using FluentValidation;

namespace Turbohesap.Shared.Contracts.Auth;

/// <summary>
/// Giriş doğrulaması. Shared içinde tutulur; API (Wolverine middleware) ve Blazor
/// (EditForm) tarafında aynen çalışır (req 5, 14).
/// </summary>
public sealed class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("E-posta zorunludur.")
            .EmailAddress().WithMessage("Geçerli bir e-posta adresi giriniz.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Parola zorunludur.")
            .MinimumLength(6).WithMessage("Parola en az 6 karakter olmalıdır.");
    }
}
