<template>
  <div
    v-if="isOpen"
    class="position-fixed top-0 start-0 w-100 h-100 d-flex justify-content-center align-items-center bg-black bg-opacity-50"
    style="z-index: 1050"
  >
    <div
      class="bg-body p-4 rounded-4 shadow-lg mx-3 overflow-auto"
      style="max-width: 500px; width: 100%; max-height: 90vh"
    >
      <div class="d-flex justify-content-between align-items-center mb-4">
        <h4 class="fw-bold m-0">Complete Your Order</h4>
        <button
          class="btn"
          @click="closeModal"
        >
          <i class="bi bi-x-circle"></i>
        </button>
      </div>

      <div
        v-if="errorList.length > 0"
        class="alert alert-danger"
        role="alert"
      >
        <span
          v-for="error in errorList"
          :key="error"
          class="d-block"
        >
          {{ error }}
        </span>
      </div>

      <form @submit.prevent="submitOrder">
        <div class="mb-3">
          <label
            for="pickupName"
            class="form-label"
          >
            Name
          </label>
          <input
            v-model="orderData.pickUpName"
            type="text"
            class="form-control"
            id="pickupName"
          />
        </div>

        <div class="mb-3">
          <label
            for="pickupPhoneNumber"
            class="form-label"
          >
            Phone Number
          </label>
          <input
            v-model="orderData.pickUpPhoneNumber"
            type="tel"
            class="form-control"
            id="pickupPhoneNumber"
          />
        </div>

        <div class="mb-4">
          <label
            for="pickupEmail"
            class="form-label"
          >
            Email
          </label>
          <input
            v-model="orderData.pickUpEmail"
            type="email"
            class="form-control"
            id="pickupEmail"
          />
        </div>

        <div class="bg-body-tertiary rounded-3 p-3 mb-4">
          <h5 class="fw-bold mb-3">Order Summary</h5>
          <div v-if="cartStore.cartItems.length > 0">
            <div
              v-for="item in cartStore.cartItems"
              :key="item.id"
              class="d-flex justify-content-between align-items-center mb-2"
            >
              <span class="fw-medium">{{ item.name }}</span>
              <div class="d-flex align-items-center gap-3">
                <span class="text-body-secondary">{{ item.quantity }}</span>
                <span class="fw-medium">{{ item.price }}</span>
              </div>
            </div>
            <div class="border-top pt-3 mt-3">
              <div class="d-flex justify-content-between align-items-center">
                <span class="fw-bold">Total</span>
                <span class="fw-bold fs-5">{{ cartStore.cartTotal }}</span>
              </div>
            </div>
          </div>
        </div>

        <div class="d-flex justify-content-end gap-2">
          <button
            @click="closeModal"
            type="button"
            class="btn btn-outline-secondary px-4"
          >
            Cancel
          </button>
          <button
            type="submit"
            class="btn btn-success px-4"
            :disabled="isSubmitting || !cartStore.cartItems || cartStore.cartItems.length === 0"
          >
            <span
              v-if="isSubmitting"
              class="d-flex align-items-center gap-2"
            >
              <div
                class="spinner-border spinner-border-sm"
                role="status"
              >
                <span class="visually-hidden">Loading...</span>
              </div>
              Processing...
            </span>
            <span v-else>Place Order</span>
          </button>
        </div>
      </form>
    </div>
  </div>
</template>

<script setup>
import { APP_ROUTES_NAMES } from '@/constant/routeNames'
import orderService from '@/services/orderService'
import { useAuthStore } from '@/stores/authStore'
import { useCartStore } from '@/stores/cartStore'
import { onMounted, reactive, ref } from 'vue'
import { useRouter } from 'vue-router'

const isSubmitting = ref(false)
const errorList = reactive([])
const cartStore = useCartStore()
const authStore = useAuthStore()
const router = useRouter()
const orderData = reactive({
  pickUpName: '',
  pickUpPhoneNumber: '',
  pickUpEmail: '',
  applicationUserId: '',
  orderTotal: 0,
  totalItem: 0,
  orderDetails: [],
})

const props = defineProps({
  isOpen: {
    type: Boolean,
    default: false,
  },
})

const emit = defineEmits(['close'])

const closeModal = () => {
  emit('close')
}

onMounted(() => {
  if (authStore.isAuthenticated) {
    orderData.applicationUserId = authStore.user.id
    orderData.pickUpName = authStore.user.name
    orderData.pickUpEmail = authStore.user.email
  }
})

const submitOrder = async () => {
  try {
    isSubmitting.value = true
    errorList.length = 0

    if (!orderData.pickUpName || !orderData.pickUpPhoneNumber || !orderData.pickUpEmail) {
      errorList.push('Please fill in all required fields.')
      return
    }

    if (errorList.length > 0) {
      isSubmitting.value = false
      return
    }

    orderData.orderTotal = cartStore.cartTotal
    orderData.totalItem = cartStore.cartCount
    orderData.orderDetails = cartStore.cartItems.map((item) => ({
      menuItemId: item.id,
      quantity: item.quantity,
      itemName: item.name,
      price: item.price,
    }))

    const orderHeader = await orderService.createOrder(orderData)

    if (orderHeader && orderHeader.orderHeaderId > 0) {
      console.log('Order placed successfully:', orderHeader)
      cartStore.clearCart()
      closeModal()
      router.push({
        name: APP_ROUTES_NAMES.ORDER_CONFIRM,
        params: { orderId: orderHeader.orderHeaderId },
      })
    }
  } catch (error) {
    errorList.push(
      'An error occurred while submitting your order. Please try again.',
      error.message
    )
  } finally {
    isSubmitting.value = false
  }
}
</script>
