using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using Turbohesap.Web.Components.Feedback;

namespace Turbohesap.Web.Services;

/// <summary>
/// Dinamik modal yönetim servisi. Razor sayfalarını parametreli modal içerisine yerleştirip
/// C# Form gibi geri dönüş değeri (ModalResult) almayı sağlar.
/// </summary>
public sealed class ThModalService
{
    private readonly List<ThModalRef> _modals = [];

    public IReadOnlyList<ThModalRef> ActiveModals => _modals;

    public event Action? OnChange;

    public ThModalRef Show<TComponent>(string title, Dictionary<string, object>? parameters = null, ModalOptions? options = null)
        where TComponent : IComponent
    {
        var modalOptions = options ?? new ModalOptions();
        var modalRef = new ThModalRef
        {
            Title = title,
            Options = modalOptions
        };

        // Modal içeriğini CascadingValue sarmalıyla oluşturup parametreleri geçiriyoruz
        modalRef.Content = builder =>
        {
            builder.OpenComponent<CascadingValue<ThModalRef>>(0);
            builder.AddAttribute(1, "Value", modalRef);
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

        _modals.Add(modalRef);
        OnChange?.Invoke();

        return modalRef;
    }

    public void Close(ThModalRef modalRef, ModalResult result)
    {
        if (_modals.Remove(modalRef))
        {
            modalRef.Close(result);
            OnChange?.Invoke();
        }
    }
}
