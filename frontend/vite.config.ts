import { defineConfig, loadEnv } from 'vite'
import vue from '@vitejs/plugin-vue'
import mkcert from 'vite-plugin-mkcert'
import dns from 'dns'

dns.setDefaultResultOrder('verbatim')

// https://vitejs.dev/config/
export default defineConfig(({ command, mode }) => {
  // Load env file based on `mode` in the current working directory.
  // Set the third parameter to '' to load all env regardless of the `VITE_` prefix.
  const env = loadEnv(mode, process.cwd(), '')
  return {
    // vite config
    server:{
      https: true,
      hmr: {
        host: 'localhost'
      },
    },
    plugins: [vue(), mkcert()],
    define: {
      __API_BASE_PATH__: env.APP_API_BASE_PATH
    }
  }
})
