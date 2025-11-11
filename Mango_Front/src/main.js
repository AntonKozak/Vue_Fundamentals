import { createPinia } from 'pinia'
import { createApp } from 'vue'

import 'bootstrap-icons/font/bootstrap-icons.css'
import 'bootstrap/dist/css/bootstrap.min.css'
import 'bootstrap/dist/js/bootstrap.bundle.min.js'
import './assets/styles/theme.css'

import App from './App.vue'
import router from './router/routes'
import { useThemeStore } from './stores/theme'

const app = createApp(App)
const pinia = createPinia()

app.use(pinia)
app.use(router)

// Initialize theme
const themeStore = useThemeStore()
themeStore.initTheme()

app.mount('#app')
