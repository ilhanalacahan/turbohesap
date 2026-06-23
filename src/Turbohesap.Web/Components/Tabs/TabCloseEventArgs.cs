using System;

namespace Turbohesap.Web.Components.Tabs;

public class TabCloseEventArgs : EventArgs
{
    public bool Cancel { get; set; } = false;
    public ThTabPanel Tab { get; }

    public TabCloseEventArgs(ThTabPanel tab)
    {
        Tab = tab;
    }
}
