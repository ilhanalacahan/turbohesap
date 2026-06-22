using System.Linq.Expressions;
using Microsoft.AspNetCore.Components;

namespace Turbohesap.Web.Components.Input;

/// <summary>Form alanı sarmalayıcısı: etiket + içerik + FluentValidation mesajı (For ifadesinden).</summary>
public partial class ThField<TValue>
{
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public string? Label { get; set; }
    [Parameter] public string? Hint { get; set; }
    [Parameter] public bool Required { get; set; }

    /// <summary>Doğrulama mesajının bağlanacağı alan ifadesi, örn. <c>() => model.Email</c>.</summary>
    [Parameter] public Expression<Func<TValue>>? For { get; set; }
}
