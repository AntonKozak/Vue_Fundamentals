import { defineStore } from 'pinia';
import { computed, ref } from 'vue';

export const useCartStore = defineStore('cart', () => {

    const cartItems = ref([]);

    const cartCount = computed(() =>{
        return cartItems.value.reduce((total, item) => total + item.quantity, 0)
    });

    const cartTotal = computed(() =>{
        return cartItems.value.reduce((total, item) => total + item.quantity * item.price, 0)
    });

    function addToCart(menuItem, quantity = 1) {
        const existingItem = cartItems.value.find(item => item.id === menuItem.id);
        if (existingItem) {
            existingItem.quantity += quantity;
        } else {
            cartItems.value.push({
                id: menuItem.id,
                name: menuItem.name,
                image: menuItem.image,
                quantity: quantity,
                price: menuItem.price,
            });
        }
    }

    function updateQuantity(menuItemId, quantity) {
        const item = cartItems.value.find(item => item.id === menuItemId);
        if (item) {
            if (item.quantity <= 0) {
                removeFromCart(menuItemId);
            } else {
                item.quantity = quantity;
            }
        }
    }

    function removeFromCart(menuItemId) {
        const index = cartItems.value.findIndex(item => item.id === menuItemId);
        if (index !== -1) {
            cartItems.value.splice(index, 1);
        }
    }

    function clearCart() {
        cartItems.value = [];
    }

    return {
        cartItems,
        cartCount,
        cartTotal,
       addToCart,
        clearCart,
        updateQuantity,
        removeFromCart,
    };
},
    {
    persist: true,
});
