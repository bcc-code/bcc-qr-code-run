/// <reference types="vite/client" />

declare const __API_BASE_PATH__: string

declare module '*.vue' {
  import type { DefineComponent } from 'vue'
  const component: DefineComponent<{}, {}, any>
  export default component
}

declare module 'vue3-qrcode-reader'