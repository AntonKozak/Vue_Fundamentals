<template>
    <nav class="navbar navbar-expand-lg bg-body-tertiary">
        <div class="container-fluid">
            <router-link class="navbar-brand" :to="{ name: 'home' }">Navbar</router-link>

            <div class="collapse navbar-collapse" id="navbarSupportedContent">
                <ul class="navbar-nav me-auto mb-2 mb-lg-0">
                    <li class="nav-item">
                        <router-link class="nav-link " to="/">Home</router-link>
                    </li>
                    <li class="nav-item">
                        <router-link class="nav-link" :to="{ name: 'contact' }">Contact us</router-link>
                    </li>
                    <li class="nav-item dropdown">
                        <router-link class="nav-link dropdown-toggle" role="button" data-bs-toggle="dropdown">
                            Product
                        </router-link>
                        <ul class="dropdown-menu">
                            <li><router-link class="dropdown-item" to="/productList">Product List</router-link></li>
                            <li><router-link class="dropdown-item" to='/product'>Product detail</router-link></li>
                        </ul>
                    </li>
                </ul>
                <form class="d-flex" role="search">
                    <router-link class="nav-link " :to='{ name: "login" }'>Login</router-link>
                </form>
            </div>
        </div>
    </nav>
</template>

<script setup>
</script>


<style scoped></style>
