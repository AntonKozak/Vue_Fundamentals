<template>
    <footer class="footer">
        <div class="footer-content">
            <!-- Company Info Section -->
            <div class="footer-section">
                <h3 class="footer-title">{{ companyName }}</h3>
                <p class="footer-description">{{ companyDescription }}</p>
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
                <address class="contact-info">
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
        <div class="footer-bottom">
            <p>&copy; {{ currentYear }} {{ companyName }}. All rights reserved.</p>
            <div class="footer-legal">
                <router-link to="/privacy" class="footer-link">Privacy Policy</router-link>
                <span class="separator">|</span>
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
    background-color: #2c3e50;
    color: #ecf0f1;
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
    color: #3498db;
}

.section-title {
    font-size: 1.1rem;
    margin-bottom: 1rem;
    color: #ecf0f1;
    font-weight: 600;
}

.footer-description {
    color: #bdc3c7;
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
    color: #bdc3c7;
    text-decoration: none;
    transition: color 0.3s ease;
}

.footer-link:hover {
    color: #3498db;
}

.contact-info {
    font-style: normal;
    color: #bdc3c7;
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
    border-top: 1px solid #34495e;
    display: flex;
    justify-content: space-between;
    align-items: center;
    flex-wrap: wrap;
    gap: 1rem;
}

.footer-bottom p {
    color: #95a5a6;
    margin: 0;
}

.footer-legal {
    display: flex;
    gap: 1rem;
    align-items: center;
}

.separator {
    color: #7f8c8d;
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
