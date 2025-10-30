<template>
  <div class="button-counter">
    <h3>{{ title }}</h3>
    <button
      @click="increment"
      :type="buttonType"
      :class="{ 'submit-button': isFormButton }"
    >
      {{ isFormButton ? `${title} (${count})` : `Count: ${count}` }}
    </button>
  </div>
</template>

<script setup>
const props = defineProps({
  title: {
    type: String,
    default: 'Button Counter'
  },
  count: {
    type: Number,
    default: 0
  },
  shared: {
    type: Boolean,
    default: false
  },
  buttonType: {
    type: String,
    default: 'button'
  },
  isFormButton: {
    type: Boolean,
    default: false
  }
})

const emit = defineEmits(['increment'])

const increment = () => {
  emit('increment')
}
</script>

<style scoped>
.button-counter {
  margin: 20px;
  padding: 20px;
  border: 2px solid #42b983;
  border-radius: 8px;
  background-color: #f9f9f9;
}

button {
  background-color: #42b983;
  color: white;
  border: none;
  padding: 10px 20px;
  font-size: 16px;
  border-radius: 4px;
  cursor: pointer;
  transition: background-color 0.3s;
}

button:hover {
  background-color: #369870;
}

.submit-button {
  background-color: #007bff !important;
  font-size: 18px;
  padding: 12px 24px;
  font-weight: bold;
}

.submit-button:hover {
  background-color: #0056b3 !important;
}

h3 {
  margin-top: 0;
  color: #2c3e50;
}
</style>
