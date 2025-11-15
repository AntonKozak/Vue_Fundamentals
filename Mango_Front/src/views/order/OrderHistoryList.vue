<template>
    <div class="container py-5">
    <div class="d-flex align-items-center mb-4">
      <i class="bi bi-bag" style="font-size: 2.5rem"></i> &nbsp;
      <h1 class="mb-0">My Orders</h1>
    </div>

    <div class="text-center py-5" v-if="loading">
      <p class="text-body-secondary">Loading your orders...</p>
    </div>

    <div class="text-center py-5" v-else-if="orders.length === 0">
      <div class="bg-body-tertiary rounded-4 p-5">
        <i class="bi bi-bag" style="font-size: 2.5rem"></i>
        <h3 class="mb-3">No Orders Yet</h3>
        <p class="text-body-secondary mb-4">
          Start your culinary journey by exploring our delicious menu!
        </p>
        <router-link :to="{name: APP_ROUTES_NAMES.HOME}" class="btn btn-success btn-lg">
          <i class="bi bi-menu-button-wide"></i>
          Browse Menu
        </router-link>
      </div>
    </div>

    <div class="row g-4" v-else>
      <div class="col-md-6 col-lg-4" v-for="order in orders" :key="order.orderHeaderId">
   <OrderListCard :order="order"></OrderListCard>
      </div>
    </div>
  </div>
</template>

<script setup>
import OrderListCard from '@/components/Card/OrderListCard.vue';
import { APP_ROUTES_NAMES } from '@/constant/routeNames';
import orderService from '@/services/orderService';
import { useAuthStore } from '@/stores/authStore';
import { onMounted, reactive, ref } from 'vue';

const authStore = useAuthStore();
const orders = reactive([]);
const loading = ref(false);

const fetchOrders = async () => {
    orders.length = 0;
    loading.value = true;
try {
const result = await orderService.getOrders(authStore.user.id);
    orders.push(...result);

} catch (error) {
    console.error('Error fetching orders:', error);
    throw error;
} finally {
    loading.value = false;
    }
};

onMounted(fetchOrders);
</script>
