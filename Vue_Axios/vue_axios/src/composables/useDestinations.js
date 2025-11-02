import { ref } from 'vue';
import { api } from '../services/http';

export function useDestinations() {
  const destinations = ref([]);
  const loading = ref(false);
  const error = ref('');

  const sleep = (ms) => new Promise((resolve) => setTimeout(resolve, ms));

  const fetchDestinations = async ({ minDelayMs = 3000 } = {}) => {
    loading.value = true;
    error.value = '';
    const start = Date.now();
    try {
      const { data } = await api.get('/destination');
      destinations.value = data;
    } catch (err) {
      console.error(err);
      error.value = 'Failed to load destinations';
    } finally {
      const elapsed = Date.now() - start;
      if (elapsed < minDelayMs) {
        await sleep(minDelayMs - elapsed);
      }
      loading.value = false;
    }
  };

  return { destinations, loading, error, fetchDestinations };
}
