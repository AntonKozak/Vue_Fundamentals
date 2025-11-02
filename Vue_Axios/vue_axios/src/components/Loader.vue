<template>
    <div class="loader-wrap" role="status" aria-live="polite" aria-busy="true">
        <span class="spinner" />
        <span v-if="message" class="msg">{{ message }}</span>
    </div>
</template>

<script setup>
defineProps({
    message: {
        type: String,
        default: '',
    },
});
</script>

<style scoped>
.loader-wrap {
    display: inline-flex;
    align-items: center;
    gap: 0.5rem;
}

.spinner {
    width: 16px;
    height: 16px;
    border: 3px solid #ddd;
    border-top-color: #42b883;
    border-radius: 50%;
    display: inline-block;
    animation: spin 0.8s linear infinite;
}

.msg {
    font-size: 0.95rem;
    color: #333;
}

@keyframes spin {
    to {
        transform: rotate(360deg);
    }
}
</style>
