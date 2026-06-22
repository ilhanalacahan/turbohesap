// Blazor'ın IJSRuntime üzerinden çağırdığı tarayıcı yardımcıları. window.turbohesap
// altında toplanır. Blazor Interactive Server, etkileşimin çoğunu C# tarafında yönetir;
// burada yalnızca tarayıcıda yapılması gereken işler bulunur.

import * as theme from './theme';
import * as dialog from './dialog';
import * as tabs from './tabs';
import * as menu from './menu';

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
      setFont: theme.setFont,
      setDensity: theme.setDensity,
      getDensity: theme.getDensity,
      setShadow: theme.setShadow,
      getShadow: theme.getShadow,
      getTokens: theme.getTokens,
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
    lockScroll() {
      const scrollbarWidth = window.innerWidth - document.documentElement.clientWidth;
      if (scrollbarWidth > 0) {
        document.body.style.paddingRight = `${scrollbarWidth}px`;
      }
      document.body.classList.add('th-no-scroll');
    },
    unlockScroll() {
      const overlays = document.querySelectorAll('.th-drawer-overlay, .th-modal-overlay, .th-dialog-overlay');
      if (overlays.length <= 1) {
        document.body.classList.remove('th-no-scroll');
        document.body.style.paddingRight = '';
      }
    },
    initResizableDrawer(drawer: HTMLElement, handle: HTMLElement, position: string) {
      let isResizing = false;
      let startX = 0, startY = 0;
      let startWidth = 0, startHeight = 0;

      function onMouseDown(e: MouseEvent) {
        isResizing = true;
        startX = e.clientX;
        startY = e.clientY;
        startWidth = drawer.offsetWidth;
        startHeight = drawer.offsetHeight;

        drawer.style.transition = 'none';
        if (position === 'left' || position === 'right') {
          drawer.style.width = `${startWidth}px`;
          drawer.style.maxWidth = 'none';
        } else {
          drawer.style.height = `${startHeight}px`;
          drawer.style.maxHeight = 'none';
        }

        document.addEventListener('mousemove', onMouseMove);
        document.addEventListener('mouseup', onMouseUp);
        e.preventDefault();
      }

      function onMouseMove(e: MouseEvent) {
        if (!isResizing) return;
        const dx = e.clientX - startX;
        const dy = e.clientY - startY;

        if (position === 'right') {
          const w = Math.max(250, Math.min(window.innerWidth * 0.95, startWidth - dx));
          drawer.style.width = `${w}px`;
        } else if (position === 'left') {
          const w = Math.max(250, Math.min(window.innerWidth * 0.95, startWidth + dx));
          drawer.style.width = `${w}px`;
        } else if (position === 'top') {
          const h = Math.max(150, Math.min(window.innerHeight * 0.95, startHeight + dy));
          drawer.style.height = `${h}px`;
        } else if (position === 'bottom') {
          const h = Math.max(150, Math.min(window.innerHeight * 0.95, startHeight - dy));
          drawer.style.height = `${h}px`;
        }
      }

      function onMouseUp() {
        isResizing = false;
        drawer.style.transition = '';
        document.removeEventListener('mousemove', onMouseMove);
        document.removeEventListener('mouseup', onMouseUp);
      }

      function onTouchStart(e: TouchEvent) {
        const touch = e.touches[0];
        isResizing = true;
        startX = touch.clientX;
        startY = touch.clientY;
        startWidth = drawer.offsetWidth;
        startHeight = drawer.offsetHeight;

        drawer.style.transition = 'none';
        if (position === 'left' || position === 'right') {
          drawer.style.width = `${startWidth}px`;
          drawer.style.maxWidth = 'none';
        } else {
          drawer.style.height = `${startHeight}px`;
          drawer.style.maxHeight = 'none';
        }

        document.addEventListener('touchmove', onTouchMove, { passive: false });
        document.addEventListener('touchend', onTouchEnd);
      }

      function onTouchMove(e: TouchEvent) {
        if (!isResizing) return;
        const touch = e.touches[0];
        const dx = touch.clientX - startX;
        const dy = touch.clientY - startY;

        if (position === 'right') {
          const w = Math.max(250, Math.min(window.innerWidth * 0.95, startWidth - dx));
          drawer.style.width = `${w}px`;
        } else if (position === 'left') {
          const w = Math.max(250, Math.min(window.innerWidth * 0.95, startWidth + dx));
          drawer.style.width = `${w}px`;
        } else if (position === 'top') {
          const h = Math.max(150, Math.min(window.innerHeight * 0.95, startHeight + dy));
          drawer.style.height = `${h}px`;
        } else if (position === 'bottom') {
          const h = Math.max(150, Math.min(window.innerHeight * 0.95, startHeight - dy));
          drawer.style.height = `${h}px`;
        }
        e.preventDefault();
      }

      function onTouchEnd() {
        isResizing = false;
        drawer.style.transition = '';
        document.removeEventListener('touchmove', onTouchMove);
        document.removeEventListener('touchend', onTouchEnd);
      }

      handle.addEventListener('mousedown', onMouseDown);
      handle.addEventListener('touchstart', onTouchStart, { passive: true });

      return {
        dispose() {
          handle.removeEventListener('mousedown', onMouseDown);
          handle.removeEventListener('touchstart', onTouchStart);
          document.removeEventListener('mousemove', onMouseMove);
          document.removeEventListener('mouseup', onMouseUp);
          document.removeEventListener('touchmove', onTouchMove);
          document.removeEventListener('touchend', onTouchEnd);
        }
      };
    },
    clearDrawerStyle(drawer: HTMLElement | null) {
      if (drawer) {
        drawer.style.width = '';
        drawer.style.height = '';
        drawer.style.maxWidth = '';
        drawer.style.maxHeight = '';
      }
    },
    dialog: {
      show: dialog.show,
      updateProgress: dialog.updateProgress,
      updateCountdown: dialog.updateCountdown,
      close: dialog.close
    },
    tabs: {
      init: tabs.init
    },
    menu: {
      initDropdown: menu.initDropdown,
      initContextMenu: menu.initContextMenu,
      initSubmenu: menu.initSubmenu
    },
    initDraggable(dialog: HTMLElement, header: HTMLElement) {
      let isDragging = false;
      let startX = 0, startY = 0;
      let dialogX = 0, dialogY = 0;

      function onMouseDown(e: MouseEvent) {
        if (e.button !== 0) return;
        const target = e.target as HTMLElement;
        if (target.closest('button, a, input, select, textarea, [role="button"]')) return;

        isDragging = true;
        startX = e.clientX;
        startY = e.clientY;

        const computedStyle = window.getComputedStyle(dialog);
        const transform = computedStyle.transform;
        if (transform && transform !== 'none') {
          const matrix = new DOMMatrix(transform);
          dialogX = matrix.m41;
          dialogY = matrix.m42;
        } else {
          dialogX = 0;
          dialogY = 0;
        }

        document.addEventListener('mousemove', onMouseMove);
        document.addEventListener('mouseup', onMouseUp);
        header.style.cursor = 'grabbing';
        e.preventDefault();
      }

      function onMouseMove(e: MouseEvent) {
        if (!isDragging) return;
        const dx = e.clientX - startX;
        const dy = e.clientY - startY;
        dialog.style.transform = `translate(${dialogX + dx}px, ${dialogY + dy}px)`;
      }

      function onMouseUp() {
        isDragging = false;
        document.removeEventListener('mousemove', onMouseMove);
        document.removeEventListener('mouseup', onMouseUp);
        header.style.cursor = 'grab';
      }

      function onTouchStart(e: TouchEvent) {
        const touch = e.touches[0];
        const target = e.target as HTMLElement;
        if (target.closest('button, a, input, select, textarea, [role="button"]')) return;

        isDragging = true;
        startX = touch.clientX;
        startY = touch.clientY;

        const transform = window.getComputedStyle(dialog).transform;
        if (transform && transform !== 'none') {
          const matrix = new DOMMatrix(transform);
          dialogX = matrix.m41;
          dialogY = matrix.m42;
        } else {
          dialogX = 0;
          dialogY = 0;
        }

        document.addEventListener('touchmove', onTouchMove, { passive: false });
        document.addEventListener('touchend', onTouchEnd);
      }

      function onTouchMove(e: TouchEvent) {
        if (!isDragging) return;
        const touch = e.touches[0];
        const dx = touch.clientX - startX;
        const dy = touch.clientY - startY;
        dialog.style.transform = `translate(${dialogX + dx}px, ${dialogY + dy}px)`;
        e.preventDefault();
      }

      // touchend event handler (defined inside registerInterop/initDraggable context)
      function onTouchEnd() {
        isDragging = false;
        document.removeEventListener('touchmove', onTouchMove);
        document.removeEventListener('touchend', onTouchEnd);
      }

      header.style.cursor = 'grab';
      header.addEventListener('mousedown', onMouseDown);
      header.addEventListener('touchstart', onTouchStart, { passive: true });

      return {
        dispose() {
          header.removeEventListener('mousedown', onMouseDown);
          header.removeEventListener('touchstart', onTouchStart);
          document.removeEventListener('mousemove', onMouseMove);
          document.removeEventListener('mouseup', onMouseUp);
          document.removeEventListener('touchmove', onTouchMove);
          document.removeEventListener('touchend', onTouchEnd);
          header.style.cursor = '';
          dialog.style.transform = '';
        }
      };
    }
  };

  (window as unknown as { turbohesap: typeof api }).turbohesap = api;
}
