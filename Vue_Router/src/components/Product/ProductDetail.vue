<template>
    <div>
        <h1>Product detail</h1>
        <div v-if='route.params.productId'>
            <p>Product ID {{ route.params.productId }}</p>
            <p>Category ID {{ route.params.categoryId }}</p>
        </div>
        <div v-else>
            <p>No product selected</p>
        </div>
    </div>
</template>

<script setup>
import { useRoute } from 'vue-router';

const route = useRoute();
console.log(route.params);

</script>
