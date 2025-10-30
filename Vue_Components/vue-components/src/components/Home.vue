<template>
  <div>
    <h1>You did it!</h1>
    <p>
      Visit <a href="https://vuejs.org/" target="_blank" rel="noopener">vuejs.org</a> to read the
      documentation
    </p>

    <div class="counters-section">
      <h2>Shared Counters (Communication Between Pages)</h2>
      <p>These counters share the same state across both pages:</p>
      <ButtonCounter
        title="Shared Counter A"
        :count="sharedCountA"
        :shared="true"
        @increment="incrementSharedA"
      />
      <ButtonCounter
        title="Shared Counter B"
        :count="sharedCountB"
        :shared="true"
        @increment="incrementSharedB"
      />

      <h2>Independent Counters (No Communication)</h2>
      <p>These counters only count clicks on this page:</p>
      <ButtonCounter
        title="Home Independent Counter 1"
        :count="homeCounter1"
        @increment="incrementHome1"
      />
      <ButtonCounter
        title="Home Independent Counter 2"
        :count="homeCounter2"
        @increment="incrementHome2"
      />
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue'
import { useSharedCounters } from '../composables/useSharedCounters.js'
import ButtonCounter from './ButtonCounter.vue'

// Shared counters (global state)
const { sharedCountA, sharedCountB, incrementSharedA, incrementSharedB } = useSharedCounters()

// Independent counters (local to Home page)
const homeCounter1 = ref(0)
const homeCounter2 = ref(0)

const incrementHome1 = () => {
  homeCounter1.value++
}

const incrementHome2 = () => {
  homeCounter2.value++
}
</script>

<style scoped>
.counters-section {
  margin-top: 30px;
  text-align: left;
  max-width: 800px;
  margin-left: auto;
  margin-right: auto;
}

h2 {
  color: #2c3e50;
  border-bottom: 2px solid #42b983;
  padding-bottom: 10px;
}

p {
  color: #666;
  margin-bottom: 20px;
}
</style>
