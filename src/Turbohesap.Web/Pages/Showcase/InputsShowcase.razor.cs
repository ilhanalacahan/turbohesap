using Microsoft.AspNetCore.Components;
using Turbohesap.Web.Components.Base;

namespace Turbohesap.Web.Pages.Showcase;

/// <summary>
/// ThSwitch, ThCheckbox ve ThTextArea bileşenleri için test vitrini.
/// </summary>
public partial class InputsShowcase : ComponentBase
{
    private bool _switch1 = true;
    private bool _switch2;
    private bool _switch3 = true;
    private bool _switchDisabled;

    private bool _check1 = true;
    private bool _check2;
    private bool _check3 = true;
    private bool _checkDisabled;

    private string _text1 = "Örnek veri";
    private string _text2 = "";
    private string _textDisabled = "Bu alan pasiftir, düzenlenemez.";
}
