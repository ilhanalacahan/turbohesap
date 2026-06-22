# ThAvatar

> Kullanıcı resmini, resim bulunamadığında ad/soyad harflerini (initials) veya varsayılan bir ikon göstergesini ve çevrimiçi durum bilgisini barındıran avatar bileşeni.

- **Sınıf**: `th-avatar-wrap`, `th-avatar` (+ `--sm|lg`, `--square`), `th-avatar-status` (+ `--offline|away|busy`)
- **Namespace**: `Turbohesap.Web.Components.Avatar`
- **Dosyalar**: `ThAvatar.razor`, `ThAvatar.razor.cs`, `Frontend/css/avatar.css`

## Parametreler
| Parametre | Tip | Varsayılan | Açıklama |
|-----------|-----|------------|----------|
| `ImageUrl` | `string?` | — | Kullanıcı profil görseli bağlantısı |
| `Initials` | `string?` | — | Resim yüklenemezse gösterilecek ad/soyad baş harfleri (Örn. "AB") |
| `AltText` | `string` | `"Kullanıcı Avatarı"` | Resim alternatif metni (erişilebilirlik) |
| `Size` | `ComponentSize` | `Md` | Avatar boyutu (`Sm`, `Md`, `Lg`) |
| `Shape` | `AvatarShape` | `Circle` | Şekil varyantı (`Circle`, `Square`) |
| `ShowStatus` | `bool` | `false` | Çevrimiçi durum noktası gösterilsin mi |
| `Status` | `AvatarStatus` | `Online` | Çevrimiçi durum tipi (`Online`, `Offline`, `Away`, `Busy`) |
| `Class`/`Style`/`AdditionalAttributes` | — | — | Köke aktarılan CSS ve stil nitelikleri |

## Kullanım
```razor
<ThAvatar ImageUrl="https://picsum.photos/100" AltText="Ahmet Yılmaz" ShowStatus="true" Status="AvatarStatus.Online" />
<ThAvatar Initials="AY" Shape="AvatarShape.Square" Size="ComponentSize.Lg" />
```

## Kullanılan token'lar
- `--th-surface`
- `--th-surface-3`
- `--th-text-muted`
- `--th-text-subtle`
- `--th-border`
- `--th-radius-full`
- `--th-radius-md`
- `--th-success`
- `--th-warning`
- `--th-danger`
- `--th-font-sans`

## Tema/uyum notları
- **Hata Toleransı**: Profil resmi yüklenemediğinde (`onerror`), otomatik olarak `Initials` parametresine düşer. O da yoksa varsayılan kullanıcı ikonunu gösterir.
- **Koyu Tema**: Çerçeve ve durum kenarlığı aktif temaya uyum sağlar.
