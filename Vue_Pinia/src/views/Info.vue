<template>
    <div>
        <p>Welcome to theInfo page!</p>

        <h2 class='text-primary p-5'>Current score: {{ gameStore.getScore }}</h2>
        <span class='text-primary p-5'>Winning score: {{ gameStore.getWinningScore }}</span>


        <input type="text" v-model="gameStore.maxHeath" class='form-control' placeholder='Set winning score' />
    </div>
</template>

<script setup>
import { useGameStore } from '@/stores/gameStore';
const gameStore = useGameStore();

</script>

<style scoped></style>
