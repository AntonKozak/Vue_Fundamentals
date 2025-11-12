<template>
    <!-- Toast Notification -->
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

    <div class="d-flex justify-content-center align-items-center" v-if="loading">
        <div class="spinner-grow text-success" style="width: 2.5rem; height: 2.5rem" role="status">
            <span class="visually-hidden">Loading...</span>
        </div>
    </div>

    <div class="container" v-else>
        <div class="mx-auto">
            <div class="mb-4 border-bottom d-flex justify-content-between align-items-center py-3">
                <h3 class="fw-semibold text-success">{{ menuItemIdForUpdate ? 'Edit Menu' : 'Add Menu' }}</h3>
                <div class="d-flex gap-3">
                    <button type="submit" form="menuForm" class="btn btn-success btn-sm gap-2 rounded-1 px-4 py-2"
                        :disabled="isProcessing">
                        <span v-if="isProcessing" class="spinner-border spinner-border-sm me-2"></span>
                        {{ isProcessing ? (menuItemIdForUpdate ? 'Updating...' : 'Creating...') : (menuItemIdForUpdate ?
                            'Update Item' : 'Create Item') }}
                    </button>

                    <button @click="router.push({ name: APP_ROUTES_NAMES.MENU_ITEM_LIST })" type="button"
                        class="btn btn-outline border btn-sm gap-2 rounded-1 px-4 py-2">
                        Cancel
                    </button>
                </div>
            </div>
            <div class="alert alert-danger pb-0" v-if="errorList.length > 0">
                Please fix the following errors:
                <ul>
                    <li v-for="error in errorList" :key="error">{{ error }}</li>
                </ul>
            </div>
            <form enctype="multipart/form-data" class="needs-validation" id="menuForm" @submit="onFormSubmit">
                <div class="row g-4">
                    <div class="col-lg-7">
                        <div class="d-flex flex-column g-12">
                            <div class="mb-3">
                                <label for="name" class="form-label">Item Name</label>
                                <input id="name" type="text" v-model="menuItemObj.name" class="form-control"
                                    placeholder="Enter item name" />
                            </div>

                            <div class="mb-3">
                                <label for="description" class="form-label">Description</label>
                                <textarea id="description" v-model="menuItemObj.description" class="form-control"
                                    placeholder="Describe the menu item..." rows="3"></textarea>
                            </div>

                            <div class="mb-3">
                                <label for="specialTag" class="form-label">Special Tag (Optional)</label>
                                <input id="specialTag" v-model="menuItemObj.specialTag" type="text" class="form-control"
                                    placeholder="e.g., Chef's Special" />
                            </div>

                            <div class="mb-3">
                                <label for="category" class="form-label">Category</label>
                                <select id="category" class="form-select" v-model="menuItemObj.category">

                                    <option> -- Select category --</option>
                                    <option v-for="category in CATEGORIES" :key="category"> {{ category }}</option>
                                </select>
                            </div>

                            <div class="mb-3">
                                <label for="price" class="form-label">Price</label>
                                <input id="price" class="form-control" v-model="menuItemObj.price" />
                            </div>
                        </div>
                    </div>

                    <div class="col-lg-5">
                        <div>
                            <img v-if="newUploadImage_base64 || menuItemObj.image"
                                :src="newUploadImage_base64 || (menuItemObj.image ? CONFIG_IMAGE_URL + menuItemObj.image : '')"
                                class="img-fluid w-100 mb-3 rounded" style="aspect-ratio: 1/1; object-fit: cover" />
                            <div class="mb-3">
                                <label for="image" class="form-label">Item Image</label>
                                <input id="image" type="file" class="form-control" accept="image/*"
                                    @change="handleFileChange" />
                                <div class="form-text">{{ menuItemIdForUpdate ? 'Leave empty to keep existing image' :
                                    'Please select an image' }}</div>
                            </div>
                        </div>
                    </div>
                </div>
            </form>
        </div>
    </div>
</template>

<script setup>
import { useToast } from '@/composables/useToast';
import { CONFIG_IMAGE_URL } from '@/constant/config';
import { CATEGORIES } from '@/constant/constant';
import { APP_ROUTES_NAMES } from '@/constant/routeNames';
import menuItemService from '@/services/menuItemService';
import { onMounted, reactive, ref } from 'vue';
import { useRoute, useRouter } from 'vue-router';


const loading = ref(false);
const isProcessing = ref(false);
const router = useRouter();
const route = useRoute();
const newUploadImage = ref(null);
const newUploadImage_base64 = ref('');
const menuItemIdForUpdate = route.params.id;

const { toastElement, toastMessage, toastType, showSuccess, showError } = useToast();

const errorList = reactive([]);

const menuItemObj = reactive({
    name: '',
    description: '',
    specialTag: '',
    category: '',
    price: 0.0,
    image: ''
});

const handleFileChange = (event) => {
    isProcessing.value = true;
    const file = event.target.files[0];
    console.log('File selected:', file);
    newUploadImage.value = file;
    if (file) {
        const reader = new FileReader();
        reader.onload = (e) => {
            newUploadImage_base64.value = e.target.result;
        };
        reader.readAsDataURL(file);
        isProcessing.value = false;
    } else {
        newUploadImage.value = null;
        newUploadImage_base64.value = '';
        isProcessing.value = false;
    }
};

onMounted(async () => {
    if (!menuItemIdForUpdate) return;

    loading.value = true;
    try {
        const result = await menuItemService.getMenuItemById(menuItemIdForUpdate);
        Object.assign(menuItemObj, result);
    } catch (error) {
        console.error('Error fetching menu item for update:', error);
    } finally {
        loading.value = false;
    }
});

const onFormSubmit = async (e) => {
    e.preventDefault();
    isProcessing.value = true;
    errorList.length = 0;

    if (menuItemObj.name.length < 3) {
        errorList.push('Name must be at least 3 characters long.');
    }
    if (menuItemObj.description.length < 10) {
        errorList.push('Description must be at least 10 characters long.');
    }
    if (menuItemObj.category === '' || menuItemObj.category === ' -- Select category --') {
        errorList.push('Please select a category.');
    }
    if (menuItemObj.price <= 0) {
        errorList.push('Price must be greater than zero.');
    }
    if (!newUploadImage.value && !menuItemIdForUpdate) {
        errorList.push('Please upload an image for the menu item.');
    }

    if (!errorList.length) {

        const submitFormData = new FormData();

        submitFormData.append('Name', menuItemObj.name);
        submitFormData.append('Description', menuItemObj.description);
        submitFormData.append('Price', menuItemObj.price);
        submitFormData.append('Category', menuItemObj.category);
        submitFormData.append('SpecialTag', menuItemObj.specialTag || '');

        if (newUploadImage.value) {
            submitFormData.append('File', newUploadImage.value);
            console.log('File appended to FormData:', newUploadImage.value.name);
        } else {
            console.error('No file selected!');
        }

        try {
            const response = menuItemIdForUpdate
                ? await menuItemService.updateMenuItem(menuItemIdForUpdate, submitFormData)
                : await menuItemService.createMenuItem(submitFormData);
            console.log('Menu item saved successfully:', response);

            showSuccess(menuItemIdForUpdate ? '✓ Menu item updated successfully!' : '✓ Menu item created successfully!');

            router.push({ name: APP_ROUTES_NAMES.MENU_ITEM_LIST });

        } catch (error) {
            console.error('Error creating menu item:', error);
            if (error.response && error.response.data) {
                console.error('Server error:', error.response.data);

                if (error.response.data.errors) {
                    console.error('Validation errors:', error.response.data.errors);
                    Object.entries(error.response.data.errors).forEach(([field, messages]) => {
                        messages.forEach(msg => errorList.push(`${field}: ${msg}`));
                    });
                } else if (error.response.data.errorMessages) {
                    error.response.data.errorMessages.forEach(err => errorList.push(err));
                } else if (error.response.data.title) {
                    errorList.push(error.response.data.title);
                }
            } else {
                errorList.push('An error occurred while creating the menu item. Please try again.');
            }
        } finally {
            isProcessing.value = false;
        }
    } else {
        isProcessing.value = false;
    }
};


</script>

<style scoped></style>
