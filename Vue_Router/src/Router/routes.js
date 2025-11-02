import { createRouter, createWebHistory } from 'vue-router';
import Login from '../components/Authentication/Login.vue';
import Contact from '../components/Home/Contact.vue';
import HomePage from '../components/Home/HomePage.vue';
import NotFound from '../components/Layout/NotFound.vue';
import ProductDetails from '../components/Product/ProductDetail.vue';
import ProductList from '../components/Product/ProductList.vue';

// Simple helpers you can later replace with real logic/services
function isUserAuthenticated() {
    // Replace with a real authentication check (e.g., from a store)
    return true;
}

function userHasProductListAccess() {
    // Replace with a real permission check
    return false;
}

// Navigation guards (return-style for Vue Router 4)
function requireAuthForProductDetails(to) {
    if (to.name === 'productDetails' && !isUserAuthenticated()) {
        console.log('Access to product details denied. Redirecting to login.');
        return { name: 'login' };
    }
}

function requireAccessForProductList() {
    if (!userHasProductListAccess()) {
        console.log('Access denied to product list. Redirecting to home.');
        return { name: 'home' };
    }
}


const router = createRouter({
    history: createWebHistory(import.meta.env.BASE_URL),
    routes: [
        {
            path: '/',
            component: HomePage,
            name: 'home'
        },
        {
            path: '/login',
            component: Login,
            name: 'login'
        },
        {
            path: '/contact',
            component: Contact,
            name: 'contact'
        },
        {
            path: '/productList', component: ProductList, name: 'productList',
            beforeEnter: requireAccessForProductList
        },
        {
            path: '/product/:productId/:categoryId?',
            component: ProductDetails,
            name: 'productDetails',
            props: true
        },
        {
            path: '/product',
            component: ProductDetails
        },
        {
            path: '/:pathMatch(.*)*',
            component: NotFound
        }
    ],
    linkActiveClass: 'active-link btn btn-primary',

});

router.beforeEach((to, from) => {
    return requireAuthForProductDetails(to, from);
});
export default router;
