// Turbohesap frontend giriş noktası. Tüm stiller ve scriptler buradan toplanır ve
// ../wwwroot/turbohesap.{css,js} olarak derlenir (req 21, 22).
import './main.css';

import { initTheme } from './scripts/theme';
import { registerInterop } from './scripts/interop';

// Tema (data-theme + token override) sayfa boyanmadan önce uygulanır.
initTheme();

// Blazor'ın çağıracağı window.turbohesap API'sini kaydet.
registerInterop();
