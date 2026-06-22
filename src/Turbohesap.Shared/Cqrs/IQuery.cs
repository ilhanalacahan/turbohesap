namespace Turbohesap.Shared.Cqrs;

/// <summary>
/// Durum değiştirmeyen bir CQRS sorgusunu işaretler. Sorgu handler'ları daima
/// AsNoTracking ile çalışır (req 10).
/// </summary>
/// <typeparam name="TResponse">Sorgunun döndürdüğü sonuç tipi.</typeparam>
public interface IQuery<TResponse>;
