namespace Turbohesap.Web.Components.Feedback;

/// <summary>Modal genişlik ölçüsü; sabit CSS sınıflarına eşlenir.</summary>
public enum ModalSize { Sm, Md, Lg, Xl }

/// <summary>Modal başlık varyantı; DESIGN.md'ye göre soft renk eşlemelerine izin verir.</summary>
public enum ModalVariant
{
    Default,
    Primary,
    Secondary,
    Success,
    Warning,
    Danger,
    Info
}

/// <summary>Service-driven modal pencere seçenekleri.</summary>
public class ModalOptions
{
    public ModalSize Size { get; set; } = ModalSize.Md;
    public bool Draggable { get; set; } = false;
    public bool Fullscreen { get; set; } = false;
    public bool Maximizable { get; set; } = false;
    public bool CloseOnOverlay { get; set; } = true;
    public ModalVariant Variant { get; set; } = ModalVariant.Default;
}
