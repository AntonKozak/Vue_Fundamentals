<template> <div class="container-fluid py-2">
    <!-- <h1 class="mb-4">Order Management</h1> -->
    <p class="text-success h2 pb-1">Order Management</p>
    <!-- Filters -->
    <div class="card border-0 shadow-sm p-4 mb-4">
      <div class="row">
        <div class="col-md-4 mb-3">
          <label class="form-label">Filter by Status</label>
          <select v-model="statusFilter" class="form-select">
            <option value="">All Status</option>
            <option v-for="status in ORDER_STATUS" :key="status" :value="status">{{ status }}</option>
          </select>
        </div>
        <div class="col-md-4 mb-3">
          <label class="form-label">Sort By</label>
          <select class="form-select">
            <option value="orderHeaderId">Order ID</option>
            <option value="orderTotal">Total Amount</option>
            <option value="pickUpName">Customer Name</option>
          </select>
        </div>
        <div class="col-md-4 mb-3">
          <label class="form-label">Sort Direction</label>
          <select class="form-select">
            <option value="asc">Ascending</option>
            <option value="desc">Descending</option>
          </select>
        </div>
      </div>
      <div class="row mt-2">
        <div class="col-md-8 mb-3">
          <label class="form-label">Search</label>
          <input v-model="searchQuery" type="text" class="form-control" placeholder="Search by name, email or phone" />
        </div>
        <div class="col-md-4 mb-3 d-flex align-items-end">
          <button class="btn btn-outline-secondary w-100">Reset Filters</button>
        </div>
      </div>
    </div>

    <div v-if="loading" class="text-center py-4 fs-5 text-body-secondary">Loading orders...</div>
    <div class="text-center py-5 card border-0 shadow-sm" v-else-if="filterOrders.length === 0">
      <p class="mb-0">No orders found matching your criteria.</p>
    </div>
    <div v-else>
      <div class="mb-3">
        <span class="badge bg-success">´{{ filterOrders.length }} orders found</span>
      </div>
      <div class="table-responsive card border-0 shadow-sm">
        <table class="table table-hover mb-0">
          <thead>
            <tr>
              <th class="cursor-pointer">
                Order ID
                <span class="ms-1"> ↑↓ </span>
              </th>
              <th class="cursor-pointer">
                Customer
                <span class="ms-1"> ↑↓ </span>
              </th>
              <th>Contact</th>
              <th>Number of Items</th>
              <th class="cursor-pointer">
                Total
                <span class="ms-1"> ↑↓ </span>
              </th>
              <th>Status</th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="order in filterOrders" :key="order.orderHeaderId">
              <td>#{{ order.orderHeaderId }}</td>
                <td>{{ order.pickUpName }}</td>
                  <td>
                <div> {{ order.pickUpPhoneNumber }} </div>
                <div class="text-body-secondary small">{{ order.pickUpEmail }}</div>
              </td>
              <td>{{ order.totalItem }}</td>
              <td>${{ order.orderTotal }}</td>
              <td>
                <div class="badge rounded-pill">{{ order.status }}</div>
              </td>
              <td>
                <button class="btn btn-sm btn-success">
                  <i class="bi bi-card-checklist"></i> &nbsp;View Details
                </button>
              </td>
            </tr>
          </tbody>
        </table>
      </div>

      <!-- Pagination -->
      <nav aria-label="Order pagination" class="mt-4 d-flex justify-content-end">
        <ul class="pagination pagination-md">
          <!-- First page button -->
          <li class="page-item">
            <a class="page-link text-success border-success" href="#" aria-label="First">
              <span aria-hidden="true">&laquo;</span>
              <span class="visually-hidden">First page</span>
            </a>
          </li>

          <!-- Previous button -->
          <li class="page-item">
            <a class="page-link text-success border-success" href="#" aria-label="Previous">
              <span aria-hidden="true">&lsaquo;</span>
              <span class="visually-hidden">Previous page</span>
            </a>
          </li>

          <!-- Page numbers with limited display -->
          <li class="page-item disabled">
            <span class="page-link border-success">...</span>
          </li>
          <li class="page-item">
            <a class="page-link text-muted border-success" href="#"> XX </a>
          </li>
          <!-- Next button -->
          <li class="page-item">
            <a class="page-link text-success border-success" href="#" aria-label="Next">
              <span aria-hidden="true">&rsaquo;</span>
              <span class="visually-hidden">Next page</span>
            </a>
          </li>

          <!-- Last page button -->
          <li class="page-item">
            <a class="page-link text-success border-success" href="#" aria-label="Last">
              <span aria-hidden="true">&raquo;</span>
              <span class="visually-hidden">Last page</span>
            </a>
          </li>
        </ul>
      </nav>
    </div>

    <!-- Order Details Modal Component -->
  </div></template>


<script setup>
import { ORDER_STATUS } from '@/constant/constant';
import orderService from '@/services/orderService';
import { computed, onMounted, reactive, ref } from 'vue';

const loading = ref(false);
const orders = reactive([]);

const statusFilter = ref('');
const searchQuery = ref('');
const sortBy = ref('orderHeaderId');
const sortDirection = ref('desc');

const itemsPerPage = ref(5);
const currentPage = ref(1);

const resetFilters = () => {
    statusFilter.value = '';
    searchQuery.value = '';
    sortBy.value = 'orderHeaderId';
    sortDirection.value = 'desc';
    currentPage.value = 1;
};

const filterOrders = computed(() => {
    let result = [...orders];
    if(statusFilter.value) {
        result = result.filter(order => order.status.toUpperCase() === statusFilter.value.toUpperCase());
    }
    if (searchQuery.value) {
        const query = searchQuery.value.toUpperCase();
        result = result.filter(order =>
            order.pickUpName.toUpperCase().includes(query) ||
            order.pickUpEmail.toUpperCase().includes(query) ||
            order.pickUpPhoneNumber.toUpperCase().includes(query)
        );
    }
    // result.sort((a, b) => {
    //     let fieldA = a[sortBy.value];
    //     let fieldB = b[sortBy.value];
    //     if (typeof fieldA === 'string') fieldA = fieldA.toUpperCase();
    //     if (typeof fieldB === 'string') fieldB = fieldB.toUpperCase();
    //     if (fieldA < fieldB) return sortDirection.value === 'asc' ? -1 : 1;
    //     if (fieldA > fieldB) return sortDirection.value === 'asc' ? 1 : -1;
    //     return 0;


    // });

    return result;
});

const fetchOrders = async () => {
    orders.length = 0;
    loading.value = true;
    try {
        var result = await orderService.getOrders();
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
