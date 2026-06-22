namespace Turbohesap.Shared.Cqrs;

/// <summary>
/// Durum değiştiren bir CQRS komutunu işaretler. Wolverine handler'ları sözleşme
/// gereği bu mesajları işler; işaret arayüzü niyeti belgeler ve genel kısıtlar için kullanılır.
/// </summary>
/// <typeparam name="TResponse">Komutun döndürdüğü sonuç tipi.</typeparam>
public interface ICommand<TResponse>;

/// <summary>Sonuç döndürmeyen komutlar için.</summary>
public interface ICommand;
