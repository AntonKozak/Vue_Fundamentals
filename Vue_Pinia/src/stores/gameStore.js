import { defineStore } from 'pinia'
import { computed, ref } from 'vue'

export const useGameStore = defineStore('gameStore', () => {
  const score = ref(50)
  const maxHeath = ref(100)
  const maxAttack = ref(50)
  const maxDefense = ref(50)

  const getScore = computed(() => score.value)
  const getWinningScore = computed(() => maxHeath.value)

  const setNextAttack = () => {
    const attack = Math.floor(Math.random() * maxAttack.value) + 1
    score.value += attack
  }

  const setNextDefense = () => {
    const defense = Math.floor(Math.random() * maxDefense.value) + 1
    score.value -= defense
  }

  const resetScore = () => {
    score.value = 50
  }

  return {
    score,
    maxHeath,
    maxAttack,
    maxDefense,
    getScore,
    getWinningScore,
    setNextAttack,
    setNextDefense,
    resetScore,
  }
})
