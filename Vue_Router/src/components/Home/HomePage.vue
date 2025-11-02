<template>
    <div>
        <h1>Home page</h1>
        <button @click='goToContact'> Contact Us</button>
    </div>

    <button @click='$router.go(-1)'>Back</button>
    <button @click='$router.go(+1)'>Forward</button>
</template>

<script setup>
import { useRouter } from 'vue-router';

const router = useRouter();

function goToContact() {
    router.push({ name: 'contact' });
}
</script>
