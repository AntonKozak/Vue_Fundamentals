<template>
  <div class="form-container">
    <h2>Contact Form</h2>
    <form @submit.prevent="handleSubmit">
      <!-- Звичайне поле без слотів -->
      <FormField
        label="Name"
        type="text"
        placeholder="Enter your name"
        v-model="formData.name"
        :required="true"
        field-id="name"
        hint="Enter your full name"
      />

      <!-- Поле з кастомним label та іконкою -->
      <FormField
        label="Email"
        type="email"
        placeholder="Enter your email"
        v-model="formData.email"
        :required="true"
        field-id="email"
        :has-error="emailError"
        :error-message="emailErrorMessage"
      >
        <template #label="{ label, fieldId }">
          <label :for="fieldId" style="color: #007bff; font-size: 18px;">
            📧 {{ label }} (обов'язково):
          </label>
        </template>

        <template #prefix-icon>
          <span class="prefix-icon">@</span>
        </template>

        <template #suffix-icon>
          <span class="suffix-icon" v-if="formData.email">✓</span>
        </template>
      </FormField>

      <!-- Поле з кастомною підказкою та помилкою -->
      <FormField
        label="Message"
        type="textarea"
        placeholder="Enter your message"
        v-model="formData.message"
        :required="true"
        field-id="message"
        :rows="4"
      >
        <template #hint>
          <div style="color: #42b983; font-style: italic;">
            💡 Мінімум 10 символів. Поточна довжина: {{ formData.message.length }}
          </div>
        </template>

        <template #error="{ hasError }">
          <div v-if="hasError" style="color: #e74c3c; background: #ffeaea; padding: 5px; border-radius: 3px;">
            ❌ Повідомлення занадто коротке!
          </div>
        </template>
      </FormField>

      <!-- Використання ButtonCounter як submit кнопки -->
      <ButtonCounter
        title="Submit Form"
        :count="submitCount"
        button-type="submit"
        :is-form-button="true"
        @increment="handleSubmit"
      />

      <div v-if="isSubmitted" class="success-message">
        <p>✅ Form submitted successfully!</p>
        <p><strong>Name:</strong> {{ submittedData.name }}</p>
        <p><strong>Email:</strong> {{ submittedData.email }}</p>
        <p><strong>Message:</strong> {{ submittedData.message }}</p>
        <p><strong>Submitted {{ submitCount }} times</strong></p>
      </div>
    </form>
  </div>
</template>

<script setup>
import { computed, reactive, ref } from 'vue'
import ButtonCounter from './ButtonCounter.vue'
import FormField from './FormField.vue'

// Дані форми
const formData = reactive({
  name: '',
  email: '',
  message: ''
})

// Стан форми
const submitCount = ref(0)
const isSubmitted = ref(false)
const submittedData = ref({})

// Валідація email
const emailError = computed(() => {
  if (!formData.email) return false
  const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/
  return !emailRegex.test(formData.email)
})

const emailErrorMessage = computed(() => {
  return emailError.value ? 'Невірний формат email!' : ''
})

// Обробка submit
const handleSubmit = () => {
  // Перевірка валідності
  if (!formData.name || !formData.email || !formData.message) {
    alert('Please fill in all fields!')
    return
  }

  if (emailError.value) {
    alert('Please enter a valid email!')
    return
  }

  if (formData.message.length < 10) {
    alert('Message should be at least 10 characters!')
    return
  }

  // Збільшуємо лічильник відправок
  submitCount.value++

  // Зберігаємо дані для відображення
  submittedData.value = { ...formData }
  isSubmitted.value = true

  // Очищаємо форму
  formData.name = ''
  formData.email = ''
  formData.message = ''

  // Ховаємо повідомлення через 5 секунд
  setTimeout(() => {
    isSubmitted.value = false
  }, 5000)

  console.log('Form submitted:', submittedData.value)
}
</script>

<style scoped>
.form-container {
  max-width: 600px;
  margin: 0 auto;
  padding: 20px;
  background-color: #f9f9f9;
  border-radius: 8px;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
}

.success-message {
  margin-top: 20px;
  padding: 15px;
  background-color: #d4edda;
  border: 1px solid #c3e6cb;
  border-radius: 4px;
  color: #155724;
}

.success-message p {
  margin: 5px 0;
}

h2 {
  color: #2c3e50;
  text-align: center;
  margin-bottom: 30px;
}
</style>
