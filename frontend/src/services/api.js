import axios from 'axios';

// Create axios instance
export const api = axios.create({
  baseURL: '/api', // Use relative URL since we're using Vite proxy
  timeout: 10000,
  headers: {
    'Content-Type': 'application/json',
  },
});

// Request interceptor to add auth token
api.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem('token');
    console.log('API Request:', config.method?.toUpperCase(), config.url, 'Token exists:', !!token);
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
      console.log('Authorization header set:', config.headers.Authorization ? 'Yes' : 'No');
    } else {
      console.log('No token found in localStorage');
    }
    return config;
  },
  (error) => {
    console.error('Request interceptor error:', error);
    return Promise.reject(error);
  }
);

// Response interceptor to handle errors
api.interceptors.response.use(
  (response) => {
    console.log('API Response:', response.status, response.config.url, response.data);
    return response;
  },
  (error) => {
    console.error('API Error:', error.response?.status, error.config?.url, error.response?.data);
    if (error.response?.status === 401) {
      // Unauthorized - clear token but don't redirect automatically
      // Let the components handle the redirect
      localStorage.removeItem('token');
    }
    return Promise.reject(error);
  }
);

// API endpoints
export const authAPI = {
  login: (credentials) => api.post('/Auth/login', credentials),
  register: (userData) => api.post('/Auth/register', userData),
  getProfile: () => api.get('/Auth/profile'),
  changePassword: (passwordData) => api.post('/Auth/change-password', passwordData),
  logout: () => api.post('/Auth/logout'),
};

export const customersAPI = {
  getCustomers: (params) => api.get('/customers', { params }),
  getCustomer: (id) => api.get(`/customers/${id}`),
  createCustomer: (data) => api.post('/customers', data),
  updateCustomer: (id, data) => api.put(`/customers/${id}`, data),
  deleteCustomer: (id) => api.delete(`/customers/${id}`),
  getProvinces: () => api.get('/customers/provinces'),
};

export const productsAPI = {
  getProducts: (params) => api.get('/products', { params }),
  getProduct: (id) => api.get(`/products/${id}`),
  createProduct: (data) => api.post('/products', data),
  updateProduct: (id, data) => api.put(`/products/${id}`, data),
  deleteProduct: (id) => api.delete(`/products/${id}`),
};

export const invoicesAPI = {
  getInvoices: (params) => api.get('/invoices', { params }),
  getInvoice: (id) => api.get(`/invoices/${id}`),
  createInvoice: (data) => api.post('/invoices', data),
  updateInvoice: (id, data) => api.put(`/invoices/${id}`, data),
  deleteInvoice: (id) => api.delete(`/invoices/${id}`),
  submitToFbr: (id, fbrData) => api.post(`/invoices/${id}/submit-to-fbr`, fbrData),
  getStatuses: () => api.get('/invoices/statuses'),
  getCustomerTypes: () => api.get('/invoices/customer-types'),
  getSaleTypes: () => api.get('/invoices/sale-types'),
};

export const taxRatesAPI = {
  getAll: () => api.get('/tax-rates'),
  getById: (id) => api.get(`/tax-rates/${id}`),
  create: (data) => api.post('/tax-rates', data),
  update: (id, data) => api.put(`/tax-rates/${id}`, data),
  delete: (id) => api.delete(`/tax-rates/${id}`),
};

export const categoriesAPI = {
  getAll: () => api.get('/categories'),
  getById: (id) => api.get(`/categories/${id}`),
  create: (data) => api.post('/categories', data),
  update: (id, data) => api.put(`/categories/${id}`, data),
  delete: (id) => api.delete(`/categories/${id}`),
};

export const fbrAPI = {
  getApiLogs: (params) => api.get('/fbr/api-logs', { params }),
};

// Export all APIs as a single object for easier imports
export const apiEndpoints = {
  auth: authAPI,
  customers: customersAPI,
  products: productsAPI,
  invoices: invoicesAPI,
  taxRates: taxRatesAPI,
  categories: categoriesAPI,
  fbr: fbrAPI,
}; 