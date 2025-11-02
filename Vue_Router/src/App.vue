<template>

  <Header></Header>
  <router-view> </router-view>
  <h2>Footer</h2>
</template>

<script setup>
import Header from './components/Layout/Header.vue';
</script>

<style>
#app {
  background-color: #2d2d2d;
  min-height: 100vh;
  color: rgb(122, 122, 122);
  font-family: Arial, sans-serif;
}

h1 {
  text-align: center;
  margin: 0;
}
</style>
