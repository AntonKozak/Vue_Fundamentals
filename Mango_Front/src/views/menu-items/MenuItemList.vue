<template>
  <div
    class="position-fixed top-0 end-0 p-3"
    style="z-index: 11"
  >
    <div
      ref="toastElement"
      :class="['toast', 'align-items-center', 'text-white', 'border-0', `bg-${toastType}`]"
      role="alert"
      aria-live="assertive"
      aria-atomic="true"
    >
      <div class="d-flex">
        <div class="toast-body">
          {{ toastMessage }}
        </div>
        <button
          type="button"
          class="btn-close btn-close-white me-2 m-auto"
          data-bs-dismiss="toast"
          aria-label="Close"
        ></button>
      </div>
    </div>
  </div>

  <div class="container px-3">
    <div
      v-if="loading"
      class="d-flex justify-content-center align-items-center vh-100"
    >
      <div
        class="spinner-grow text-success"
        role="status"
      >
        <span class="visually-hidden">Loading...</span>
      </div>
    </div>

    <div v-else>
      <div class="card">
        <div
          class="card-header d-flex flex-column flex-md-row justify-content-between align-items-md-center p-3"
        >
          <div>
            <h2 class="h5 text-success mb-0">Menu Items</h2>
            <p class="mb-0 small text-muted">Manage your restaurant's offerings</p>
          </div>
          <button
            class="btn btn-success btn-sm gap-2 rounded-1 px-4 py-2 mt-2 mt-md-0"
            @click="router.push({ name: APP_ROUTES_NAMES.CREATE_MENU_ITEM })"
          >
            <i class="bi bi-plus-square"></i>
            &nbsp;
            <span>Add Item</span>
          </button>
        </div>
        <div class="card-body p-3">
          <div class="table-responsive">
            <table class="table table-hover align-middle mb-0">
              <thead>
                <tr>
                  <th class="ps-3 small">Item</th>
                  <th class="small">Category</th>
                  <th class="small">Price</th>
                  <th class="small">Tag</th>
                  <th class="pe-3 text-end small">Actions</th>
                </tr>
              </thead>
              <tbody>
                <tr
                  v-for="item in menuItems"
                  :key="item.id"
                >
                  <td class="ps-3">
                    <div class="d-flex align-items-center">
                      <img
                        :src="CONFIG_IMAGE_URL + item.image"
                        :alt="item.name"
                        class="rounded object-fit-cover me-2"
                        style="width: 50px; height: 50px"
                      />
                      <div>
                        <div class="fw-semibold small">{{ item.name }}</div>
                      </div>
                    </div>
                  </td>
                  <td>
                    <span class="badge bg-success bg-opacity-10 text-success small">
                      {{ item.category }}
                    </span>
                  </td>
                  <td class="fw-semibold small">${{ item.price }}</td>
                  <td>
                    <span class="badge bg-info bg-opacity-10 text-info small">
                      {{ item.specialTag || 'N/A' }}
                    </span>
                  </td>
                  <td class="pe-3 text-end">
                    <div class="d-flex gap-2 justify-content-end">
                      <button
                        class="btn btn-sm btn-outline-success"
                        @click="
                          router.push({
                            name: APP_ROUTES_NAMES.EDIT_MENU_ITEM,
                            params: { id: item.id },
                          })
                        "
                      >
                        <i class="bi bi-pencil-square"></i>
                      </button>
                      <button
                        class="btn btn-sm btn-outline-danger"
                        @click="handleDelete(item.id, item.name)"
                      >
                        <i class="bi bi-trash3-fill"></i>
                      </button>
                    </div>
                  </td>
                </tr>
              </tbody>
            </table>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { useToast } from '@/composables/useToast'
import { CONFIG_IMAGE_URL } from '@/constant/config'
import { APP_ROUTES_NAMES } from '@/constant/routeNames'
import menuItemService from '@/services/menuItemService'
import { onMounted, reactive, ref } from 'vue'
import { useRouter } from 'vue-router'

const menuItems = reactive([])
const loading = ref(false)
const router = useRouter()
const { toastElement, toastMessage, toastType, showSuccess, showError } = useToast()

const fetchMenuItems = async () => {
  loading.value = true
  try {
    var result = await menuItemService.getMenuItems()
    menuItems.push(...result)
    console.log('Fetched menu items:', menuItems)
  } catch (error) {
    console.error('Error fetching menu items:', error)
  } finally {
    loading.value = false
  }
}

const handleDelete = async (id, name) => {
  if (confirm(`Are you sure you want to delete "${name}"?`)) {
    loading.value = true
    try {
      await menuItemService.deleteMenuItem(id)
      const index = menuItems.findIndex((item) => item.id === id)
      if (index > -1) {
        menuItems.splice(index, 1)
      }
      showSuccess('✓ Menu item deleted successfully!')
    } catch (error) {
      console.error('Error deleting menu item:', error)
      showError('Failed to delete menu item. Please try again.')
    } finally {
      loading.value = false
    }
  }
}

onMounted(() => {
  fetchMenuItems()
})
</script>

<style scoped>
/* Optional: Add any custom styling here if needed */
</style>
