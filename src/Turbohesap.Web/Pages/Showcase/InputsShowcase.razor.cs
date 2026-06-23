using Microsoft.AspNetCore.Components;
using System.Globalization;
using Turbohesap.Web.Components.Base;
using Turbohesap.Web.Services;

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

    // --- Yeni Giriş Alanı Bindings ---
    private string _inputNormal = "";
    private string _inputEmail = "cihad@turbohesap.local";
    private string _inputPassword = "MySecurePassword123!";
    private string _passwordType = "password";
    private string _passwordIcon = "fa-solid fa-eye";
    private string _inputSearch = "";
    private string _inputMultiBtn = "";

    // --- Radyo Grubu Bindings ---
    private string _radioHorizontal = "A";
    private string _radioVertical = "2";

    // --- Tarih/Saat Seçici Bindings ---
    private DateTime? _dateValue = DateTime.Today;
    private TimeOnly? _timeValue = new TimeOnly(10, 30);
    private DateTime? _dateTimeValue = DateTime.Now;

    // --- Para Girişi & Maskeli Giriş Bindings ---
    private static readonly CultureInfo TrCulture = new CultureInfo("tr-TR");
    private decimal? _currencyValue1 = 1250.50M;
    private decimal? _currencyValue2 = null;
    private string _phoneValueRaw = "3465121017";
    private string _phoneValueFormatted = "";
    private decimal _standaloneCalcValue = 100M;
    private DateTime? _rangeStart = DateTime.Today.AddDays(-7);
    private DateTime? _rangeEnd = DateTime.Today;
    private string _ibanValueRaw2 = "26000100000000000000000000";
    private string _plateValue = "34ABC123";
    private string _fileName = "fatura_yazdir";
    private string _addressValue = "A-03-B-04";
    private string _segmentedValue = "ALL";
    private List<string> _tags = new() { "Acil", "Ön Muhasebe", "2026" };
    private List<string> _readonlyTags = new() { "Kilitli", "Sistem" };
    private Dictionary<string, string> _segmentedOptions = new()
    {
        { "ALL", "Tüm Kayıtlar" },
        { "IN", "Giriş Fişleri" },
        { "OUT", "Çıkış Fişleri" }
    };

    [Inject] private ToastService? ToastService { get; set; }

    private void OnAddressChanged(string val)
    {
        ToastService?.Info($"Yeni Depo Adresi: {val}");
    }

    private void SetAddress(string val)
    {
        _addressValue = val;
        ToastService?.Success($"Adres Ayarlandı: {val}");
    }

    private void OnSegmentedChanged(string val)
    {
        ToastService?.Info($"Filtre Seçildi: {val} ({_segmentedOptions[val]})");
    }

    private void TogglePassword()
    {
        if (_passwordType == "password")
        {
            _passwordType = "text";
            _passwordIcon = "fa-solid fa-eye-slash";
        }
        else
        {
            _passwordType = "password";
            _passwordIcon = "fa-solid fa-eye";
        }
    }

    private void HandleSearch()
    {
        ToastService?.Success($"Arama yapılıyor: {_inputSearch}");
    }

    private void HandleClearSearch()
    {
        _inputSearch = "";
        ToastService?.Info("Arama temizlendi");
    }

    private void HandleRefresh()
    {
        ToastService?.Info("İçerik yenileniyor...");
    }

    private void HandleSave()
    {
        ToastService?.Success("Kaydedildi!");
    }

    private void OnCurrencyChanged(decimal? val)
    {
        ToastService?.Info($"Tutar Değişti: {val?.ToString("N2", TrCulture) ?? "Boş"}");
    }

    private void OnPhoneRawChanged(string? val)
    {
        ToastService?.Info($"Telefon (Ham): {val}");
    }

    private void OnPhoneFormattedChanged(string? val)
    {
        ToastService?.Info($"Telefon (Formatlı): {val}");
    }

    private void OnStandaloneCalcSelected(decimal val)
    {
        _standaloneCalcValue = val;
        ToastService?.Success($"Hesap Makinesi Onaylandı: {val.ToString("N2", TrCulture)}");
    }

    private void OnRangeChanged()
    {
        if (_rangeStart.HasValue && _rangeEnd.HasValue)
        {
            ToastService?.Info($"Tarih Aralığı: {_rangeStart.Value.ToString("dd.MM.yyyy", TrCulture)} - {_rangeEnd.Value.ToString("dd.MM.yyyy", TrCulture)}");
        }
    }

    private void OnIbanRawChanged(string? val)
    {
        ToastService?.Info($"IBAN (Ham): {val}");
    }

    private void OnIban2RawChanged(string? val)
    {
        ToastService?.Info($"Yeni IBAN Ham: {val}");
    }

    private void HandleIbanPrefixClick()
    {
        ToastService?.Success("IBAN öneki (TR) tıklandı! Kopyalama veya doğrulama yapılabilir.");
    }

    private void HandleFileExtensionClick()
    {
        ToastService?.Success("Dosya soneki (.jpeg) tıklandı! Uzantı değiştirilebilir.");
    }
}
