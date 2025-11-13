import { defineStore } from 'pinia';
import { computed, reactive } from 'vue';

export const useCartStore = defineStore('cart', () => {

    const cartItems = reactive([]);

    const cartCount = computed(() =>{
        return cartItems.reduce((total, item) => total + item.quantity, 0)
    });

    const cartTotal = computed(() =>{
        return cartItems.reduce((total, item) => total + item.quantity * item.price, 0)
    });

    function addToCart(menuItem, quantity = 1) {
        const existingItem = cartItems.find(item => item.id === menuItem.id);
        if (existingItem) {
            existingItem.quantity += quantity;
        } else {
            cartItems.push({
                id: menuItem.id,
                name: menuItem.name,
                image: menuItem.image,
                quantity: quantity,
                price: menuItem.price,
            });
        }
    }

    function updateQuantity(menuItemId, quantity) {
        const item = cartItems.find(item => item.id === menuItemId);
        if (item) {
            if (item.quantity <= 0) {
                removeFromCart(menuItemId);
            } else {
                item.quantity = quantity;
            }
        }
    }

    function removeFromCart(menuItemId) {
        const index = cartItems.findIndex(item => item.id === menuItemId);
        if (index !== -1) {
            cartItems.splice(index, 1);
        }
    }

    function clearCart() {
        cartItems.length = 0;
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
});
