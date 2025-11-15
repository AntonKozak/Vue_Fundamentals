
import { useToast } from '@/composables/useToast';
import { APP_ROUTES_NAMES } from '@/constant/routeNames';
import router from '@/router/routes';
import authService from '@/services/authService';
import Cookies from 'js-cookie';
import { defineStore } from 'pinia';
import { computed, reactive, ref } from 'vue';

export const useAuthStore = defineStore('authStore', () => {
    const user = reactive({
        id: null,
        name: '',
        email: '',
        role: ''
    });
    const token = ref(localStorage.getItem('token') || '');
    const isAuthenticated = ref(!!localStorage.getItem('token'));
    const isAdmin = computed(() => {
        return isAuthenticated && user.role === 'Admin';
    });
    const getUserInfor = computed(() => {
        return isAuthenticated.value ? { ...user } : null;
    });

    function decodeToken(token) {
        const playload = JSON.parse(atob(token.split('.')[1]));
        console.log('Decoded JWT payload during initialization:', playload);
        return {
            email: playload.email,
                name: playload.fullname,
                role: playload.role,
                id: playload.id
        };
    }

    function initialize() {
        try {
            const token = Cookies.get('token');
            if (token) {
                const userData = decodeToken(token);
                if (userData) {

                    Object.assign(user, userData);
                    isAuthenticated.value = true;
                } else {
                    clearAuthData();
                }
            } else {
                clearAuthData();
            }


        } catch (error) {
            console.error('Error during initialization:', error.message);

        }
    }

    async function signUp(userData) {
    try {
        // Call to authService to sign up the user
        await authService.signUp(userData);
        useToast().showSuccess('User registered successfully');

        return { success: true, message: 'User registered successfully' };
    } catch (error) {
        console.error('Error during sign up:', error);
        return { success: false, message: error.message };
    }

    }

    async function signIn(credentials) {
        try {
            const { token, user: userData } = await authService.signIn(credentials);

            Object.assign(user, userData);
            user.isLoggedIn = true
            isAuthenticated.value = true;
            Cookies.set('token', token, { expires: 7 });

            router.push({ name: APP_ROUTES_NAMES.HOME });
            useToast().showSuccess('Signed in successfully');
            return { success: true };

        } catch (error) {
            console.error('Error during sign in:', error);
            useToast().showError('Sign in failed');
            return { success: false, message: error.message };
        }
    }

    function logout() {
        clearAuthData();
        router.push({ name: APP_ROUTES_NAMES.SIGN_IN });

        useToast().showSuccess('Logged out successfully');
    }

    function clearAuthData() {
        Object.assign(user, {
            id: null,
            name: '',
            email: '',
            role: ''
        });
        isAuthenticated.value = false;
        Cookies.remove('token');
    }



    return {
        user,
        token,
        isAuthenticated,
        getUserInfor,
        isAdmin,
        signUp,
        signIn,
        logout,
        initialize,
        clearAuthData
    };
});
