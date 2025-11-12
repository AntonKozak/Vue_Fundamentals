<template>
    <div class="position-fixed top-0 end-0 p-3" style="z-index: 11">
        <div ref="toastElement" :class="['toast', 'align-items-center', 'text-white', 'border-0', `bg-${toastType}`]"
            role="alert" aria-live="assertive" aria-atomic="true">
            <div class="d-flex">
                <div class="toast-body">
                    {{ toastMessage }}
                </div>
                <button type="button" class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast"
                    aria-label="Close"></button>
            </div>
        </div>
    </div>
</template>

<script setup>
import { useToast } from '@/composables/useToast';

const { toastElement, toastMessage, toastType } = useToast();

// Expose the toast element to parent
defineExpose({ toastElement });
</script>

<style scoped>
/* Optional: Custom styles */
</style>
