<template>
  <div>
    <div v-if="loading">Loading users…</div>
    <div v-else-if="error" class="error">{{ error }}</div>
    <div v-else>
      <div v-for="user in users" :key="user.id">
        <h3>{{ user.name }}</h3>
        <p>{{ user.email }}</p>
        <hr />
      </div>
    </div>
  </div>
</template>

<script setup>
import { onMounted } from 'vue';
import { useUsers } from '../composables/useUsers';

const { users, loading, error, fetchUsers } = useUsers();

onMounted(() => {
  fetchUsers();
});
</script>

<style scoped>
.error {
  color: #b00020;
}
</style>
