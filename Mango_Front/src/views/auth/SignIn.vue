<template>
  <div class="container mt-5">
    <div class="row justify-content-center">
      <div class="col-md-6 col-lg-4">
        <div class="card shadow">
          <div class="card-body p-4">
            <h2 class="text-center mb-4">Sign In</h2>
            <form @submit.prevent="onSignInSubmit">
              <div class="mb-3">
                <label
                  for="email"
                  class="form-label"
                >
                  Email
                </label>
                <input
                  v-model="formObject.email"
                  type="email"
                  class="form-control"
                  id="email"
                />
              </div>

              <div class="mb-3">
                <label
                  for="password"
                  class="form-label"
                >
                  Password
                </label>
                <input
                  v-model="formObject.password"
                  type="password"
                  class="form-control"
                  id="password"
                />
              </div>

              <div
                v-if="errorList.length > 0"
                class="alert alert-danger"
              >
                <span
                  v-for="error in errorList"
                  :key="error"
                  class="d-block"
                >
                  {{ error }}
                </span>
              </div>

              <button
                :disabled="isLoading"
                type="submit"
                class="btn btn-success w-100"
              >
                <span
                  v-if="isLoading"
                  class="spinner-border spinner-border-sm me-2"
                ></span>
                Login
              </button>
            </form>

            <div class="text-center mt-3">
              <router-link :to="APP_ROUTES_NAMES.SIGN_UP">
                Don't have an account? Sign up
              </router-link>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { APP_ROUTES_NAMES } from '@/constant/routeNames'
import router from '@/router/routes'
import { useAuthStore } from '@/stores/authStore'
import { reactive, ref } from 'vue'

const authStore = useAuthStore()

const errorList = reactive([])
const isLoading = ref(false)
const formObject = reactive({
  email: '',
  password: '',
})

const onSignInSubmit = async () => {
  isLoading.value = true
  errorList.length = 0 // Clear previous errors
  console.log('Form Submitted', formObject)
  try {
    if (!formObject.email) {
      errorList.push('Email is required.')
    }
    if (!formObject.password) {
      errorList.push('Password is required.')
    }

    if (errorList.length > 0) {
      isLoading.value = false
      return
    }

    const response = await authStore.signIn(formObject)
    console.log('Sign In Response:', response)

    if (response.success) {
      console.log('Sign in successful!', response.data)
      router.push({ name: 'home' }) // Redirect to home page
    } else {
      if (response.message) {
        errorList.push(response.message)
      }
    }
  } catch (error) {
    errorList.push('An error occurred during sign in.', error.message)
  } finally {
    isLoading.value = false
  }
}
</script>
<style scoped></style>
