import { ref } from 'vue';
import axios from 'axios';

export function useUsers() {
  const users = ref([]);
  const loading = ref(false);
  const error = ref('');

  const fetchUsers = async () => {
    loading.value = true;
    error.value = '';
    try {
      const { data } = await axios.get('https://jsonplaceholder.typicode.com/users');
      users.value = data;
    } catch (err) {
      console.error(err);
      error.value = 'Failed to load users';
    } finally {
      loading.value = false;
    }
  };

  return { users, loading, error, fetchUsers };
}
