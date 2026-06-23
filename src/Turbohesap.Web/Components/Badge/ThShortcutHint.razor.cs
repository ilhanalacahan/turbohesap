using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components;
using Turbohesap.Web.Components.Base;

namespace Turbohesap.Web.Components.Badge;

/// <summary>
/// Klavye kısayollarını (Ctrl+S, ESC, vb.) görsel olarak ayrıştırılmış kbd kutularıyla şık bir şekilde sunan ipucu göstergesi.
/// </summary>
public partial class ThShortcutHint : TurboComponentBase
{
    [Parameter] public string Keys { get; set; } = string.Empty;
    [Parameter] public ComponentSize Size { get; set; } = ComponentSize.Sm;

    private List<string> KeySegments => !string.IsNullOrEmpty(Keys)
        ? Keys.Split(new[] { '+', '-' }, StringSplitOptions.RemoveEmptyEntries)
              .Select(k => k.Trim())
              .ToList()
        : new List<string>();

    private string RootClass => Cx(
        "th-shortcut-hint",
        SizeClass(Size, "th-shortcut-hint"),
        Class);
}
