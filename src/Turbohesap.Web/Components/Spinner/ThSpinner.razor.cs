using Microsoft.AspNetCore.Components;
using Turbohesap.Web.Components.Base;

namespace Turbohesap.Web.Components.Spinner;

public enum SpinnerVariant { Primary, Success, Warning, Danger, Info, Neutral }

/// <summary>
/// ThSpinner: Dairesel yükleniyor animasyonu (Spinner) bileşeni.
/// </summary>
public partial class ThSpinner : TurboComponentBase
{
    [Parameter] public ComponentSize Size { get; set; } = ComponentSize.Md;
    [Parameter] public SpinnerVariant Variant { get; set; } = SpinnerVariant.Primary;

    private string RootClass => Cx(
        "th-spinner",
        SizeClass(Size, "th-spinner"),
        Variant switch
        {
            SpinnerVariant.Success => "th-spinner--success",
            SpinnerVariant.Warning => "th-spinner--warning",
            SpinnerVariant.Danger => "th-spinner--danger",
            SpinnerVariant.Info => "th-spinner--info",
            SpinnerVariant.Neutral => "th-spinner--neutral",
            _ => null
        },
        Class);
}
