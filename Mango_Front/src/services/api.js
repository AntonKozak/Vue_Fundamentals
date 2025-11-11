import axios from 'axios';
import { CONFIG_API_URL } from '../constant/config.js';

const api = axios.create({
    baseURL: CONFIG_API_URL,
    headers: { 'Content-Type': 'application/json' }
});


export default api;
