using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using Turbohesap.Web.Components.Base;

namespace Turbohesap.Web.Components.Breadcrumb;

/// <summary>
/// ThBreadcrumb: Sayfa hiyerarşisini gösteren ekmek kırıntısı patikası.
/// </summary>
public partial class ThBreadcrumb : TurboComponentBase
{
    [Parameter] public List<BreadcrumbItem> Items { get; set; } = [];

    private string RootClass => Cx("th-breadcrumb", Class);
}
