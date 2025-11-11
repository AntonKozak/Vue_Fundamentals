<template>
    <div class="container px-3">
        <div v-if="loading" class="d-flex justify-content-center align-items-center vh-100">
            <div class="spinner-grow text-success" role="status">
                <span class="visually-hidden">Loading...</span>
            </div>
        </div>

        <div v-else>
            <div class="card">
                <div
                    class="card-header d-flex flex-column flex-md-row justify-content-between align-items-md-center p-3">
                    <div>
                        <h2 class="h5 text-success mb-0">Menu Items</h2>
                        <p class="mb-0 small text-muted">Manage your restaurant's offerings</p>
                    </div>
                    <button class="btn btn-success btn-sm gap-2 rounded-1 px-4 py-2 mt-2 mt-md-0">
                        <i class="bi bi-plus-square"></i> &nbsp;
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
                                <tr v-for="item in menuItems" :key="item.id">
                                    <td class="ps-3">
                                        <div class="d-flex align-items-center">
                                            <img :src="item.image || 'https://placehold.co/600x400'" :alt="item.name"
                                                class="rounded object-fit-cover me-2"
                                                style="width: 50px; height: 50px" />
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
                                        <span class="badge bg-info bg-opacity-10 text-info small">{{ item.specialTag ||
                                            'N/A' }}</span>
                                    </td>
                                    <td class="pe-3 text-end">
                                        <div class="d-flex gap-2 justify-content-end">
                                            <button class="btn btn-sm btn-outline-success">
                                                <i class="bi bi-pencil-square"></i>
                                            </button>
                                            <button class="btn btn-sm btn-outline-danger">
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
import menuItemService from '@/services/menuItemService';
import { onMounted, reactive, ref } from 'vue';

const menuItems = reactive([])
const loading = ref(false)

const fetchMenuItems = async () => {
    loading.value = true
    try {
        var result = await menuItemService.getMenuItems()
        menuItems.push(...result)
        console.log('Fetched menu items:', menuItems.value)
    } catch (error) {
        console.error('Error fetching menu items:', error)
    } finally {
        loading.value = false
    }
}

onMounted(() => {
    fetchMenuItems()
})

</script>

<style scoped>
/* Optional: Add any custom styling here if needed */
</style>
