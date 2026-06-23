using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using Turbohesap.Web.Models;
using Turbohesap.Web.Services;

namespace Turbohesap.Web.Pages.Showcase.Dialog;

/// <summary>
/// Showcase sayfasındaki Dialog testlerinin izole edilmiş C# katmanı.
/// </summary>
public partial class DialogShowcase : ComponentBase
{
    [Inject] private ThDialogService DialogService { get; set; } = default!;
    [Inject] private ToastService Toasts { get; set; } = default!;

    private async Task ShowNormalAlert()
    {
        await DialogService.AlertAsync("Bilgi", "İşleminiz sıraya alındı.");
    }

    private async Task ShowPrimaryAlert()
    {
        await DialogService.AlertAsync("Hatırlatma", "Lütfen bilgilerinizi kontrol edin.", DialogVariant.Primary);
    }

    private async Task ShowSuccessAlert()
    {
        await DialogService.SuccessAsync("Başarılı", "Kayıt başarıyla oluşturuldu.");
    }

    private async Task ShowWarningAlert()
    {
        await DialogService.WarningAsync("Uyarı", "Bu işlem geri alınamaz.");
    }

    private async Task ShowDangerAlert()
    {
        await DialogService.ErrorAsync("Hata", "Sunucu ile bağlantı kurulamadı.");
    }

    private async Task ShowConfirm()
    {
        var result = await DialogService.ConfirmAsync(
            "Onay Gerekli", 
            "Devam etmek istediğinize emin misiniz?", 
            DialogVariant.Primary, 
            "Evet", 
            "Hayır"
        );

        if (result.Status == DialogStatus.Confirmed)
        {
            Toasts.Success("İşlem onaylandı.");
        }
        else
        {
            Toasts.Info("İşlem iptal edildi.");
        }
    }

    private async Task ShowDangerConfirm()
    {
        var result = await DialogService.ConfirmAsync(
            "Müşteriyi Sil", 
            "Müşteri kaydını silmek istediğinize emin misiniz? Bu işlem geri alınamaz.", 
            DialogVariant.Danger, 
            "Evet, Sil", 
            "Vazgeç"
        );

        if (result.Status == DialogStatus.Confirmed)
        {
            Toasts.Error("Müşteri kaydı silindi.");
        }
        else
        {
            Toasts.Info("Silme işlemi iptal edildi.");
        }
    }

    private async Task ShowPrompt()
    {
        var result = await DialogService.PromptAsync(
            "Değer Girişi", 
            "Lütfen yeni kategori adını girin:", 
            "örn. Elektronik"
        );

        if (result.Status == DialogStatus.Confirmed)
        {
            Toasts.Success($"Girdiğiniz değer: {result.Value}");
        }
        else
        {
            Toasts.Info("Giriş iptal edildi.");
        }
    }

    private async Task ShowLoading()
    {
        var dialogRef = await DialogService.LoadingAsync("Yükleniyor", "Lütfen bekleyin, veriler indiriliyor...");
        
        // Simüle edilen işlem
        await Task.Delay(2500);
        
        await dialogRef.CloseAsync();
        Toasts.Success("Veriler başarıyla yüklendi.");
    }

    private async Task ShowProgress()
    {
        var dialogRef = await DialogService.ProgressAsync("Dosya Gönderiliyor", "Lütfen bekleyin...", 0);

        for (int i = 0; i <= 100; i += 10)
        {
            await Task.Delay(200);
            await dialogRef.UpdateProgressAsync(i);
        }

        await dialogRef.CloseAsync();
        Toasts.Success("Dosya başarıyla gönderildi.");
    }

    private async Task ShowCountdown()
    {
        var dialogRef = await DialogService.CountdownAsync("Geri Sayım", "İşlem başlatılıyor...", 5);

        for (int i = 5; i >= 0; i--)
        {
            await Task.Delay(1000);
            await dialogRef.UpdateCountdownAsync(i);
        }

        await dialogRef.CloseAsync();
        Toasts.Success("Geri sayım tamamlandı, işlem başladı!");
    }
}
