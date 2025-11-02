<template>
  <div>
    <div v-if="loading">
      <Loader message="Loading destinations…" />
    </div>
    <div v-else-if="error" class="error">{{ error }}</div>
    <div v-else>
      <div v-for="destination in destinations" :key="destination.id">
        <h3>{{ destination.name }}</h3>
        <p>{{ destination.funFact }}</p>
        <hr />
      </div>
    </div>
  </div>
</template>

<script setup>
import { onMounted } from 'vue';
import { useDestinations } from '../composables/useDestinations';
import Loader from './Loader.vue';

const { destinations, loading, error, fetchDestinations } = useDestinations();

onMounted(() => {
  fetchDestinations();
});
</script>

<style scoped>
.error {
  color: #b00020;
}
</style>
