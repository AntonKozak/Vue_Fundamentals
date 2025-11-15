import { APP_ROUTES_NAMES } from '@/constant/routeNames';
import { useAuthStore } from '@/stores/authStore';


export const requireAuth = (to, from, next) => {
    const authStore = useAuthStore();
    if (authStore.isAuthenticated) {
        next();
    } else {
        next({ name: APP_ROUTES_NAMES.SIGN_IN });
    }
};

export const requireAdmin = (to, from, next) => {
    const authStore = useAuthStore();
    if (authStore.isAuthenticated && authStore.isAdmin) {
        next();
    } else {
        next({ name: APP_ROUTES_NAMES.ACCESS_DENIED });
    }
};

export const preventAuthAccess = (to, from, next) => {
    const authStore = useAuthStore();
    if (!authStore.isAuthenticated) {
        next();
    } else {
        next({ name: APP_ROUTES_NAMES.HOME });
    }
};
