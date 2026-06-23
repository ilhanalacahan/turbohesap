namespace Turbohesap.Web.Services;

/// <summary>
/// Kabuk (shell) arayüz durumunu koordine eder: sidebar açık/daraltılmış, komut başlatıcı,
/// uygulama başlatıcı ve AI paneli. Bileşenler <see cref="OnChange"/> ile yeniden çizilir.
/// </summary>
public sealed class LayoutState
{
    public bool SidebarOpen { get; private set; }       // mobil off-canvas
    public bool SidebarCollapsed { get; private set; }  // masaüstü daraltma
    public bool CommandOpen { get; private set; }
    public bool AppLauncherOpen { get; private set; }
    public bool AiOpen { get; private set; }
    public bool ThemeDesignerOpen { get; private set; }

    public event Action? OnChange;

    public void ToggleSidebar() { SidebarOpen = !SidebarOpen; Notify(); }
    public void CloseSidebar() { if (SidebarOpen) { SidebarOpen = false; Notify(); } }
    public void ToggleCollapse() { SidebarCollapsed = !SidebarCollapsed; Notify(); }

    public void ToggleCommand() { CommandOpen = !CommandOpen; Notify(); }
    public void CloseCommand() { if (CommandOpen) { CommandOpen = false; Notify(); } }

    public void ToggleAppLauncher() { AppLauncherOpen = !AppLauncherOpen; Notify(); }
    public void CloseAppLauncher() { if (AppLauncherOpen) { AppLauncherOpen = false; Notify(); } }

    public void ToggleAi() { AiOpen = !AiOpen; Notify(); }
    public void CloseAi() { if (AiOpen) { AiOpen = false; Notify(); } }

    public void ToggleThemeDesigner() { ThemeDesignerOpen = !ThemeDesignerOpen; Notify(); }
    public void CloseThemeDesigner() { if (ThemeDesignerOpen) { ThemeDesignerOpen = false; Notify(); } }

    private void Notify() => OnChange?.Invoke();
}
