import { defineConfig } from 'vite';
import tailwindcss from '@tailwindcss/vite';
import { resolve } from 'node:path';

/**
 * Tüm frontend kaynakları tek giriş noktasından (main.ts) derlenip
 * ../wwwroot/turbohesap.js ve ../wwwroot/turbohesap.css olarak üretilir (req 21).
 * Font Awesome font dosyaları ayrı olarak ../wwwroot/fonts altına yazılır (req 22 + ikon talebi).
 * emptyOutDir=false: Blazor'ın ürettiği diğer wwwroot varlıkları silinmez.
 */
export default defineConfig({
  plugins: [tailwindcss()],
  build: {
    outDir: resolve(__dirname, '../wwwroot'),
    emptyOutDir: false,
    cssCodeSplit: false,
    assetsInlineLimit: 0,
    rollupOptions: {
      input: resolve(__dirname, 'main.ts'),
      output: {
        entryFileNames: 'turbohesap.js',
        chunkFileNames: 'turbohesap-[name].js',
        assetFileNames: (assetInfo) => {
          const name = assetInfo.names?.[0] ?? '';
          if (name.endsWith('.css')) return 'turbohesap.css';
          if (/\.(woff2?|ttf|eot|otf)$/i.test(name)) return 'fonts/[name][extname]';
          return 'assets/[name][extname]';
        },
      },
    },
  },
});
