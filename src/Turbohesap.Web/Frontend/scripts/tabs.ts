// Reusable tab component horizontal scroll helpers.
// Handles mouse wheel scrolling and showing/hiding overflow navigation arrows.

export function init(
  viewport: HTMLElement,
  leftArrow: HTMLElement | null,
  rightArrow: HTMLElement | null
) {
  function updateArrows() {
    const scrollLeft = viewport.scrollLeft;
    const maxScroll = viewport.scrollWidth - viewport.clientWidth;

    // Show left arrow if scrolled away from start
    if (leftArrow) {
      leftArrow.style.display = scrollLeft > 1 ? 'flex' : 'none';
    }

    // Show right arrow if there is scrollable content remaining
    if (rightArrow) {
      rightArrow.style.display = maxScroll - scrollLeft > 1 ? 'flex' : 'none';
    }
  }

  // Handle mouse wheel horizontal scrolling
  function onWheel(e: WheelEvent) {
    if (e.deltaY !== 0) {
      viewport.scrollLeft += e.deltaY;
      e.preventDefault();
    }
  }

  // Scroll viewport on click of arrow buttons
  function onLeftClick() {
    viewport.scrollBy({ left: -200, behavior: 'smooth' });
  }

  function onRightClick() {
    viewport.scrollBy({ left: 200, behavior: 'smooth' });
  }

  // Set up listeners
  viewport.addEventListener('scroll', updateArrows);
  viewport.addEventListener('wheel', onWheel, { passive: false });
  window.addEventListener('resize', updateArrows);

  if (leftArrow) leftArrow.addEventListener('click', onLeftClick);
  if (rightArrow) rightArrow.addEventListener('click', onRightClick);

  // Initial check
  setTimeout(updateArrows, 100);

  // Use a MutationObserver to update arrows when tabs are dynamically added/removed
  const observer = new MutationObserver(updateArrows);
  observer.observe(viewport, { childList: true, subtree: true });

  return {
    update() {
      updateArrows();
    },
    dispose() {
      observer.disconnect();
      viewport.removeEventListener('scroll', updateArrows);
      viewport.removeEventListener('wheel', onWheel);
      window.removeEventListener('resize', updateArrows);
      if (leftArrow) leftArrow.removeEventListener('click', onLeftClick);
      if (rightArrow) rightArrow.removeEventListener('click', onRightClick);
    }
  };
}
