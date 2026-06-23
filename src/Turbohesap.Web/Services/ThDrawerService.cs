using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using Turbohesap.Web.Components.Feedback;

namespace Turbohesap.Web.Services;

/// <summary>
/// Dinamik yan panel (drawer) yönetim servisi. Razor sayfalarını parametreli drawer içerisine yerleştirip
/// C# Form gibi geri dönüş değeri (DrawerResult) almayı sağlar.
/// </summary>
public sealed class ThDrawerService
{
    private readonly List<ThDrawerRef> _drawers = [];

    public IReadOnlyList<ThDrawerRef> ActiveDrawers => _drawers;

    public event Action? OnChange;

    public ThDrawerRef Show<TComponent>(string title, Dictionary<string, object>? parameters = null, DrawerOptions? options = null)
        where TComponent : IComponent
    {
        var drawerOptions = options ?? new DrawerOptions();
        var drawerRef = new ThDrawerRef
        {
            Title = title,
            Options = drawerOptions
        };

        // Drawer içeriğini CascadingValue sarmalıyla oluşturup parametreleri geçiriyoruz
        drawerRef.Content = builder =>
        {
            builder.OpenComponent<CascadingValue<ThDrawerRef>>(0);
            builder.AddAttribute(1, "Value", drawerRef);
            builder.AddAttribute(2, "ChildContent", (RenderFragment)(childBuilder =>
            {
                childBuilder.OpenComponent(3, typeof(TComponent));
                if (parameters != null)
                {
                    int seq = 4;
                    foreach (var parameter in parameters)
                    {
                        childBuilder.AddAttribute(seq++, parameter.Key, parameter.Value);
                    }
                }
                childBuilder.CloseComponent();
            }));
            builder.CloseComponent();
        };

        _drawers.Add(drawerRef);
        OnChange?.Invoke();

        return drawerRef;
    }

    public void Close(ThDrawerRef drawerRef, DrawerResult result)
    {
        if (_drawers.Remove(drawerRef))
        {
            drawerRef.Close(result);
            OnChange?.Invoke();
        }
    }
}
