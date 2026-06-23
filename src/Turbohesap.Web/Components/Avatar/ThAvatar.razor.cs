using Microsoft.AspNetCore.Components;
using Turbohesap.Web.Components.Base;

namespace Turbohesap.Web.Components.Avatar;

public enum AvatarStatus { Online, Offline, Away, Busy }
public enum AvatarShape { Circle, Square }

/// <summary>
/// Erişilebilir ve özelleştirilmiş avatar (th-avatar) bileşeni.
/// </summary>
public partial class ThAvatar : TurboComponentBase
{
    [Parameter] public string? ImageUrl { get; set; }
    [Parameter] public string? Initials { get; set; }
    [Parameter] public string AltText { get; set; } = "Kullanıcı Avatarı";
    [Parameter] public ComponentSize Size { get; set; } = ComponentSize.Md;
    [Parameter] public AvatarShape Shape { get; set; } = AvatarShape.Circle;
    [Parameter] public bool ShowStatus { get; set; }
    [Parameter] public AvatarStatus Status { get; set; } = AvatarStatus.Online;

    private bool _imageError;

    protected override void OnParametersSet()
    {
        _imageError = false;
    }

    private void HandleImageError()
    {
        _imageError = true;
        StateHasChanged();
    }

    private string RootClass => Cx(
        "th-avatar",
        SizeClass(Size, "th-avatar"),
        Shape == AvatarShape.Square ? "th-avatar--square" : null,
        Class);

    private string StatusClass => Cx(
        "th-avatar-status",
        Status switch
        {
            AvatarStatus.Offline => "th-avatar-status--offline",
            AvatarStatus.Away => "th-avatar-status--away",
            AvatarStatus.Busy => "th-avatar-status--busy",
            _ => null
        });
}
