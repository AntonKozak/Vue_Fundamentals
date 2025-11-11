<template>
    <nav class="navbar navbar-expand-lg">
        <div class="container-fluid">
            <router-link :to="{ name: APP_ROUTES_NAMES.HOME }" class="navbar-brand">
                <img src="../../assets/logo.png" alt="Mango" height="48">
            </router-link>

            <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNav"
                aria-controls="navbarNav" aria-expanded="false" aria-label="Toggle navigation">
                <span class="navbar-toggler-icon"></span>
            </button>

            <div class="collapse navbar-collapse" id="navbarNav">
                <ul class="navbar-nav me-auto mb-2 mb-lg-0">
                    <li class="nav-item">
                        <router-link :to="{ name: APP_ROUTES_NAMES.HOME }" class="nav-link" active-class="active">
                            Home
                        </router-link>
                    </li>
                    <li class="nav-item">
                        <router-link :to="{ name: APP_ROUTES_NAMES.ORDER_LIST }" class="nav-link" active-class="active">
                            Orders
                        </router-link>
                    </li>

                    <!-- Products Dropdown -->
                    <li class="nav-item dropdown">
                        <a class="nav-link dropdown-toggle" href="#" role="button" data-bs-toggle="dropdown"
                            aria-expanded="false">
                            Admin
                        </a>
                        <ul class="dropdown-menu">
                            <li>
                                <router-link :to="{ name: APP_ROUTES_NAMES.MENU_ITEM_LIST }" class="dropdown-item">
                                    Menu Items
                                </router-link>
                            </li>
                            <li>
                                <router-link :to="{ name: APP_ROUTES_NAMES.MANAGE_ORDER_ADMIN }" class="dropdown-item">
                                    Order Managment
                                </router-link>
                            </li>
                        </ul>
                    </li>
                </ul>

                <!-- Search Form & Theme Toggle -->
                <div class="d-flex align-items-center gap-2">
                    <form class="d-flex" role="search" @submit.prevent="handleSearch">
                        <input v-model="searchQuery" class="form-control me-2" type="search" placeholder="Search"
                            aria-label="Search" />
                        <button class="btn btn-outline-primary" type="submit">Search</button>
                    </form>

                </div>

                <ul class="navbar-nav ms-auto align-items-center small">
                    <li class="nav-item mx-3"><router-link :to="{ name: APP_ROUTES_NAMES.CART }"
                            class="nav-link px-2 position-relative" active-class="active">
                            <i class="bi bi-cart3"></i><span
                                class="position-absolute start-100 translate-middle badge rounded-pill bg-danger ">0</span>
                        </router-link></li>
                    <li class="nav-item">
                        <router-link :to="{ name: APP_ROUTES_NAMES.SIGN_IN }" class="nav-link" active-class="active">
                            Sign In
                        </router-link>
                    </li>
                    <li class="nav-item">
                        <router-link :to="{ name: APP_ROUTES_NAMES.SIGN_UP }" class="nav-link" active-class="active">
                            Sign Up
                        </router-link>
                    </li>
                    <li class="nav-item">
                        <button class="nav-link px-2" active-class="active">
                            Logout
                        </button>
                    </li>
                    <!-- Theme Toggle -->
                    <ThemeToggle />
                </ul>
            </div>
        </div>
    </nav>
</template>

<script setup>
import ThemeToggle from '@/components/ThemeToggle.vue'
import { APP_ROUTES_NAMES } from '@/constant/routeNames'
import { ref } from 'vue'
import { useRouter } from 'vue-router'

const router = useRouter()
const searchQuery = ref('')

// Navigation Links using route names from constants
const navLinks = [
    { name: 'Home', routeName: APP_ROUTES_NAMES.HOME },
    { name: 'Menu Items', routeName: APP_ROUTES_NAMES.MENU_ITEM_LIST },
    { name: 'Orders', routeName: APP_ROUTES_NAMES.ORDER_LIST },
    { name: 'Cart', routeName: APP_ROUTES_NAMES.CART }
]

// Product Dropdown Links
const productLinks = [
    { name: 'All Menu Items', routeName: APP_ROUTES_NAMES.MENU_ITEM_LIST },
    { name: 'Create Menu Item', routeName: APP_ROUTES_NAMES.CREATE_MENU_ITEM },
    { name: 'Manage Orders', routeName: APP_ROUTES_NAMES.MANAGE_ORDER_ADMIN }
]

// Search Handler
const handleSearch = () => {
    if (searchQuery.value.trim()) {
        router.push({
            path: '/search',
            query: { q: searchQuery.value }
        })
        searchQuery.value = ''
    }
}
</script>

<style scoped>
.navbar {
    background-color: var(--bg-secondary);
    box-shadow: var(--shadow-md);
    border-bottom: 1px solid var(--border-color);
    transition: all var(--transition-speed);
}

.navbar-brand {
    font-weight: 700;
    font-size: 1.5rem;
    color: var(--color-primary) !important;
}

.nav-link {
    color: var(--text-primary) !important;
    transition: color var(--transition-speed);
}

.nav-link:hover {
    color: var(--color-primary) !important;
}

.nav-link.active {
    color: var(--color-primary) !important;
    font-weight: 600;
}

.dropdown-menu {
    background-color: var(--bg-secondary);
    border-color: var(--border-color);
}

.dropdown-item {
    color: var(--text-primary);
    transition: all var(--transition-speed);
}

.dropdown-item:hover {
    background-color: var(--bg-tertiary);
    color: var(--color-primary);
}

.form-control {
    background-color: var(--bg-primary);
    border-color: var(--border-color);
    color: var(--text-primary);
    transition: all var(--transition-speed);
}

.form-control:focus {
    background-color: var(--bg-primary);
    border-color: var(--color-primary);
    color: var(--text-primary);
    box-shadow: 0 0 0 0.2rem rgba(52, 152, 219, 0.25);
}

.form-control::placeholder {
    color: var(--text-muted);
}

.btn-outline-primary {
    border-color: var(--color-primary);
    color: var(--color-primary);
    transition: all var(--transition-speed);
}

.btn-outline-primary:hover {
    background-color: var(--color-primary);
    color: white;
}

.navbar-toggler {
    border-color: var(--border-color);
}

.navbar-toggler-icon {
    filter: var(--icon-filter, none);
}

[data-theme='dark'] .navbar-toggler-icon {
    filter: invert(1);
}

.gap-2 {
    gap: 0.5rem;
}
</style>
