// Tema yönetimi: açık/koyu/sistem modu + token override (tema özelleştirme).
// Tüm görsel parametreler --th-* CSS değişkenleridir; mod değişince tokenlar otomatik
// güncellenir (req 16, 29). Tercihler localStorage'da saklanır.

export type ThemeMode = 'light' | 'dark' | 'system';

const MODE_KEY = 'th-theme-mode';
const TOKENS_KEY = 'th-theme-tokens';
const media = () => window.matchMedia('(prefers-color-scheme: dark)');

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

/** Özelleştirmeleri temizler, tema varsayılanlarına döner. */
export function resetTokens(): void {
  const stored = readStoredTokens();
  for (const key of Object.keys(stored)) {
    document.documentElement.style.removeProperty(key);
  }
  localStorage.removeItem(TOKENS_KEY);
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
