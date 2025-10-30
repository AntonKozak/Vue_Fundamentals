<template>
  <div class="form-group">
    <!-- Слот для label - можна кастомізувати -->
    <slot name="label" :label="label" :fieldId="fieldId">
      <label :for="fieldId">{{ label }}:</label>
    </slot>

    <!-- Слот для іконки перед полем -->
    <div class="input-wrapper">
      <slot name="prefix-icon"></slot>

      <input
        v-if="type !== 'textarea'"
        :id="fieldId"
        :type="type"
        :placeholder="placeholder"
        :value="modelValue"
        @input="$emit('update:modelValue', $event.target.value)"
        :required="required"
        :class="{ 'has-prefix': $slots['prefix-icon'], 'has-suffix': $slots['suffix-icon'] }"
      />
      <textarea
        v-else
        :id="fieldId"
        :placeholder="placeholder"
        :value="modelValue"
        @input="$emit('update:modelValue', $event.target.value)"
        :required="required"
        :rows="rows"
      ></textarea>

      <!-- Слот для іконки після поля -->
      <slot name="suffix-icon"></slot>
    </div>

    <!-- Слот для підказки -->
    <slot name="hint" :hint="hint">
      <small v-if="hint" class="hint">{{ hint }}</small>
    </slot>

    <!-- Слот для помилок -->
    <slot name="error" :errorMessage="errorMessage" :hasError="hasError">
      <span v-if="hasError" class="error">{{ errorMessage }}</span>
    </slot>
  </div>
</template>

<script setup>
defineProps({
  label: {
    type: String,
    required: true
  },
  type: {
    type: String,
    default: 'text'
  },
  placeholder: {
    type: String,
    default: ''
  },
  modelValue: {
    type: String,
    default: ''
  },
  required: {
    type: Boolean,
    default: false
  },
  fieldId: {
    type: String,
    required: true
  },
  rows: {
    type: Number,
    default: 4
  },
  hint: {
    type: String,
    default: ''
  },
  errorMessage: {
    type: String,
    default: ''
  },
  hasError: {
    type: Boolean,
    default: false
  }
})

defineEmits(['update:modelValue'])
</script>

<style scoped>
.form-group {
  margin-bottom: 20px;
}

label {
  display: block;
  margin-bottom: 5px;
  font-weight: bold;
  color: #2c3e50;
}

.input-wrapper {
  position: relative;
  display: flex;
  align-items: center;
}

input,
textarea {
  width: 100%;
  padding: 10px;
  border: 1px solid #ddd;
  border-radius: 4px;
  font-size: 16px;
  transition: border-color 0.3s;
  box-sizing: border-box;
}

input.has-prefix {
  padding-left: 35px;
}

input.has-suffix {
  padding-right: 35px;
}

input:focus,
textarea:focus {
  outline: none;
  border-color: #42b983;
  box-shadow: 0 0 5px rgba(66, 185, 131, 0.3);
}

.hint {
  display: block;
  margin-top: 5px;
  color: #666;
  font-size: 12px;
}

.error {
  display: block;
  margin-top: 5px;
  color: #e74c3c;
  font-size: 12px;
  font-weight: bold;
}

/* Стилі для іконок */
:deep(.prefix-icon),
:deep(.suffix-icon) {
  position: absolute;
  top: 50%;
  transform: translateY(-50%);
  color: #666;
  pointer-events: none;
}

:deep(.prefix-icon) {
  left: 10px;
}

:deep(.suffix-icon) {
  right: 10px;
}
</style>
