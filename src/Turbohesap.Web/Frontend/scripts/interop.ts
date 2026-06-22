// Blazor'ın IJSRuntime üzerinden çağırdığı tarayıcı yardımcıları. window.turbohesap
// altında toplanır. Blazor Interactive Server, etkileşimin çoğunu C# tarafında yönetir;
// burada yalnızca tarayıcıda yapılması gereken işler bulunur.

import * as theme from './theme';

function trapFocus(container: HTMLElement): () => void {
  const selector =
    'a[href], button:not([disabled]), textarea, input, select, [tabindex]:not([tabindex="-1"])';
  function onKey(event: KeyboardEvent) {
    if (event.key !== 'Tab') return;
    const focusable = Array.from(container.querySelectorAll<HTMLElement>(selector)).filter(
      (el) => el.offsetParent !== null,
    );
    if (focusable.length === 0) return;
    const first = focusable[0];
    const last = focusable[focusable.length - 1];
    if (event.shiftKey && document.activeElement === first) {
      event.preventDefault();
      last.focus();
    } else if (!event.shiftKey && document.activeElement === last) {
      event.preventDefault();
      first.focus();
    }
  }
  container.addEventListener('keydown', onKey);
  return () => container.removeEventListener('keydown', onKey);
}

export function registerInterop(): void {
  const api = {
    theme: {
      setMode: theme.setMode,
      toggle: theme.toggle,
      getMode: theme.getMode,
      current: () => document.documentElement.getAttribute('data-theme') ?? 'light',
      applyTokens: theme.applyTokens,
      resetTokens: theme.resetTokens,
    },
    focusTrap(element: HTMLElement) {
      const release = trapFocus(element);
      element.querySelector<HTMLElement>('[autofocus], input, button')?.focus();
      return { dispose: release };
    },
    focus(element: HTMLElement | null) {
      element?.focus();
    },
    scrollToTop(element: HTMLElement | null) {
      (element ?? document.scrollingElement)?.scrollTo({ top: 0, behavior: 'smooth' });
    },
  };

  (window as unknown as { turbohesap: typeof api }).turbohesap = api;
}
