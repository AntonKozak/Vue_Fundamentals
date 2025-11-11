<template>
    <footer class="footer bg-body-secondary border-top">
        <div class="footer-content">
            <!-- Company Info Section -->
            <div class="footer-section">
                <h3 class="footer-title text-success">{{ companyName }}</h3>
                <p class="footer-description text-muted">{{ companyDescription }}</p>
            </div>

            <!-- Quick Links Section -->
            <div class="footer-section">
                <h4 class="section-title">Quick Links</h4>
                <nav class="footer-nav">
                    <ul>
                        <li v-for="link in quickLinks" :key="link.name">
                            <router-link :to="link.path" class="footer-link">
                                {{ link.name }}
                            </router-link>
                        </li>
                    </ul>
                </nav>
            </div>

            <!-- Contact Section -->
            <div class="footer-section">
                <h4 class="section-title">Contact Us</h4>
                <address class="contact-info text-muted">
                    <p v-if="contact.email">
                        <span class="icon">📧</span>
                        <a :href="`mailto:${contact.email}`" class="footer-link">
                            {{ contact.email }}
                        </a>
                    </p>
                    <p v-if="contact.phone">
                        <span class="icon">📞</span>
                        <a :href="`tel:${contact.phone}`" class="footer-link">
                            {{ contact.phone }}
                        </a>
                    </p>
                    <p v-if="contact.address">
                        <span class="icon">📍</span>
                        {{ contact.address }}
                    </p>
                </address>
            </div>

            <!-- Social Media Section -->
            <div class="footer-section">
                <h4 class="section-title">Follow Us</h4>
                <div class="social-links">
                    <a v-for="social in socialMedia" :key="social.name" :href="social.url" :aria-label="social.name"
                        class="social-link" target="_blank" rel="noopener noreferrer">
                        {{ social.icon }}
                    </a>
                </div>
            </div>
        </div>

        <!-- Copyright Section -->
        <div class="footer-bottom border-top">
            <p class="text-muted">&copy; {{ currentYear }} {{ companyName }}. All rights reserved.</p>
            <div class="footer-legal">
                <router-link to="/privacy" class="footer-link">Privacy Policy</router-link>
                <span class="separator text-muted">|</span>
                <router-link to="/terms" class="footer-link">Terms of Service</router-link>
            </div>
        </div>
    </footer>
</template>

<script setup>
import { computed } from 'vue'

// Props (optional - for customization)
const props = defineProps({
    companyName: {
        type: String,
        default: 'Mango Company'
    },
    companyDescription: {
        type: String,
        default: 'Your trusted partner for quality products and services.'
    }
})

// Computed current year
const currentYear = computed(() => new Date().getFullYear())

// Quick Links
const quickLinks = [
    { name: 'Home', path: '/' },
    { name: 'About', path: '/about' },
    { name: 'Products', path: '/products' },
    { name: 'Contact', path: '/contact' }
]

// Contact Information
const contact = {
    email: 'info@mango.com',
    phone: '+1 234 567 890',
    address: '123 Mango Street, City, Country'
}

// Social Media Links
const socialMedia = [
    { name: 'Facebook', icon: '📘', url: 'https://facebook.com' },
    { name: 'Twitter', icon: '🐦', url: 'https://twitter.com' },
    { name: 'Instagram', icon: '📷', url: 'https://instagram.com' },
    { name: 'LinkedIn', icon: '💼', url: 'https://linkedin.com' }
]
</script>

<style scoped>
.footer {
    padding: 3rem 2rem 1rem;
    margin-top: auto;
}

.footer-content {
    max-width: 1200px;
    margin: 0 auto;
    display: grid;
    grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
    gap: 2rem;
    margin-bottom: 2rem;
}

.footer-section {
    display: flex;
    flex-direction: column;
}

.footer-title {
    font-size: 1.5rem;
    margin-bottom: 0.5rem;
}

.section-title {
    font-size: 1.1rem;
    margin-bottom: 1rem;
    font-weight: 600;
}

.footer-description {
    line-height: 1.6;
}

.footer-nav ul {
    list-style: none;
    padding: 0;
    margin: 0;
}

.footer-nav li {
    margin-bottom: 0.5rem;
}

.footer-link {
    color: var(--bs-secondary-color);
    text-decoration: none;
    transition: color 0.3s ease;
}

.footer-link:hover {
    color: var(--bs-success);
}

.contact-info {
    font-style: normal;
}

.contact-info p {
    margin-bottom: 0.75rem;
    display: flex;
    align-items: center;
    gap: 0.5rem;
}

.icon {
    font-size: 1.2rem;
}

.social-links {
    display: flex;
    gap: 1rem;
}

.social-link {
    font-size: 1.5rem;
    transition: transform 0.3s ease;
    text-decoration: none;
}

.social-link:hover {
    transform: scale(1.2);
}

.footer-bottom {
    max-width: 1200px;
    margin: 0 auto;
    padding-top: 2rem;
    display: flex;
    justify-content: space-between;
    align-items: center;
    flex-wrap: wrap;
    gap: 1rem;
}

.footer-bottom p {
    margin: 0;
}

.footer-legal {
    display: flex;
    gap: 1rem;
    align-items: center;
}

/* Responsive Design */
@media (max-width: 768px) {
    .footer {
        padding: 2rem 1rem 1rem;
    }

    .footer-content {
        grid-template-columns: 1fr;
        gap: 1.5rem;
    }

    .footer-bottom {
        flex-direction: column;
        text-align: center;
    }
}
</style>
