<template>
  <div>
    <h1>Contact us</h1>

    <!-- Додаємо форму -->
    <ContactForm />

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
        title="Contact Independent Counter 1"
        :count="contactCounter1"
        @increment="incrementContact1"
      />
      <ButtonCounter
        title="Contact Independent Counter 2"
        :count="contactCounter2"
        @increment="incrementContact2"
      />
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue'
import { useSharedCounters } from '../composables/useSharedCounters.js'
import ButtonCounter from './ButtonCounter.vue'
import ContactForm from './ContactForm.vue'

// Shared counters (global state)
const { sharedCountA, sharedCountB, incrementSharedA, incrementSharedB } = useSharedCounters()

// Independent counters (local to Contact page)
const contactCounter1 = ref(0)
const contactCounter2 = ref(0)

const incrementContact1 = () => {
  contactCounter1.value++
}

const incrementContact2 = () => {
  contactCounter2.value++
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
