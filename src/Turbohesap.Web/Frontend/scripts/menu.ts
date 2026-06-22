// Dropdown and Context Menu positioning and portal-like fixed alignment helpers.
// Prevents clipping under parent containers with overflow: hidden or lower z-index.

interface MenuInstance {
  trigger?: HTMLElement;
  menu: HTMLElement;
  dotNetRef: any;
  cleanup?: () => void;
}

const activeMenus = new Map<string, MenuInstance>();

export function initDropdown(id: string, trigger: HTMLElement, menu: HTMLElement, dotNetRef: any) {
  function positionMenu() {
    if (!trigger || !menu) return;

    menu.style.position = 'fixed';
    menu.style.zIndex = '1000'; // var(--th-z-dropdown) equivalent

    const triggerRect = trigger.getBoundingClientRect();
    const menuWidth = menu.offsetWidth;
    const menuHeight = menu.offsetHeight;
    const screenWidth = window.innerWidth;
    const screenHeight = window.innerHeight;

    // Default position: bottom-left of trigger
    let left = triggerRect.left;
    let top = triggerRect.bottom;

    // Handle horizontal overflow (right edge)
    if (left + menuWidth > screenWidth) {
      // Align with right edge of trigger
      left = triggerRect.right - menuWidth;
    }
    // Keep within screen left edge
    if (left < 0) left = 4;

    // Handle vertical overflow (bottom edge)
    if (top + menuHeight > screenHeight) {
      // Flip to top of trigger
      top = triggerRect.top - menuHeight;
    }
    // Keep within screen top edge
    if (top < 0) top = 4;

    menu.style.left = `${left}px`;
    menu.style.top = `${top}px`;
    menu.style.visibility = 'visible';
  }

  // Position initially
  setTimeout(positionMenu, 0);

  // Reposition on scroll or resize
  window.addEventListener('scroll', positionMenu, { passive: true });
  window.addEventListener('resize', positionMenu);

  // Click outside listener
  function onClickOutside(e: MouseEvent) {
    const target = e.target as HTMLElement;
    if (!trigger.contains(target) && !menu.contains(target)) {
      dotNetRef.invokeMethodAsync('CloseMenu');
    }
  }

  document.addEventListener('mousedown', onClickOutside);

  const cleanup = () => {
    window.removeEventListener('scroll', positionMenu);
    window.removeEventListener('resize', positionMenu);
    document.removeEventListener('mousedown', onClickOutside);
  };

  activeMenus.set(id, { trigger, menu, dotNetRef, cleanup });

  return {
    reposition() {
      positionMenu();
    },
    dispose() {
      cleanup();
      activeMenus.delete(id);
    }
  };
}

export function initContextMenu(id: string, x: number, y: number, menu: HTMLElement, dotNetRef: any) {
  function positionMenu() {
    if (!menu) return;

    menu.style.position = 'fixed';
    menu.style.zIndex = '1000';

    const menuWidth = menu.offsetWidth;
    const menuHeight = menu.offsetHeight;
    const screenWidth = window.innerWidth;
    const screenHeight = window.innerHeight;

    let left = x;
    let top = y;

    // Handle horizontal overflow
    if (left + menuWidth > screenWidth) {
      left = screenWidth - menuWidth - 4;
    }
    if (left < 0) left = 4;

    // Handle vertical overflow
    if (top + menuHeight > screenHeight) {
      top = screenHeight - menuHeight - 4;
    }
    if (top < 0) top = 4;

    menu.style.left = `${left}px`;
    menu.style.top = `${top}px`;
    menu.style.visibility = 'visible';
  }

  setTimeout(positionMenu, 0);

  window.addEventListener('scroll', positionMenu, { passive: true });
  window.addEventListener('resize', positionMenu);

  function onClickOutside(e: MouseEvent) {
    const target = e.target as HTMLElement;
    if (!menu.contains(target)) {
      dotNetRef.invokeMethodAsync('CloseMenu');
    }
  }

  document.addEventListener('mousedown', onClickOutside);

  const cleanup = () => {
    window.removeEventListener('scroll', positionMenu);
    window.removeEventListener('resize', positionMenu);
    document.removeEventListener('mousedown', onClickOutside);
  };

  activeMenus.set(id, { menu, dotNetRef, cleanup });

  return {
    reposition() {
      positionMenu();
    },
    dispose() {
      cleanup();
      activeMenus.delete(id);
    }
  };
}

export function initSubmenu(item: HTMLElement, submenu: HTMLElement) {
  function positionSubmenu() {
    if (!item || !submenu) return;

    submenu.style.position = 'fixed';
    submenu.style.zIndex = '1010'; // higher z-index to stay on top of parent menu

    const itemRect = item.getBoundingClientRect();
    const subWidth = submenu.offsetWidth;
    const subHeight = submenu.offsetHeight;
    const screenWidth = window.innerWidth;
    const screenHeight = window.innerHeight;

    // Default: side-by-side on the right, top-aligned
    let left = itemRect.right;
    let top = itemRect.top;

    // Overflow right edge -> flip to left side
    if (left + subWidth > screenWidth) {
      left = itemRect.left - subWidth;
    }
    if (left < 0) left = 4;

    // Overflow bottom edge -> adjust top
    if (top + subHeight > screenHeight) {
      top = screenHeight - subHeight - 4;
    }
    if (top < 0) top = 4;

    submenu.style.left = `${left}px`;
    submenu.style.top = `${top}px`;
    submenu.style.visibility = 'visible';
  }

  setTimeout(positionSubmenu, 0);

  window.addEventListener('scroll', positionSubmenu, { passive: true });
  window.addEventListener('resize', positionSubmenu);

  return {
    reposition() {
      positionSubmenu();
    },
    dispose() {
      window.removeEventListener('scroll', positionSubmenu);
      window.removeEventListener('resize', positionSubmenu);
    }
  };
}
