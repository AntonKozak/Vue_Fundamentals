<template>
  <div class="container mt-5">
    <div class="row justify-content-center">
      <div class="col-md-6 col-lg-4">
        <div class="card shadow">
          <div class="card-body p-4">
            <h2 class="text-center mb-4">Sign Up</h2>

            <form @submit.prevent="onSignUpSubmit">
              <div class="mb-3">
                <label
                  for="name"
                  class="form-label"
                >
                  Full Name
                </label>
                <input
                  v-model="formObject.name"
                  type="text"
                  class="form-control"
                  id="name"
                />
              </div>

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
                  for="role"
                  class="form-label"
                >
                  Role
                </label>
                <select
                  class="form-select"
                  id="role"
                  v-model="formObject.role"
                >
                  <option
                    v-for="role in ROLES"
                    :key="role"
                  >
                    {{ role }}
                  </option>
                </select>
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

              <div class="mb-3">
                <label
                  for="confirmPassword"
                  class="form-label"
                >
                  Confirm Password
                </label>
                <input
                  v-model="formObject.confirmPassword"
                  type="password"
                  class="form-control"
                  id="confirmPassword"
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
                class="btn btn-secondary w-100"
              >
                <span
                  v-if="isLoading"
                  class="spinner-border spinner-border-sm me-2"
                ></span>
                Sign Up
              </button>
            </form>

            <div class="text-center mt-3">
              <router-link :to="APP_ROUTES_NAMES.SIGN_IN">
                Already have an account? Login
              </router-link>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ROLES } from '@/constant/constant.js'
import { APP_ROUTES_NAMES } from '@/constant/routeNames'
import router from '@/router/routes'
import { useAuthStore } from '@/stores/authStore'
import { reactive, ref } from 'vue'

const authStore = useAuthStore()

const isLoading = ref(false)
const formObject = reactive({
  name: '',
  email: '',
  role: 'Customer',
  password: '',
  confirmPassword: '',
})

const errorList = reactive([])

const onSignUpSubmit = async () => {
  isLoading.value = true
  errorList.length = 0 // Clear previous errors
  console.log('Form Submitted', formObject)
  try {
    if (!formObject.name) {
      errorList.push('Name is required.')
    }
    if (!formObject.email) {
      errorList.push('Email is required.')
    }
    if (!formObject.password) {
      errorList.push('Password is required.')
    }
    if (!formObject.confirmPassword) {
      errorList.push('Confirm Password is required.')
    }
    if (
      formObject.password &&
      formObject.confirmPassword &&
      formObject.password !== formObject.confirmPassword
    ) {
      errorList.push('Passwords do not match.')
    }

    if (errorList.length > 0) {
      isLoading.value = false
      return
    }
    const response = await authStore.signUp(formObject)
    console.log('Sign Up Response:', response)
    if (response.success) {
      // Handle successful sign up (e.g., redirect to sign in)
      console.log('Sign up successful!')
      router.push({ name: APP_ROUTES_NAMES.SIGN_IN })
    } else {
      if (response.message !== undefined) {
        response.message.split('--').forEach((msg) => errorList.push(msg))
      }
    }
  } catch (error) {
    errorList.push('An error occurred during sign up.', error.message)
  } finally {
    isLoading.value = false
  }
}

console.log(ROLES)
</script>
