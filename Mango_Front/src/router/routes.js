import { APP_ROUTES_NAMES } from '@/constant/routeNames'
import NoAccess from '@/views/auth/NoAccess.vue'
import SignIn from '@/views/auth/SignIn.vue'
import SignUp from '@/views/auth/SignUp.vue'
import ShopingCart from '@/views/cart/ShopingCart.vue'
import Home from '@/views/home/Home.vue'
import MenuItemList from '@/views/menu-items/MenuItemList.vue'
import MenuItemUpsert from '@/views/menu-items/MenuItemUpsert.vue'
import OrderConfirmation from '@/views/order/OrderConfirmation.vue'
import OrderHistoryList from '@/views/order/OrderHistoryList.vue'
import OrderManagment from '@/views/order/OrderManagment.vue'
import { createRouter, createWebHistory } from 'vue-router'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: APP_ROUTES_NAMES.HOME,
      component: Home
    },
    {
      path: '/no-access',
      name: APP_ROUTES_NAMES.ACCESS_DENIED,
      component: NoAccess
    },
    {
      path: '/sign-in',
      name: APP_ROUTES_NAMES.SIGN_IN,
      component: SignIn
    },
    {
      path: '/sign-up',
      name: APP_ROUTES_NAMES.SIGN_UP,
      component: SignUp
    },
    {
      path: '/cart',
      name: APP_ROUTES_NAMES.CART,
      component: ShopingCart
    },
    {
      path: '/menu-items',
      name: APP_ROUTES_NAMES.MENU_ITEM_LIST,
      component: MenuItemList
    },
    {
      path: '/menu-items/create',
      name: APP_ROUTES_NAMES.CREATE_MENU_ITEM,
      component: MenuItemUpsert
    },
    {
      path: '/menu-items/:id/edit',
      name: APP_ROUTES_NAMES.EDIT_MENU_ITEM,
      component: MenuItemUpsert,
      props: true
    },
    {
      path: '/order-confirmation/:orderId',
      name: APP_ROUTES_NAMES.ORDER_CONFIRM,
      component: OrderConfirmation,
      props: true
    },
    {
      path: '/orders',
      name: APP_ROUTES_NAMES.ORDER_LIST,
      component: OrderHistoryList
    },
    {
      path: '/orders/manage',
      name: APP_ROUTES_NAMES.MANAGE_ORDER_ADMIN,
      component: OrderManagment
    },
    {
      path: '/:catchAll(.*)',
      name: APP_ROUTES_NAMES.NOT_FOUND,
      component: Home
    }
  ]
})

export default router
