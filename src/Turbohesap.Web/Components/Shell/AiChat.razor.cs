using Microsoft.AspNetCore.Components.Web;

namespace Turbohesap.Web.Components.Shell;

/// <summary>Sağ alt AI asistanı paneli (demo). Gerçek entegrasyon Claude API ile yapılabilir.</summary>
public partial class AiChat : IDisposable
{
    private readonly List<(string Text, bool IsUser)> _messages = [];
    private string _draft = string.Empty;

    protected override void OnInitialized() => Layout.OnChange += Changed;

    private void Send()
    {
        if (string.IsNullOrWhiteSpace(_draft))
        {
            return;
        }
        _messages.Add((_draft, true));
        _messages.Add(("Bu bir demo asistandır. Gerçek entegrasyon Claude API ile yapılabilir.", false));
        _draft = string.Empty;
    }

    private void OnKeyDown(KeyboardEventArgs e)
    {
        if (e.Key == "Enter")
        {
            Send();
        }
    }

    private void Changed() => InvokeAsync(StateHasChanged);

    public void Dispose() => Layout.OnChange -= Changed;
}
