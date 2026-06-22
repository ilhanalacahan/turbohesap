// TypeScript-driven Dialog module for Turbohesap.
// Handled completely on the client side to avoid InteractiveServer latency.

interface DialogJSInstance {
  id: string;
  overlay: HTMLElement;
  dotNetHelper: any;
  options: any;
}

const activeDialogs = new Map<string, DialogJSInstance>();

export function show(id: string, options: any, dotNetHelper: any) {
  // Create overlay container
  const overlay = document.createElement('div');
  overlay.className = 'th-overlay th-overlay-js';
  overlay.id = `th-dialog-overlay-${id}`;

  // Close helper
  function closeDialog(status: 'confirm' | 'cancel' | 'close', value?: string) {
    overlay.classList.add('th-overlay--fade-out');
    const dialogEl = overlay.querySelector('.th-dialog');
    if (dialogEl) {
      dialogEl.classList.add('th-dialog--fade-out');
    }
    setTimeout(() => {
      overlay.remove();
      activeDialogs.delete(id);
      if (status === 'confirm') {
        dotNetHelper.invokeMethodAsync('OnConfirm', value || null);
      } else if (status === 'cancel') {
        dotNetHelper.invokeMethodAsync('OnCancel');
      } else {
        dotNetHelper.invokeMethodAsync('OnClose');
      }
    }, 200); // match transition duration
  }

  // Create Dialog Card
  const dialogCard = document.createElement('div');
  dialogCard.className = `th-dialog th-dialog--sm th-dialog--js th-dialog--type-${options.type} th-dialog--variant-${options.variant}`;
  
  // Create Header
  const header = document.createElement('div');
  header.className = 'th-panel-header';
  
  const titleContainer = document.createElement('div');
  titleContainer.className = 'th-dialog__header-title-area';
  
  const title = document.createElement('div');
  title.className = 'th-panel-title';
  title.innerText = options.title;
  titleContainer.appendChild(title);
  header.appendChild(titleContainer);

  // Close button inside header
  const closeable = ['alert', 'success', 'warning', 'error', 'prompt', 'confirm', 'countdown'].includes(options.type);
  if (closeable) {
    const closeBtn = document.createElement('span');
    closeBtn.className = 'th-dialog__close';
    closeBtn.innerHTML = '<i class="fa-solid fa-xmark"></i>';
    closeBtn.onclick = () => closeDialog('close');
    header.appendChild(closeBtn);
  }
  dialogCard.appendChild(header);

  // Create Body
  const body = document.createElement('div');
  body.className = 'th-dialog__body';

  // Determine icon class and color by variant & type
  let iconHtml = '';
  let iconColorClass = 'text-primary';
  let iconClass = '';

  if (options.variant === 'danger') {
    iconClass = 'fa-solid fa-circle-exclamation';
    iconColorClass = 'text-danger';
  } else if (options.variant === 'warning') {
    iconClass = 'fa-solid fa-triangle-exclamation';
    iconColorClass = 'text-warning';
  } else if (options.variant === 'success') {
    iconClass = 'fa-solid fa-circle-check';
    iconColorClass = 'text-success';
  } else if (options.variant === 'info') {
    iconClass = 'fa-solid fa-circle-info';
    iconColorClass = 'text-info';
  } else if (options.variant === 'primary') {
    iconClass = 'fa-solid fa-circle-info';
    iconColorClass = 'text-primary';
  }

  // Refine/override icon based on dialog type
  if (options.type === 'success') {
    iconClass = 'fa-solid fa-circle-check';
    iconColorClass = 'text-success';
  } else if (options.type === 'warning') {
    iconClass = 'fa-solid fa-triangle-exclamation';
    iconColorClass = 'text-warning';
  } else if (options.type === 'error') {
    iconClass = 'fa-solid fa-circle-xmark';
    iconColorClass = 'text-danger';
  } else if (options.type === 'loading' || options.type === 'busy') {
    iconClass = 'fa-solid fa-spinner fa-spin';
    iconColorClass = 'text-primary';
  } else if (options.type === 'confirm') {
    if (!iconClass) {
      iconClass = 'fa-solid fa-circle-question';
      iconColorClass = 'text-primary';
    }
  } else if (options.type === 'prompt') {
    if (!iconClass) {
      iconClass = 'fa-solid fa-pen-to-square';
      iconColorClass = 'text-primary';
    }
  }

  if (iconClass) {
    iconHtml = `<i class="${iconClass} ${iconColorClass} text-3xl"></i>`;
  }

  // Create Flex Container for side-by-side icon/content structure
  const flexContainer = document.createElement('div');
  flexContainer.className = 'flex gap-4 items-start text-start w-full';

  if (iconHtml) {
    const leftCol = document.createElement('div');
    leftCol.className = 'flex-shrink-0 pt-0.5';
    leftCol.innerHTML = iconHtml;
    flexContainer.appendChild(leftCol);
  }

  const rightCol = document.createElement('div');
  rightCol.className = 'flex-1 min-w-0';

  const msg = document.createElement('p');
  msg.className = 'th-dialog__message-text';
  msg.innerText = options.message;
  rightCol.appendChild(msg);

  // Add specific inputs/widgets based on type (nested under rightCol for aesthetic alignment)
  if (options.type === 'prompt') {
    const promptInput = document.createElement('input');
    promptInput.type = 'text';
    promptInput.className = 'th-input w-full mt-3';
    promptInput.placeholder = options.placeholder || '';
    rightCol.appendChild(promptInput);
    
    // Add enter key support to submit
    promptInput.onkeydown = (e) => {
      if (e.key === 'Enter') {
        closeDialog('confirm', promptInput.value);
      }
    };
  } else if (options.type === 'progress') {
    const progressContainer = document.createElement('div');
    progressContainer.className = 'th-dialog-progress-container w-full mt-3 bg-surface-3 rounded-full h-2.5 overflow-hidden border border-border';
    
    const progressBar = document.createElement('div');
    progressBar.className = 'th-dialog-progress-bar bg-primary h-full transition-all duration-200';
    progressBar.style.width = `${options.value || 0}%`;
    progressContainer.appendChild(progressBar);
    rightCol.appendChild(progressContainer);

    const progressText = document.createElement('div');
    progressText.className = 'th-dialog-progress-text text-xs text-text-muted mt-1.5';
    progressText.innerText = `%${Math.round(options.value || 0)}`;
    rightCol.appendChild(progressText);
  } else if (options.type === 'countdown') {
    const countdownEl = document.createElement('div');
    countdownEl.className = 'th-dialog-countdown text-2xl font-bold text-primary mt-3';
    countdownEl.innerText = `${options.duration || 0}`;
    rightCol.appendChild(countdownEl);
  }

  flexContainer.appendChild(rightCol);
  body.appendChild(flexContainer);
  dialogCard.appendChild(body);

  // Create Footer
  const footer = document.createElement('div');
  footer.className = 'th-dialog__footer';

  // Add buttons based on type
  if (options.type === 'confirm' || options.type === 'prompt') {
    const cancelBtn = document.createElement('button');
    cancelBtn.className = 'th-btn th-btn--secondary';
    cancelBtn.innerText = options.cancelText;
    cancelBtn.onclick = () => closeDialog('cancel');
    footer.appendChild(cancelBtn);

    const confirmBtn = document.createElement('button');
    confirmBtn.className = `th-btn th-btn--primary ${options.variant === 'danger' ? 'th-btn--danger' : ''}`;
    confirmBtn.innerText = options.confirmText;
    confirmBtn.onclick = () => {
      let val = undefined;
      if (options.type === 'prompt') {
        const inp = body.querySelector('input');
        val = inp ? inp.value : '';
      }
      closeDialog('confirm', val);
    };
    footer.appendChild(confirmBtn);
    dialogCard.appendChild(footer);
  } else if (options.type === 'alert' || options.type === 'success' || options.type === 'warning' || options.type === 'error') {
    const okBtn = document.createElement('button');
    okBtn.className = 'th-btn th-btn--primary';
    okBtn.innerText = options.confirmText;
    okBtn.onclick = () => closeDialog('confirm');
    footer.appendChild(okBtn);
    dialogCard.appendChild(footer);
  } else if (options.type === 'loading' || options.type === 'busy' || options.type === 'progress') {
    // Optional cancel action if cancel text is explicitly provided
    if (options.cancelText && options.cancelText !== 'İptal') {
      const cancelBtn = document.createElement('button');
      cancelBtn.className = 'th-btn th-btn--secondary';
      cancelBtn.innerText = options.cancelText;
      cancelBtn.onclick = () => closeDialog('cancel');
      footer.appendChild(cancelBtn);
      dialogCard.appendChild(footer);
    }
  }

  overlay.appendChild(dialogCard);
  document.body.appendChild(overlay);

  // Focus prompt input if exists
  if (options.type === 'prompt') {
    setTimeout(() => {
      body.querySelector('input')?.focus();
    }, 50);
  }

  // Save active instance
  activeDialogs.set(id, { id, overlay, dotNetHelper, options });

  return id;
}

export function updateProgress(id: string, percentage: number) {
  const instance = activeDialogs.get(id);
  if (!instance) return;
  
  const bar = instance.overlay.querySelector('.th-dialog-progress-bar') as HTMLElement;
  const txt = instance.overlay.querySelector('.th-dialog-progress-text') as HTMLElement;
  if (bar) bar.style.width = `${percentage}%`;
  if (txt) txt.innerText = `%${Math.round(percentage)}`;
}

export function updateCountdown(id: string, secondsRemaining: number) {
  const instance = activeDialogs.get(id);
  if (!instance) return;

  const countEl = instance.overlay.querySelector('.th-dialog-countdown') as HTMLElement;
  if (countEl) countEl.innerText = `${secondsRemaining}`;
}

export function close(id: string) {
  const instance = activeDialogs.get(id);
  if (!instance) return;

  const overlay = instance.overlay;
  overlay.classList.add('th-overlay--fade-out');
  const dialogEl = overlay.querySelector('.th-dialog');
  if (dialogEl) {
    dialogEl.classList.add('th-dialog--fade-out');
  }
  setTimeout(() => {
    overlay.remove();
    activeDialogs.delete(id);
    instance.dotNetHelper.invokeMethodAsync('OnClose');
  }, 200);
}
