import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'
import mkcert from 'vite-plugin-mkcert'
import dns from 'dns'

dns.setDefaultResultOrder('verbatim')

// https://vitejs.dev/config/
export default defineConfig({
  server:{
    https: true,
    hmr: {
      host: 'localhost'
    },
  },
  plugins: [vue(), mkcert()]
})
