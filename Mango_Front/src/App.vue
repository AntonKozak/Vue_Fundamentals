<template>
  <div id="app" class="app-container">
    <Header />
    <main class="main-content">
      <RouterView />
    </main>
    <Footer />
  </div>
</template>

<script setup>
import { onMounted } from 'vue';
import { RouterView } from 'vue-router';
import Footer from './components/layout/Footer.vue';
import Header from './components/layout/Header.vue';
import { useThemeStore } from './stores/theme';

const themeStore = useThemeStore();

// Ensure theme is applied on mount
onMounted(() => {
  themeStore.initTheme();
});
</script>

<style>
/* Global styles */
* {
  margin: 0;
  padding: 0;
  box-sizing: border-box;
}

html,
body {
  height: 100%;
}

#app {
  font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, Oxygen, Ubuntu, Cantarell, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
}
</style>

<style scoped>
.app-container {
  display: flex;
  flex-direction: column;
  min-height: 100vh;
  max-width: 1200px;
  margin: 0 auto;
  padding: 0 1rem;
}

.main-content {
  flex: 1 0 auto;
  width: 100%;
  padding: 2rem 0;
}

/* Responsive padding */
@media (max-width: 768px) {
  .app-container {
    padding: 0 0.5rem;
  }

  .main-content {
    padding: 1rem 0;
  }
}
</style>
