import { createApp } from 'vue'
import './style.css'
import App from './App.vue'
import VueAppInsights from "vue-application-insights";


const app = createApp(App);
app.use(VueAppInsights, {
    id: 'dc25eddd-f2aa-4dcc-adaf-52d51c56aeaf'
});
app.mount('#app')
