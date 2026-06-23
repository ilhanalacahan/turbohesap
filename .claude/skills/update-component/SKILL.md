---
name: update-component
description: Mevcut bir Turbohesap Blazor bileşenini tema/token sistemiyle tam uyumlu, hatasız biçimde günceller; ayrıca bileşenin {Component}.md dökümanını (yoksa) yazar/günceller ve Components.md kaydını tazeler. Kullanıcı "X bileşenini güncelle/değiştir", "update component", "X bileşeninin dökümanını yaz", "X'e şu özelliği ekle" dediğinde kullan.
allowed-tools: Read, Write, Edit, Bash, Grep, Glob
---

# update-component — Turbohesap bileşeni güncelleyici

Mevcut bir bileşeni **DESIGN.md/AGENTS.md** kurallarını bozmadan, tema ve sistemle tutarlı şekilde değiştirir. Aynı zamanda **dökümantasyon yazma** yoludur: var olan bileşenlerin `{Component}.md`'si bu beceriyle üretilir.

## 0. Önce oku (zorunlu)
1. Hedef bileşenin **tüm** dosyaları: `Th<Ad>.razor`, `Th<Ad>.razor.cs`, `Frontend/css/<ad>.css`, varsa `Th<Ad>.md`, varsa `Frontend/scripts/<ad>.ts` + `Services/<Ad>Interop.cs`.
2. `DESIGN.md` (token + Tailwind kuralları), `AGENTS.md` §1.
3. `Components/Base/TurboComponentBase.cs` (`Cx`, `SizeClass`, ortak parametreler).
4. Gerekirse bağımlılıkları gör: bileşeni kullanan sayfalar (`grep -rn "Th<Ad>" src/Turbohesap.Web`).

## 1. Değişmez kurallar (güncellerken de geçerli)
- **Code-behind ayrımı korunur**: işaretleme `.razor`'da, C# `.razor.cs`'te. Mantığı `.razor`'a taşıma.
- **Token-first**: yeni stiller yalnız `--th-*` token'larıyla; sabit renk/gölge/yarıçap ekleme. Tema duyarlılığını koru (renk→`--th-*`, köşe→`--th-radius-*`, gölge→`--th-card-shadow`/`--th-shadow-*`, ritim→`--th-nav-pad-y`/`--th-row-pad-y`/`--th-density-gap`, hareket→`--th-transition*`/`--th-ease`, katman→`--th-z-*`). Yazı tipini sabitleme.
- **Tailwind serbest, interpolasyon yasak**: utility'leri tam yaz; varyant/boyut C# `switch` → sabit `th-<ad>--*` sınıfı (`SizeClass`/`Cx`).
- **Geriye dönük uyumluluk**: mevcut parametre adlarını/davranışını gereksiz yere bozma. Parametre kaldırıyor/yeniden adlandırıyorsan kullanan tüm sayfaları da güncelle (grep ile bul) ve derlemeyi yeşil tut.
- **Yeni klasör/namespace** açtıysan `_Imports.razor`'a `@using` ekle.

## 2. Adımlar
1. **Kapsamı belirle**: kullanıcının istediği değişikliği (yeni varyant/boyut/parametre, davranış, stil, erişilebilirlik, performans) netleştir; belirsizse sor.
2. **İşaretleme** değişikliği → `Th<Ad>.razor`. **Mantık/parametre** değişikliği → `Th<Ad>.razor.cs`. **Görsel** değişiklik → `Frontend/css/<ad>.css` (yalnız token). Yeni CSS dosyası eklediysen `main.css`'e `@import ... layer(components);`.
3. **Performans gerekiyorsa** TypeScript interop ekle/güncelle: `Frontend/scripts/<ad>.ts` + `interop.ts` kaydı + `Services/<Ad>Interop.cs` + `Program.cs` DI + bileşende `IAsyncDisposable`. (Yalnızca yoğun DOM işi için; aksi halde C#'ta tut.)
4. **Kullananları güncelle**: imza değiştiyse `grep -rn "Th<Ad>"` ile tüm kullanımları düzelt.
5. **Döküman** (`Th<Ad>.md`): yoksa **oluştur**, varsa güncelle (şablon §3). Mevcut bileşenin dökümanını ilk kez yazıyorsan kodu okuyup parametre tablosunu, varyantları, kullanılan token'ları ve örnekleri **koddan birebir** çıkar.
6. **`Components.md`** kaydını ekle/güncelle (§4): satır yoksa ekle, özet/durum değiştiyse tazele.
7. **Doğrula**:
   ```bash
   dotnet build src/Turbohesap.Web/Turbohesap.Web.csproj -nologo -clp:NoSummary
   ( cd src/Turbohesap.Web/Frontend && npm run build )
   ```
   İkisi de başarılı olmadan bitirme.

## 3. `Th<Ad>.md` döküman şablonu (koddan doldur)
```markdown
# Th<Ad>

> Bir cümlelik amaç.

- **Sınıf**: `th-<ad>` (+ varyantlar, boyutlar)
- **Namespace**: `Turbohesap.Web.Components.<Ad>`
- **Dosyalar**: `Th<Ad>.razor`, `Th<Ad>.razor.cs`, `Frontend/css/<ad>.css`[, TS interop]

## Parametreler
| Parametre | Tip | Varsayılan | Açıklama |
|-----------|-----|------------|----------|
| … koddaki `[Parameter]`'ların tamamı … |

## Varyantlar / Boyutlar
Koddaki `switch` eşlemesinden: `--primary`, `--secondary`, … ; `--sm|md|lg`.

## Kullanım
\`\`\`razor
<Th<Ad> ... >…</Th<Ad>>
\`\`\`

## Kullanılan token'lar
CSS'te gerçekten geçen `--th-*` listesi.

## Tema/uyum notları
Hangi tema parametrelerine (renk/yarıçap/gölge/yoğunluk/font) tepki verir.
```

## 4. `Components.md` satırı
`| Th<Ad> | [<Ad>/Th<Ad>.md](<Ad>/Th<Ad>.md) | Özet | ✅ |`
Var olan satırı güncelle; yoksa tabloya ekle.

## 5. Bitiş kontrol listesi
- [ ] Code-behind ayrımı korundu; mantık `.razor`'a sızmadı
- [ ] Yalnız `--th-*` token; sabit renk/gölge/yarıçap yok; Tailwind interpolasyonu yok
- [ ] İmza değiştiyse tüm kullanım yerleri güncellendi
- [ ] `Th<Ad>.md` yazıldı/güncellendi; `Components.md` tazelendi
- [ ] `dotnet build` + `npm run build` başarılı
