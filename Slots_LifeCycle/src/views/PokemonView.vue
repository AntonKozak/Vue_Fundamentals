<template>
  <div class="pokemon-view">
    <!-- Header Section -->
    <div class="view-header">
      <h1>Pokémon Explorer</h1>
      <p>Demonstrating Vue 3 Lifecycle Hooks and Slots</p>

      <!-- Search Controls -->
      <div class="search-controls">
        <input
          v-model="searchQuery"
          type="text"
          placeholder="Enter Pokémon name..."
          @keyup.enter="handleSearch"
          class="search-input"
        />
        <button @click="handleSearch" :disabled="loading" class="search-btn">
          Search
        </button>
        <button @click="loadDefaultPokemons" :disabled="loading" class="default-btn">
          Load Defaults
        </button>
        <button @click="clearAll" class="clear-btn">
          Clear All
        </button>
      </div>
    </div>

    <!-- Stats Section -->
    <div class="stats-section">
      <div class="stat-card">
        <h4>Total Pokémon</h4>
        <span>{{ pokemons.length }}</span>
      </div>
      <div class="stat-card">
        <h4>API Calls</h4>
        <span>{{ stats.totalFetched }}</span>
      </div>
      <div class="stat-card">
        <h4>Success Rate</h4>
        <span>{{ successRate }}%</span>
      </div>
    </div>

    <!-- Component Lifecycle Log -->
    <div class="lifecycle-log">
      <h3>Lifecycle Events Log</h3>
      <div class="log-container">
        <div
          v-for="(log, index) in lifecycleLogs"
          :key="index"
          :class="['log-entry', log.type]"
        >
          <span class="timestamp">{{ log.timestamp }}</span>
          <span class="event">{{ log.event }}</span>
          <span class="message">{{ log.message }}</span>
        </div>
      </div>
    </div>

    <!-- Pokemon Grid -->
    <div class="pokemon-grid">
      <!-- Loading Card -->
      <PokemonCard v-if="loading && pokemons.length === 0" :is-loading="true">
        <template #header>
          <h3>Fetching Pokémon...</h3>
        </template>
        <template #loading>
          <div class="custom-loading">
            <div class="pokeball-spinner"></div>
            <p>Searching for Pokémon...</p>
          </div>
        </template>
      </PokemonCard>

      <!-- Error Card -->
      <PokemonCard v-if="error && pokemons.length === 0" :has-error="true" :error-message="error">
        <template #header>
          <h3>Oops!</h3>
        </template>
        <template #error="{ error }">
          <div class="custom-error">
            <div class="error-icon">😵</div>
            <p>{{ error }}</p>
            <small>Try searching for a different Pokémon</small>
          </div>
        </template>
      </PokemonCard>

      <!-- Pokemon Cards -->
      <PokemonCard
        v-for="pokemon in pokemons"
        :key="pokemon.id"
        class="pokemon-item"
      >
        <!-- Header slot with Pokemon name -->
        <template #header>
          <h3>{{ pokemon.name }}</h3>
          <small>#{{ pokemon.id.toString().padStart(3, '0') }}</small>
        </template>

        <!-- Main content slot -->
        <template #content>
          <div class="pokemon-details">
            <!-- Image Section -->
            <div class="image-section">
              <img
                :src="pokemon.sprites.front"
                :alt="pokemon.name"
                class="pokemon-image"
                @error="handleImageError"
              />
              <div class="image-controls">
                <button @click="toggleShiny(pokemon)" class="toggle-btn">
                  {{ pokemon.showShiny ? 'Normal' : 'Shiny' }}
                </button>
              </div>
            </div>

            <!-- Info Section -->
            <div class="info-section">
              <div class="basic-info">
                <div class="info-item">
                  <span class="label">Height:</span>
                  <span class="value">{{ pokemon.height }}m</span>
                </div>
                <div class="info-item">
                  <span class="label">Weight:</span>
                  <span class="value">{{ pokemon.weight }}kg</span>
                </div>
                <div class="info-item">
                  <span class="label">Experience:</span>
                  <span class="value">{{ pokemon.baseExperience }}</span>
                </div>
              </div>

              <!-- Types -->
              <div class="types">
                <span class="label">Types:</span>
                <div class="type-badges">
                  <span
                    v-for="type in pokemon.types"
                    :key="type.slot"
                    :class="['type-badge', type.name]"
                  >
                    {{ type.name }}
                  </span>
                </div>
              </div>

              <!-- Abilities -->
              <div class="abilities">
                <span class="label">Abilities:</span>
                <ul class="ability-list">
                  <li v-for="ability in pokemon.abilities" :key="ability.slot">
                    {{ ability.name }}
                    <span v-if="ability.isHidden" class="hidden-badge">Hidden</span>
                  </li>
                </ul>
              </div>
            </div>
          </div>
        </template>

        <!-- Actions slot -->
        <template #actions>
          <button @click="showStats(pokemon)" class="action-btn stats-btn">
            View Stats
          </button>
          <button @click="removePokemon(pokemon.id)" class="action-btn remove-btn">
            Remove
          </button>
        </template>

        <!-- Footer slot -->
        <template #footer>
          <small>Loaded: {{ formatTime(pokemon.loadedAt || new Date()) }}</small>
        </template>
      </PokemonCard>
    </div>

    <!-- Empty State -->
    <div v-if="!loading && pokemons.length === 0 && !error" class="empty-state">
      <div class="empty-icon">🔍</div>
      <h3>No Pokémon Found</h3>
      <p>Search for a Pokémon or load the default collection</p>
    </div>
  </div>
</template>

<script>
import { computed, nextTick, onBeforeMount, onBeforeUnmount, onMounted, onUpdated, ref, watch } from 'vue';
import PokemonCard from '../components/PokemonCard.vue';
import { usePokemonData } from '../composables/usePokemonData.js';

export default {
  name: 'PokemonView',
  components: {
    PokemonCard
  },
  setup() {
    // Composable
    const { pokemons, loading, error, stats, fetchPokemon, fetchMultiplePokemons, clearPokemons, removePokemon } = usePokemonData()

    // Local state
    const searchQuery = ref('')
    const lifecycleLogs = ref([])

    // Computed properties
    const successRate = computed(() => {
      if (stats.totalFetched === 0) return 0
      return Math.round((stats.successCount / stats.totalFetched) * 100)
    })

    // Lifecycle logging function
    const addLifecycleLog = (event, message, type = 'info') => {
      const timestamp = new Date().toLocaleTimeString()
      lifecycleLogs.value.push({
        timestamp,
        event,
        message,
        type
      })
      console.log(`[${timestamp}] ${event}: ${message}`)
    }

    // LIFECYCLE HOOKS DEMONSTRATION
    onBeforeMount(() => {
      addLifecycleLog('onBeforeMount', 'Component is about to be mounted', 'lifecycle')
    })

    onMounted(async () => {
      addLifecycleLog('onMounted', 'Component has been mounted to the DOM', 'lifecycle')

      // Load default Pokemon on mount
      await loadDefaultPokemons()
      addLifecycleLog('onMounted', 'Default Pokémon loaded', 'success')
    })

    onUpdated(() => {
      addLifecycleLog('onUpdated', 'Component has been updated', 'lifecycle')
    })

    onBeforeUnmount(() => {
      addLifecycleLog('onBeforeUnmount', 'Component is about to be unmounted', 'lifecycle')
    })

    // Watch for changes
    watch(pokemons, (newPokemons, oldPokemons) => {
      const newCount = newPokemons.length
      const oldCount = oldPokemons?.length || 0

      if (newCount > oldCount) {
        addLifecycleLog('watch:pokemons', `New Pokémon added. Total: ${newCount}`, 'success')
      } else if (newCount < oldCount) {
        addLifecycleLog('watch:pokemons', `Pokémon removed. Total: ${newCount}`, 'warning')
      }
    }, { deep: true })

    watch(loading, (isLoading) => {
      addLifecycleLog('watch:loading', `Loading state: ${isLoading}`, 'info')
    })

    // Methods
    const handleSearch = async () => {
      if (!searchQuery.value.trim()) return

      addLifecycleLog('user-action', `Searching for: ${searchQuery.value}`, 'info')

      try {
        const pokemon = await fetchPokemon(searchQuery.value)
        pokemon.loadedAt = new Date()
        searchQuery.value = ''
        addLifecycleLog('api-success', `Successfully loaded ${pokemon.name}`, 'success')
      } catch (err) {
        addLifecycleLog('api-error', `Failed to load ${searchQuery.value}: ${err.message}`, 'error')
      }
    }

    const loadDefaultPokemons = async () => {
      addLifecycleLog('user-action', 'Loading default Pokémon collection', 'info')

      const defaultPokemons = ['ditto', 'pikachu', 'charizard', 'blastoise']
      await fetchMultiplePokemons(defaultPokemons)

      // Add load time to each pokemon
      pokemons.value.forEach(pokemon => {
        if (!pokemon.loadedAt) {
          pokemon.loadedAt = new Date()
        }
      })
    }

    const clearAll = () => {
      addLifecycleLog('user-action', 'Clearing all Pokémon', 'warning')
      clearPokemons()
      searchQuery.value = ''
      // Clear some logs to prevent overflow
      if (lifecycleLogs.value.length > 50) {
        lifecycleLogs.value = lifecycleLogs.value.slice(-25)
      }
    }

    const toggleShiny = async (pokemon) => {
      pokemon.showShiny = !pokemon.showShiny
      addLifecycleLog('user-action', `Toggled shiny for ${pokemon.name}`, 'info')

      // Force reactivity
      await nextTick()
    }

    const showStats = (pokemon) => {
      addLifecycleLog('user-action', `Viewing stats for ${pokemon.name}`, 'info')
      // In a real app, this would open a modal or navigate to a detail page
      console.log('Pokemon stats:', pokemon.stats)
    }

    const handleImageError = (event) => {
      addLifecycleLog('error', 'Failed to load Pokémon image', 'error')
      event.target.src = 'https://via.placeholder.com/150x150?text=No+Image'
    }

    const formatTime = (date) => {
      return date.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })
    }

    return {
      // State
      pokemons,
      loading,
      error,
      stats,
      searchQuery,
      lifecycleLogs,

      // Computed
      successRate,

      // Methods
      handleSearch,
      loadDefaultPokemons,
      clearAll,
      removePokemon,
      toggleShiny,
      showStats,
      handleImageError,
      formatTime
    }
  }
}
</script>

<style scoped>
.pokemon-view {
  max-width: 1200px;
  margin: 0 auto;
  padding: 20px;
}

.view-header {
  text-align: center;
  margin-bottom: 30px;
}

.view-header h1 {
  color: #2c3e50;
  margin-bottom: 10px;
}

.view-header p {
  color: #7f8c8d;
  font-size: 1.1rem;
}

.search-controls {
  display: flex;
  gap: 10px;
  justify-content: center;
  margin-top: 20px;
  flex-wrap: wrap;
}

.search-input {
  padding: 12px 16px;
  border: 2px solid #e9ecef;
  border-radius: 8px;
  font-size: 1rem;
  min-width: 250px;
  transition: border-color 0.3s;
}

.search-input:focus {
  outline: none;
  border-color: #667eea;
}

.search-btn, .default-btn, .clear-btn {
  padding: 12px 20px;
  border: none;
  border-radius: 8px;
  font-size: 1rem;
  cursor: pointer;
  transition: all 0.3s;
}

.search-btn {
  background: #667eea;
  color: white;
}

.search-btn:hover:not(:disabled) {
  background: #5a6fd8;
}

.search-btn:disabled {
  background: #bdc3c7;
  cursor: not-allowed;
}

.default-btn {
  background: #2ecc71;
  color: white;
}

.default-btn:hover:not(:disabled) {
  background: #27ae60;
}

.clear-btn {
  background: #e74c3c;
  color: white;
}

.clear-btn:hover {
  background: #c0392b;
}

.stats-section {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(150px, 1fr));
  gap: 20px;
  margin-bottom: 30px;
}

.stat-card {
  background: white;
  padding: 20px;
  border-radius: 12px;
  text-align: center;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
}

.stat-card h4 {
  margin: 0 0 10px 0;
  color: #7f8c8d;
  font-size: 0.9rem;
  text-transform: uppercase;
  letter-spacing: 1px;
}

.stat-card span {
  font-size: 2rem;
  font-weight: bold;
  color: #2c3e50;
}

.lifecycle-log {
  background: #f8f9fa;
  border-radius: 12px;
  padding: 20px;
  margin-bottom: 30px;
}

.lifecycle-log h3 {
  margin: 0 0 15px 0;
  color: #2c3e50;
}

.log-container {
  max-height: 200px;
  overflow-y: auto;
  border: 1px solid #e9ecef;
  border-radius: 8px;
  background: white;
}

.log-entry {
  display: flex;
  gap: 15px;
  padding: 8px 12px;
  border-bottom: 1px solid #f8f9fa;
  font-size: 0.9rem;
}

.log-entry:last-child {
  border-bottom: none;
}

.log-entry.lifecycle {
  background: #e3f2fd;
}

.log-entry.success {
  background: #e8f5e8;
}

.log-entry.error {
  background: #ffebee;
}

.log-entry.warning {
  background: #fff3e0;
}

.timestamp {
  font-weight: bold;
  color: #6c757d;
  min-width: 80px;
}

.event {
  font-weight: 600;
  color: #495057;
  min-width: 120px;
}

.message {
  color: #6c757d;
  flex: 1;
}

.pokemon-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(350px, 1fr));
  gap: 20px;
  margin-bottom: 30px;
}

.pokemon-details {
  display: flex;
  gap: 20px;
}

.image-section {
  flex-shrink: 0;
  text-align: center;
}

.pokemon-image {
  width: 120px;
  height: 120px;
  background: #f8f9fa;
  border-radius: 8px;
  border: 2px solid #e9ecef;
}

.image-controls {
  margin-top: 10px;
}

.toggle-btn {
  background: #6c757d;
  color: white;
  border: none;
  padding: 6px 12px;
  border-radius: 6px;
  font-size: 0.8rem;
  cursor: pointer;
}

.toggle-btn:hover {
  background: #5a6268;
}

.info-section {
  flex: 1;
}

.basic-info {
  margin-bottom: 15px;
}

.info-item {
  display: flex;
  justify-content: space-between;
  margin-bottom: 8px;
}

.label {
  font-weight: 600;
  color: #495057;
}

.value {
  color: #6c757d;
}

.types {
  margin-bottom: 15px;
}

.type-badges {
  display: flex;
  gap: 5px;
  flex-wrap: wrap;
  margin-top: 5px;
}

.type-badge {
  padding: 4px 8px;
  border-radius: 12px;
  font-size: 0.8rem;
  color: white;
  text-transform: capitalize;
  background: #6c757d;
}

/* Type-specific colors */
.type-badge.fire { background: #ff6b6b; }
.type-badge.water { background: #74b9ff; }
.type-badge.grass { background: #55a3ff; }
.type-badge.electric { background: #fed330; color: #2d3436; }
.type-badge.psychic { background: #fd79a8; }
.type-badge.normal { background: #a4b0be; }

.abilities {
  margin-bottom: 15px;
}

.ability-list {
  margin: 5px 0;
  padding-left: 20px;
}

.ability-list li {
  margin: 3px 0;
  text-transform: capitalize;
}

.hidden-badge {
  background: #fd79a8;
  color: white;
  padding: 2px 6px;
  border-radius: 8px;
  font-size: 0.7rem;
  margin-left: 5px;
}

.action-btn {
  padding: 8px 16px;
  border: none;
  border-radius: 6px;
  cursor: pointer;
  font-size: 0.9rem;
  transition: all 0.3s;
}

.stats-btn {
  background: #74b9ff;
  color: white;
}

.stats-btn:hover {
  background: #0984e3;
}

.remove-btn {
  background: #ff6b6b;
  color: white;
}

.remove-btn:hover {
  background: #ee5a52;
}

.empty-state {
  text-align: center;
  padding: 60px 20px;
  color: #6c757d;
}

.empty-icon {
  font-size: 4rem;
  margin-bottom: 20px;
}

.custom-loading {
  text-align: center;
}

.pokeball-spinner {
  width: 50px;
  height: 50px;
  border: 5px solid #f3f3f3;
  border-top: 5px solid #ff6b6b;
  border-radius: 50%;
  animation: spin 1s linear infinite;
  margin: 0 auto 15px;
}

.custom-error {
  text-align: center;
}

.error-icon {
  font-size: 3rem;
  margin-bottom: 15px;
}

/* Responsive */
@media (max-width: 768px) {
  .pokemon-grid {
    grid-template-columns: 1fr;
  }

  .search-controls {
    flex-direction: column;
    align-items: center;
  }

  .search-input {
    min-width: 200px;
  }

  .pokemon-details {
    flex-direction: column;
    align-items: center;
  }

  .stats-section {
    grid-template-columns: repeat(auto-fit, minmax(120px, 1fr));
  }
}
</style>
