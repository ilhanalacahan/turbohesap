using System;
using System.Globalization;
using Microsoft.AspNetCore.Components;
using Turbohesap.Web.Components.Base;

namespace Turbohesap.Web.Components.Badge;

/// <summary>
/// ERP evrak ve kayıt durumlarını (Taslak, Onay Bekliyor, Ödendi vb.) otomatik renklendiren ve standartlaştıran akıllı rozet.
/// </summary>
public partial class ThStatusBadge : TurboComponentBase
{
    [Parameter] public string Status { get; set; } = string.Empty;
    [Parameter] public string? Label { get; set; }
    [Parameter] public bool ShowDot { get; set; } = true;
    [Parameter] public ComponentSize Size { get; set; } = ComponentSize.Md;

    private string DisplayLabel => !string.IsNullOrEmpty(Label) ? Label : FormatStatus(Status);

    private string StatusVariantClass
    {
        get
        {
            if (string.IsNullOrEmpty(Status)) return "th-status-badge--neutral";

            var key = Status.ToUpperInvariant().Trim();

            // ERP durum eşlemeleri
            return key switch
            {
                "DRAFT" or "TASLAK" => "th-status-badge--neutral",
                
                "PENDING" or "ONAY_BEKLIYOR" or "BEKLEMEDE" or "WAITING" => "th-status-badge--warning",
                
                "COMPLETED" or "ONAYLANDI" or "ODENDI" or "TAMAMLANDI" or "SUCCESS" or "ACTIVE" or "AKTIF" => "th-status-badge--success",
                
                "CANCELLED" or "IPTAL" or "REDDEDILDI" or "DANGER" or "REJECTED" or "HATA" => "th-status-badge--danger",
                
                "PROCESSING" or "INVOICED" or "SEVK_EDILDI" or "DEVAM_EDIYOR" or "INFO" or "YUKLENDI" => "th-status-badge--info",
                
                "PARTIAL" or "KISMEN_ODENDI" or "KISMEN_SEVK_EDILDI" or "ACCENT" => "th-status-badge--accent",
                
                _ => "th-status-badge--neutral"
            };
        }
    }

    private string RootClass => Cx(
        "th-status-badge",
        StatusVariantClass,
        SizeClass(Size, "th-status-badge"),
        Class);

    private string FormatStatus(string status)
    {
        if (string.IsNullOrEmpty(status)) return string.Empty;
        
        // Yılan/kebab-case olanları boşlukla ayır ve ilk harfleri büyüt
        var formatted = status.Replace("_", " ").Replace("-", " ");
        return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(formatted.ToLower());
    }
}
