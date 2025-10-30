import { ref } from 'vue'

// Global shared state - these will persist across page navigation
const sharedCount = ref(0)

export function useSharedCounters() {
  const incrementSharedA = () => {
    sharedCount.value++
  }

  const incrementSharedB = () => {
    sharedCount.value++
  }

  return {
    sharedCountA: sharedCount,
    sharedCountB: sharedCount,
    incrementSharedA,
    incrementSharedB
  }
}
