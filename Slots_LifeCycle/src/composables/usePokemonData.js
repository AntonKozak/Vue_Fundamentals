import { reactive, ref } from 'vue'

export function usePokemonData() {
  const pokemons = ref([])
  const loading = ref(false)
  const error = ref(null)

  // Stats for demonstration
  const stats = reactive({
    totalFetched: 0,
    successCount: 0,
    errorCount: 0
  })

  const fetchPokemon = async (pokemonName) => {
    loading.value = true
    error.value = null

    try {
      const response = await fetch(`https://pokeapi.co/api/v2/pokemon/${pokemonName.toLowerCase()}`)

      if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`)
      }

      const data = await response.json()

      // Transform data to a cleaner format
      const pokemonData = {
        id: data.id,
        name: data.name,
        height: data.height / 10, // Convert to meters
        weight: data.weight / 10, // Convert to kg
        baseExperience: data.base_experience,
        sprites: {
          front: data.sprites.front_default,
          back: data.sprites.back_default,
          frontShiny: data.sprites.front_shiny
        },
        types: data.types.map(type => ({
          name: type.type.name,
          slot: type.slot
        })),
        abilities: data.abilities.map(ability => ({
          name: ability.ability.name,
          isHidden: ability.is_hidden,
          slot: ability.slot
        })),
        stats: data.stats.map(stat => ({
          name: stat.stat.name,
          baseStat: stat.base_stat,
          effort: stat.effort
        }))
      }

      // Add to the list if not already present
      const existingIndex = pokemons.value.findIndex(p => p.id === pokemonData.id)
      if (existingIndex === -1) {
        pokemons.value.push(pokemonData)
      } else {
        pokemons.value[existingIndex] = pokemonData
      }

      stats.totalFetched++
      stats.successCount++

      return pokemonData
    } catch (err) {
      error.value = err.message
      stats.totalFetched++
      stats.errorCount++
      console.error('Error fetching Pokémon data:', err)
      throw err
    } finally {
      loading.value = false
    }
  }

  const fetchMultiplePokemons = async (pokemonNames) => {
    const promises = pokemonNames.map(name => fetchPokemon(name))
    try {
      await Promise.allSettled(promises)
    } catch (err) {
      console.error('Error fetching multiple Pokémon:', err)
    }
  }

  const clearPokemons = () => {
    pokemons.value = []
    error.value = null
    stats.totalFetched = 0
    stats.successCount = 0
    stats.errorCount = 0
  }

  const removePokemon = (pokemonId) => {
    const index = pokemons.value.findIndex(p => p.id === pokemonId)
    if (index !== -1) {
      pokemons.value.splice(index, 1)
    }
  }

  return {
    // State
    pokemons,
    loading,
    error,
    stats,

    // Methods
    fetchPokemon,
    fetchMultiplePokemons,
    clearPokemons,
    removePokemon
  }
}
