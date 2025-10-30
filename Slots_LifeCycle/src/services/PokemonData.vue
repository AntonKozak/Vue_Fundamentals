<template>
  <div class="pokemon-container">
    <h2>Pokémon Data Fetcher</h2>

    <div v-if="loading" class="loading">
      Loading Pokémon data...
    </div>

    <div v-else-if="error" class="error">
      Error: {{ error }}
    </div>

    <div v-else-if="pokemon" class="pokemon-card">
      <h3>{{ pokemon.name }}</h3>
      <div class="pokemon-info">
        <img :src="pokemon.sprites.front_default" :alt="pokemon.name" />
        <div class="details">
          <p><strong>Height:</strong> {{ pokemon.height / 10 }} m</p>
          <p><strong>Weight:</strong> {{ pokemon.weight / 10 }} kg</p>
          <p><strong>Base Experience:</strong> {{ pokemon.base_experience }}</p>
          <p><strong>Types:</strong>
            <span v-for="type in pokemon.types" :key="type.slot" class="type-badge">
              {{ type.type.name }}
            </span>
          </p>
          <p><strong>Abilities:</strong></p>
          <ul>
            <li v-for="ability in pokemon.abilities" :key="ability.slot">
              {{ ability.ability.name }}
              <span v-if="ability.is_hidden">(Hidden)</span>
            </li>
          </ul>
        </div>
      </div>
    </div>

    <button @click="fetchPokemon" :disabled="loading" class="fetch-btn">
      {{ loading ? 'Loading...' : 'Fetch Ditto Data' }}
    </button>
  </div>
</template>

<script>
import { onMounted, ref } from 'vue';

export default {
  name: 'PokemonData',
  setup() {
    const pokemon = ref(null)
    const loading = ref(false)
    const error = ref(null)

    const fetchPokemon = async () => {
      loading.value = true
      error.value = null

      try {
        const response = await fetch('https://pokeapi.co/api/v2/pokemon/ditto')

        if (!response.ok) {
          throw new Error(`HTTP error! status: ${response.status}`)
        }

        const data = await response.json()
        pokemon.value = data

        console.log('Pokémon data:', data)
      } catch (err) {
        error.value = err.message
        console.error('Error fetching Pokémon data:', err)
      } finally {
        loading.value = false
      }
    }

    // Automatically fetch data when component mounts
    onMounted(() => {
      fetchPokemon()
    })

    return {
      pokemon,
      loading,
      error,
      fetchPokemon
    }
  }
}
</script>

<style scoped>
.pokemon-container {
  max-width: 600px;
  margin: 0 auto;
  padding: 20px;
  font-family: Arial, sans-serif;
}

.loading {
  text-align: center;
  font-size: 18px;
  color: #666;
}

.error {
  background-color: #ffe6e6;
  color: #d63031;
  padding: 15px;
  border-radius: 8px;
  border-left: 4px solid #d63031;
}

.pokemon-card {
  background: #f8f9fa;
  border-radius: 12px;
  padding: 20px;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
  margin: 20px 0;
}

.pokemon-card h3 {
  color: #2d3436;
  text-transform: capitalize;
  margin-bottom: 20px;
  font-size: 24px;
}

.pokemon-info {
  display: flex;
  gap: 20px;
  align-items: flex-start;
}

.pokemon-info img {
  width: 150px;
  height: 150px;
  background: #fff;
  border-radius: 8px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
}

.details p {
  margin: 8px 0;
  color: #2d3436;
}

.details strong {
  color: #636e72;
}

.type-badge {
  background: #74b9ff;
  color: white;
  padding: 4px 8px;
  border-radius: 12px;
  font-size: 12px;
  margin-left: 5px;
  text-transform: capitalize;
}

.details ul {
  margin: 5px 0;
  padding-left: 20px;
}

.details li {
  margin: 3px 0;
  text-transform: capitalize;
}

.fetch-btn {
  background: #0984e3;
  color: white;
  border: none;
  padding: 12px 24px;
  border-radius: 8px;
  cursor: pointer;
  font-size: 16px;
  transition: background-color 0.3s;
}

.fetch-btn:hover:not(:disabled) {
  background: #0770c7;
}

.fetch-btn:disabled {
  background: #b2bec3;
  cursor: not-allowed;
}

@media (max-width: 600px) {
  .pokemon-info {
    flex-direction: column;
    align-items: center;
  }

  .pokemon-info img {
    width: 120px;
    height: 120px;
  }
}
</style>
