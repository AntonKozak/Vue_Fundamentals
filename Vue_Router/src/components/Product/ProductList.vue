<template>
    <div>
        <h1>List of Product</h1>
        <div v-for="product in products">
            <router-link :to="{ name: 'productDetails', params: { productId: product.id, categoryId: 777 } }">{{
                product.name
                }}</router-link>
        </div>
    </div>
</template>

<script setup>
const products = [
    { id: 1, name: 'Product 1' },
    { id: 2, name: 'Product 2' },
    { id: 3, name: 'Product 3' },
];
</script>
