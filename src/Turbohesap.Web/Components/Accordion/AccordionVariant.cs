namespace Turbohesap.Web.Components.Accordion;

/// <summary>Akordeon görsel varyantı (yalnızca sabit sınıf adlarına eşlenir).</summary>
public enum AccordionVariant
{
    /// <summary>Tek çerçeve içinde, aralarında çizgili öğeler (varsayılan).</summary>
    Bordered,

    /// <summary>Her öğe ayrı bir kart; aralarında boşluk.</summary>
    Separated,

    /// <summary>Dış çerçeve/gölge yok; bir kartın içine gömmek için.</summary>
    Flush
}
