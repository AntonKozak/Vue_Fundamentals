<template>
  <div class="pokemon-card" :class="{ 'loading': isLoading, 'error': hasError }">
    <!-- Header slot with fallback content -->
    <div class="card-header">
      <slot name="header">
        <h3>Pokémon Card</h3>
      </slot>
    </div>

    <!-- Main content area -->
    <div class="card-content">
      <!-- Loading state slot -->
      <div v-if="isLoading" class="loading-state">
        <slot name="loading">
          <div class="spinner"></div>
          <p>Loading...</p>
        </slot>
      </div>

      <!-- Error state slot -->
      <div v-else-if="hasError" class="error-state">
        <slot name="error" :error="errorMessage">
          <div class="error-icon">⚠️</div>
          <p>{{ errorMessage || 'An error occurred' }}</p>
        </slot>
      </div>

      <!-- Main content slot -->
      <div v-else class="main-content">
        <slot name="content">
          <p>No content provided</p>
        </slot>
      </div>
    </div>

    <!-- Actions slot -->
    <div class="card-actions" v-if="$slots.actions">
      <slot name="actions"></slot>
    </div>

    <!-- Footer slot -->
    <div class="card-footer" v-if="$slots.footer">
      <slot name="footer"></slot>
    </div>
  </div>
</template>

<script>
export default {
  name: 'PokemonCard',
  props: {
    isLoading: {
      type: Boolean,
      default: false
    },
    hasError: {
      type: Boolean,
      default: false
    },
    errorMessage: {
      type: String,
      default: ''
    }
  }
}
</script>

<style scoped>
.pokemon-card {
  background: white;
  border-radius: 16px;
  box-shadow: 0 4px 20px rgba(0, 0, 0, 0.1);
  overflow: hidden;
  transition: all 0.3s ease;
  border: 2px solid transparent;
  min-height: 200px;
  display: flex;
  flex-direction: column;
}

.pokemon-card:hover {
  transform: translateY(-4px);
  box-shadow: 0 8px 30px rgba(0, 0, 0, 0.15);
}

.pokemon-card.loading {
  border-color: #74b9ff;
  background: linear-gradient(135deg, #f8f9ff 0%, #e8f0ff 100%);
}

.pokemon-card.error {
  border-color: #ff6b6b;
  background: linear-gradient(135deg, #fff8f8 0%, #ffe8e8 100%);
}

.card-header {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
  padding: 20px;
  text-align: center;
}

.card-header h3 {
  margin: 0;
  font-size: 1.5rem;
  font-weight: 600;
  text-transform: capitalize;
}

.card-content {
  flex: 1;
  padding: 20px;
  display: flex;
  flex-direction: column;
  justify-content: center;
}

.loading-state {
  text-align: center;
  color: #74b9ff;
}

.spinner {
  width: 40px;
  height: 40px;
  border: 4px solid #e3f2fd;
  border-top: 4px solid #74b9ff;
  border-radius: 50%;
  animation: spin 1s linear infinite;
  margin: 0 auto 10px;
}

@keyframes spin {
  0% { transform: rotate(0deg); }
  100% { transform: rotate(360deg); }
}

.error-state {
  text-align: center;
  color: #ff6b6b;
}

.error-icon {
  font-size: 2rem;
  margin-bottom: 10px;
}

.main-content {
  flex: 1;
}

.card-actions {
  padding: 15px 20px;
  border-top: 1px solid #f0f0f0;
  display: flex;
  gap: 10px;
  justify-content: center;
  flex-wrap: wrap;
}

.card-footer {
  background: #f8f9fa;
  padding: 15px 20px;
  border-top: 1px solid #e9ecef;
  font-size: 0.9rem;
  color: #6c757d;
}

/* Responsive design */
@media (max-width: 768px) {
  .pokemon-card {
    margin: 10px;
  }

  .card-header {
    padding: 15px;
  }

  .card-content {
    padding: 15px;
  }

  .card-header h3 {
    font-size: 1.3rem;
  }
}
</style>
