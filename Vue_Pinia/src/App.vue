<template>

  <header>
    <div class='row'>
      <div class='col-4 offset-1'>
        <router-link :to='{ name: "Home" }'
          class='form-control badge rounded-pill text-bg-warning p-4'>Home</router-link>
      </div>
      <div class='col-4 offset-1'>
        <router-link :to='{ name: "Info" }'
          class='form-control badge rounded-pill text-bg-warning p-4'>Info</router-link>
      </div>
    </div>
  </header>
  <RouterView />

</template>

<script setup>
</script>
<style scoped></style>
