import { Toast } from 'bootstrap';
import { ref } from 'vue';

export const useToast = () => {
    const toastElement = ref(null);
    const toastMessage = ref('');
    const toastType = ref('success'); // success, danger, warning, info

    const showToast = (message, type = 'success') => {
        toastMessage.value = message;
        toastType.value = type;

        if (toastElement.value) {
            const toast = new Toast(toastElement.value);
            toast.show();
        }
    };

    const showSuccess = (message) => {
        showToast(message, 'success');
    };

    const showError = (message) => {
        showToast(message, 'danger');
    };

    const showWarning = (message) => {
        showToast(message, 'warning');
    };

    const showInfo = (message) => {
        showToast(message, 'info');
    };

    return {
        toastElement,
        toastMessage,
        toastType,
        showToast,
        showSuccess,
        showError,
        showWarning,
        showInfo
    };
};
