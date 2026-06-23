namespace Turbohesap.Web.Components.Feedback;

/// <summary>Modal kapatıldığında geri dönecek olan sonuç nesnesi.</summary>
public class ModalResult
{
    public bool Cancelled { get; private set; }
    public object? Data { get; private set; }

    public static ModalResult Ok() => new() { Cancelled = false };
    public static ModalResult Ok<T>(T data) => new() { Cancelled = false, Data = data };
    public static ModalResult Cancel() => new() { Cancelled = true };
}
