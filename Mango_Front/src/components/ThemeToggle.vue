<template>
    <button @click="toggleTheme" class="theme-toggle"
        :aria-label="isDark ? 'Switch to light mode' : 'Switch to dark mode'">
        <span class="icon">{{ isDark ? '🌙' : '☀️' }}</span>
    </button>
</template>

<script setup>
import { useThemeStore } from '@/stores/theme'
import { storeToRefs } from 'pinia'

const themeStore = useThemeStore()
const { isDark } = storeToRefs(themeStore)
const { toggleTheme } = themeStore
</script>

<style scoped>
.theme-toggle {
    background: transparent;
    border: 1px solid var(--bs-border-color);
    border-radius: 50%;
    width: 40px;
    height: 40px;
    cursor: pointer;
    display: flex;
    align-items: center;
    justify-content: center;
    transition: all 0.3s ease;
}

.theme-toggle:hover {
    transform: scale(1.1);
    background: var(--bs-secondary-bg);
}

.icon {
    font-size: 1.2rem;
}
</style>
