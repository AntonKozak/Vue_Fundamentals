<template>
    <div class='container text-center p-5 border border-3 rounded-3'>
        <p>Lets play</p>
        <h2 class='text-primary p-5'>Current score: {{ gameStore.getScore }}</h2>
        <span class='text-primary p-5'>Max score: {{ gameStore.maxHeath }}</span>

        <br>
        <span class='text-success pb-3' v-if='gameStore.getScore >= gameStore.maxHeath'>You won ! {{
            gameStore.nextAttack }}</span>
        <span class='text-success pb-3' v-if='gameStore.getScore < 0'>You lost ! {{ gameStore.nextAttack }}</span>

        <div class='row' v-if='gameStore.getScore < gameStore.maxHeath && gameStore.getScore > 0'>
            <div class='col-5 offset-1 '>
                <button class='form-control btn btn-success p-4' @click='IncrementScore'>Increment</button>
            </div>

            <div class='col-5 '>
                <button class='form-control btn btn-danger p-4' @click='DecrementScore'>Decrement</button>
            </div>
            <div class='col-6 offset-3 pt-3'>
                <button class='form-control btn btn-warning p-4' @click='RandomScore'>Random</button>
            </div>
        </div>
        <div v-else>
            <button class='form-control btn btn-primary p-4' @click='gameStore.resetScore()'>Reset game</button>
        </div>

    </div>
</template>

<script setup>
import { useGameStore } from '@/stores/gameStore';

const gameStore = useGameStore();


function IncrementScore() {
    gameStore.setNextAttack();
}

function DecrementScore() {
    gameStore.setNextDefense();
}

function RandomScore() {
    Math.random() > 0.5 ? IncrementScore() : DecrementScore();
}
</script>

<style scoped></style>
