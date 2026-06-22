// Tema yönetimi: açık/koyu/sistem modu + token override (tema özelleştirme).
// Tüm görsel parametreler --th-* CSS değişkenleridir; mod değişince tokenlar otomatik
// güncellenir (req 16, 29). Tercihler localStorage'da saklanır.

export type ThemeMode = 'light' | 'dark' | 'system';
export type Density = 'compact' | 'normal' | 'comfortable';
export type ShadowLevel = 'none' | 'soft' | 'medium' | 'strong';

const MODE_KEY = 'th-theme-mode';
const TOKENS_KEY = 'th-theme-tokens';
const DENSITY_KEY = 'th-theme-density';
const SHADOW_KEY = 'th-theme-shadow';
const media = () => window.matchMedia('(prefers-color-scheme: dark)');

// Gölge düzeyi → kart/tablo yükselti tokenı (--th-card-shadow). Temaya duyarlı gölgelere işaret eder.
const SHADOW_MAP: Record<ShadowLevel, string> = {
  none: 'none',
  soft: 'var(--th-shadow-xs)',
  medium: 'var(--th-shadow-sm)',
  strong: 'var(--th-shadow-md)',
};

export function getMode(): ThemeMode {
  const stored = localStorage.getItem(MODE_KEY);
  return stored === 'light' || stored === 'dark' || stored === 'system' ? stored : 'system';
}

export function resolveMode(mode: ThemeMode): 'light' | 'dark' {
  return mode === 'system' ? (media().matches ? 'dark' : 'light') : mode;
}

/** data-theme özniteliğini <html> üzerine yazar ve modu saklar. */
export function setMode(mode: ThemeMode): void {
  localStorage.setItem(MODE_KEY, mode);
  document.documentElement.setAttribute('data-theme', resolveMode(mode));
  document.documentElement.dataset.themeMode = mode;
}

/** Açık/koyu arasında geçiş yapar (system'i çözüp tersine çevirir). */
export function toggle(): 'light' | 'dark' {
  const next = resolveMode(getMode()) === 'dark' ? 'light' : 'dark';
  setMode(next);
  return next;
}

/** Özelleştirilmiş token değerlerini (örn. {'--th-primary':'#...'}) uygular ve saklar. */
export function applyTokens(tokens: Record<string, string>): void {
  const root = document.documentElement;
  for (const [key, value] of Object.entries(tokens)) {
    const name = key.startsWith('--') ? key : `--th-${key}`;
    root.style.setProperty(name, value);
  }
  localStorage.setItem(TOKENS_KEY, JSON.stringify(readStoredTokens(tokens)));
}

/** Yazı tipi ailesini değiştirir (--th-font-sans override). Değer tam CSS font-family zinciridir. */
export function setFont(family: string): void {
  applyTokens({ '--th-font-sans': family });
}

/** Yerleşim yoğunluğunu (compact/normal/comfortable) ayarlar; data-density özniteliğini yazar. */
export function setDensity(density: Density): void {
  localStorage.setItem(DENSITY_KEY, density);
  if (density === 'normal') {
    document.documentElement.removeAttribute('data-density');
  } else {
    document.documentElement.setAttribute('data-density', density);
  }
}

export function getDensity(): Density {
  const stored = localStorage.getItem(DENSITY_KEY);
  return stored === 'compact' || stored === 'comfortable' ? stored : 'normal';
}

/** Kart/tablo gölge düzeyini ayarlar (--th-card-shadow override). */
export function setShadow(level: ShadowLevel): void {
  localStorage.setItem(SHADOW_KEY, level);
  if (level === 'medium') {
    document.documentElement.style.removeProperty('--th-card-shadow');
  } else {
    document.documentElement.style.setProperty('--th-card-shadow', SHADOW_MAP[level]);
  }
}

export function getShadow(): ShadowLevel {
  const stored = localStorage.getItem(SHADOW_KEY);
  return stored === 'none' || stored === 'soft' || stored === 'strong' ? stored : 'medium';
}

/** Saklı token override'larını döner (tema tasarımcısı açık durumu eşitlemek için kullanır). */
export function getTokens(): Record<string, string> {
  return readStoredTokens();
}

/** Özelleştirmeleri temizler, tema + yoğunluk varsayılanlarına döner. */
export function resetTokens(): void {
  const stored = readStoredTokens();
  for (const key of Object.keys(stored)) {
    document.documentElement.style.removeProperty(key);
  }
  localStorage.removeItem(TOKENS_KEY);
  localStorage.removeItem(DENSITY_KEY);
  localStorage.removeItem(SHADOW_KEY);
  document.documentElement.removeAttribute('data-density');
  document.documentElement.style.removeProperty('--th-card-shadow');
}

function readStoredTokens(merge: Record<string, string> = {}): Record<string, string> {
  let current: Record<string, string> = {};
  try {
    current = JSON.parse(localStorage.getItem(TOKENS_KEY) ?? '{}');
  } catch {
    current = {};
  }
  for (const [key, value] of Object.entries(merge)) {
    current[key.startsWith('--') ? key : `--th-${key}`] = value;
  }
  return current;
}

/** Sayfa yüklenirken saklı mod + token override'ları uygular ve sistem değişimini dinler. */
export function initTheme(): void {
  setMode(getMode());
  setDensity(getDensity());
  setShadow(getShadow());

  const stored = readStoredTokens();
  for (const [key, value] of Object.entries(stored)) {
    document.documentElement.style.setProperty(key, value);
  }

  media().addEventListener('change', () => {
    if (getMode() === 'system') {
      document.documentElement.setAttribute('data-theme', resolveMode('system'));
    }
  });
}
