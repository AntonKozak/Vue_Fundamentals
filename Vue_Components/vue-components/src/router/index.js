import { createRouter, createWebHistory } from 'vue-router'
import ContactUs from '../components/ContactUs.vue'
import Home from '../components/Home.vue'

const routes = [
  {
    path: '/',
    name: 'Home',
    component: Home
  },
  {
    path: '/contact',
    name: 'ContactUs',
    component: ContactUs
  }
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

export default router
