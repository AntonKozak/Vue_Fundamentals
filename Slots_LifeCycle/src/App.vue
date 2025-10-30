<script setup>
import { onBeforeUnmount, onMounted } from 'vue'
import PokemonView from './views/PokemonView.vue'

// App-level lifecycle hooks
onMounted(() => {
  console.log('🚀 App mounted - Pokemon Explorer is ready!')
  document.title = 'Pokemon Explorer - Vue 3 Slots & Lifecycle Demo'
})

onBeforeUnmount(() => {
  console.log('👋 App is being unmounted')
})
</script>

<template>
  <div id="app">
    <header class="app-header">
      <div class="header-content">
        <h1>🔥 Vue 3 Learning Lab</h1>
        <p>Exploring <strong>Slots</strong> and <strong>Lifecycle Hooks</strong> with the PokéAPI</p>
        <div class="feature-badges">
          <span class="badge">Composition API</span>
          <span class="badge">Slots</span>
          <span class="badge">Lifecycle Hooks</span>
          <span class="badge">Composables</span>
        </div>
      </div>
    </header>

    <main class="main-content">
      <PokemonView />
    </main>

    <footer class="app-footer">
      <p>Built with Vue 3 + Vite | Data from <a href="https://pokeapi.co/" target="_blank">PokéAPI</a></p>
    </footer>
  </div>
</template>

<style>
/* Global styles */
* {
  margin: 0;
  padding: 0;
  box-sizing: border-box;
}

body {
  font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  min-height: 100vh;
  line-height: 1.6;
}

#app {
  min-height: 100vh;
  display: flex;
  flex-direction: column;
}

.app-header {
  background: rgba(255, 255, 255, 0.95);
  backdrop-filter: blur(10px);
  box-shadow: 0 2px 20px rgba(0, 0, 0, 0.1);
  position: sticky;
  top: 0;
  z-index: 100;
}

.header-content {
  max-width: 1200px;
  margin: 0 auto;
  padding: 30px 20px;
  text-align: center;
}

.header-content h1 {
  color: #2c3e50;
  font-size: 2.5rem;
  margin-bottom: 10px;
  font-weight: 700;
}

.header-content p {
  color: #7f8c8d;
  font-size: 1.2rem;
  margin-bottom: 20px;
}

.feature-badges {
  display: flex;
  justify-content: center;
  gap: 10px;
  flex-wrap: wrap;
}

.badge {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
  padding: 8px 16px;
  border-radius: 20px;
  font-size: 0.9rem;
  font-weight: 600;
  text-transform: uppercase;
  letter-spacing: 0.5px;
  box-shadow: 0 2px 10px rgba(102, 126, 234, 0.3);
}

.main-content {
  flex: 1;
  background: rgba(255, 255, 255, 0.1);
  backdrop-filter: blur(10px);
  margin: 20px;
  border-radius: 20px;
  padding: 20px;
  box-shadow: 0 8px 40px rgba(0, 0, 0, 0.1);
}

.app-footer {
  background: rgba(0, 0, 0, 0.8);
  color: white;
  text-align: center;
  padding: 20px;
  margin-top: auto;
}

.app-footer a {
  color: #74b9ff;
  text-decoration: none;
}

.app-footer a:hover {
  text-decoration: underline;
}

/* Responsive design */
@media (max-width: 768px) {
  .header-content h1 {
    font-size: 2rem;
  }

  .header-content p {
    font-size: 1rem;
  }

  .main-content {
    margin: 10px;
    padding: 15px;
  }

  .feature-badges {
    gap: 8px;
  }

  .badge {
    font-size: 0.8rem;
    padding: 6px 12px;
  }
}

/* Scrollbar styling */
::-webkit-scrollbar {
  width: 8px;
}

::-webkit-scrollbar-track {
  background: #f1f1f1;
  border-radius: 4px;
}

::-webkit-scrollbar-thumb {
  background: #667eea;
  border-radius: 4px;
}

::-webkit-scrollbar-thumb:hover {
  background: #5a6fd8;
}
</style>
