import React, { useState, useEffect } from 'react';
import { Plus, Save, FolderOpen, Trash2, RotateCcw, Search } from 'lucide-react';
import toast from 'react-hot-toast';
import { customersAPI } from '../../services/api';

const Customers = () => {
  const [customers, setCustomers] = useState([]);
  const [loading, setLoading] = useState(true);
  const [searchTerm, setSearchTerm] = useState('');
  const [currentPage, setCurrentPage] = useState(1);
  const [totalPages, setTotalPages] = useState(1);
  const [selectedCustomer, setSelectedCustomer] = useState(null);
  const [isEditing, setIsEditing] = useState(false);

  const [formData, setFormData] = useState({
    province: '',
    customerName: '',
    phone: '',
    email: '',
    address: '',
    scenario: '',
    ntn: '',
    registrationNumber: '',
    taxId: ''
  });

  const provinces = [
    'Punjab', 'Sindh', 'Khyber Pakhtunkhwa', 'Balochistan', 
    'Islamabad Capital Territory', 'Gilgit-Baltistan', 'Azad Kashmir'
  ];

  const scenarios = [
    '-- Select --',
    'Registered',
    'Unregistered',
    'Export',
    'Import'
  ];

  useEffect(() => {
    fetchCustomers();
  }, [currentPage, searchTerm]);

  const fetchCustomers = async () => {
    try {
      setLoading(true);
      const response = await customersAPI.getCustomers({
        page: currentPage,
        pageSize: 10,
        searchTerm: searchTerm || undefined
      });
      setCustomers(response.data.items);
      setTotalPages(response.data.totalPages);
    } catch (error) {
      toast.error('Failed to fetch customers');
      console.error('Error fetching customers:', error);
    } finally {
      setLoading(false);
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      if (isEditing) {
        await customersAPI.updateCustomer(selectedCustomer.id, formData);
        toast.success('Customer updated successfully');
      } else {
        await customersAPI.createCustomer(formData);
        toast.success('Customer added successfully');
      }
      resetForm();
      fetchCustomers();
    } catch (error) {
      toast.error(isEditing ? 'Failed to update customer' : 'Failed to add customer');
      console.error('Error saving customer:', error);
    }
  };

  const handleEdit = (customer) => {
    setSelectedCustomer(customer);
    setFormData({
      province: customer.province || '',
      customerName: customer.customerName || '',
      phone: customer.phone || '',
      email: customer.email || '',
      address: customer.address || '',
      scenario: customer.scenario || '',
      ntn: customer.ntn || '',
      registrationNumber: customer.registrationNumber || '',
      taxId: customer.taxId || ''
    });
    setIsEditing(true);
  };

  const handleDelete = async (id) => {
    if (!window.confirm('Are you sure you want to delete this customer?')) {
      return;
    }

    try {
      await customersAPI.deleteCustomer(id);
      toast.success('Customer deleted successfully');
      fetchCustomers();
    } catch (error) {
      toast.error('Failed to delete customer');
      console.error('Error deleting customer:', error);
    }
  };

  const resetForm = () => {
    setFormData({
      province: '',
      customerName: '',
      phone: '',
      email: '',
      address: '',
      scenario: '',
      ntn: '',
      registrationNumber: '',
      taxId: ''
    });
    setSelectedCustomer(null);
    setIsEditing(false);
  };

  const handleSearch = (e) => {
    e.preventDefault();
    setCurrentPage(1);
    fetchCustomers();
  };

  return (
    <div className="p-6 space-y-6">
      {/* Header */}
      <div className="flex items-center justify-between">
        <h1 className="text-2xl font-bold text-gray-800">Customer</h1>
        <div className="flex space-x-2">
          <button className="p-2 bg-green-500 text-white rounded hover:bg-green-600">
            <Plus className="h-4 w-4" />
          </button>
          <button className="p-2 bg-blue-500 text-white rounded hover:bg-blue-600">
            <Save className="h-4 w-4" />
          </button>
          <button className="p-2 bg-gray-500 text-white rounded hover:bg-gray-600">
            <FolderOpen className="h-4 w-4" />
          </button>
          <button className="p-2 bg-red-500 text-white rounded hover:bg-red-600">
            <Trash2 className="h-4 w-4" />
          </button>
          <button 
            onClick={fetchCustomers}
            className="p-2 bg-green-500 text-white rounded hover:bg-green-600"
          >
                            <RotateCcw className="h-4 w-4" />
          </button>
        </div>
      </div>

      {/* Customer Information Form */}
      <div className="bg-blue-50 p-6 rounded-lg">
        <h2 className="text-lg font-semibold text-gray-800 mb-4">Customer Information</h2>
        <form onSubmit={handleSubmit} className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">
              Province <span className="text-red-500">*</span>
            </label>
            <select
              value={formData.province}
              onChange={(e) => setFormData({...formData, province: e.target.value})}
              className="w-full p-2 border border-gray-300 rounded focus:ring-2 focus:ring-blue-500 focus:border-transparent"
              required
            >
              <option value="">-- Select Province --</option>
              {provinces.map(province => (
                <option key={province} value={province}>{province}</option>
              ))}
            </select>
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">
              Customer Name <span className="text-red-500">*</span>
            </label>
            <input
              type="text"
              value={formData.customerName}
              onChange={(e) => setFormData({...formData, customerName: e.target.value})}
              className="w-full p-2 border border-gray-300 rounded focus:ring-2 focus:ring-blue-500 focus:border-transparent"
              required
            />
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">
              Phone <span className="text-red-500">*</span>
            </label>
            <input
              type="tel"
              value={formData.phone}
              onChange={(e) => setFormData({...formData, phone: e.target.value})}
              className="w-full p-2 border border-gray-300 rounded focus:ring-2 focus:ring-blue-500 focus:border-transparent"
              required
            />
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">
              Email <span className="text-red-500">*</span>
            </label>
            <input
              type="email"
              value={formData.email}
              onChange={(e) => setFormData({...formData, email: e.target.value})}
              className="w-full p-2 border border-gray-300 rounded focus:ring-2 focus:ring-blue-500 focus:border-transparent"
              required
            />
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">
              Address <span className="text-red-500">*</span>
            </label>
            <input
              type="text"
              value={formData.address}
              onChange={(e) => setFormData({...formData, address: e.target.value})}
              className="w-full p-2 border border-gray-300 rounded focus:ring-2 focus:ring-blue-500 focus:border-transparent"
              required
            />
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">
              Scenario <span className="text-red-500">*</span>
            </label>
            <select
              value={formData.scenario}
              onChange={(e) => setFormData({...formData, scenario: e.target.value})}
              className="w-full p-2 border border-gray-300 rounded focus:ring-2 focus:ring-blue-500 focus:border-transparent"
              required
            >
              {scenarios.map(scenario => (
                <option key={scenario} value={scenario}>{scenario}</option>
              ))}
            </select>
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">N.T.N</label>
            <input
              type="text"
              value={formData.ntn}
              onChange={(e) => setFormData({...formData, ntn: e.target.value})}
              className="w-full p-2 border border-gray-300 rounded focus:ring-2 focus:ring-blue-500 focus:border-transparent"
            />
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">Reg#</label>
            <input
              type="text"
              value={formData.registrationNumber}
              onChange={(e) => setFormData({...formData, registrationNumber: e.target.value})}
              className="w-full p-2 border border-gray-300 rounded focus:ring-2 focus:ring-blue-500 focus:border-transparent"
            />
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">Tax Id#</label>
            <input
              type="text"
              value={formData.taxId}
              onChange={(e) => setFormData({...formData, taxId: e.target.value})}
              className="w-full p-2 border border-gray-300 rounded focus:ring-2 focus:ring-blue-500 focus:border-transparent"
            />
          </div>

          <div className="md:col-span-2 lg:col-span-3 flex space-x-4">
            <button
              type="submit"
              className="px-4 py-2 bg-blue-600 text-white rounded hover:bg-blue-700"
            >
              {isEditing ? 'Save' : 'Add'}
            </button>
            <button
              type="button"
              onClick={resetForm}
              className="px-4 py-2 bg-gray-500 text-white rounded hover:bg-gray-600"
            >
              Cancel
            </button>
          </div>
        </form>
      </div>

      {/* Search */}
      <div className="bg-blue-50 p-4 rounded-lg">
        <form onSubmit={handleSearch} className="flex space-x-4">
          <div className="flex-1">
            <label className="block text-sm font-medium text-gray-700 mb-1">Search</label>
            <input
              type="text"
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
              placeholder="Search customers..."
              className="w-full p-2 border border-gray-300 rounded focus:ring-2 focus:ring-blue-500 focus:border-transparent"
            />
          </div>
          <div className="flex items-end">
            <button
              type="submit"
              className="px-4 py-2 bg-blue-600 text-white rounded hover:bg-blue-700 flex items-center"
            >
              <Search className="h-4 w-4 mr-2" />
              Search
            </button>
          </div>
        </form>
      </div>

      {/* Detail Table */}
      <div className="bg-blue-50 p-6 rounded-lg">
        <h2 className="text-lg font-semibold text-gray-800 mb-4">Detail</h2>
        {loading ? (
          <div className="text-center py-8">
            <div className="animate-spin rounded-full h-8 w-8 border-b-2 border-blue-600 mx-auto"></div>
            <p className="mt-2 text-gray-600">Loading customers...</p>
          </div>
        ) : customers.length === 0 ? (
          <div className="text-center py-8">
            <p className="text-gray-600">No customers found</p>
          </div>
        ) : (
          <>
            <div className="overflow-x-auto">
              <table className="min-w-full bg-white border border-gray-300">
                <thead>
                  <tr className="bg-gray-100">
                    <th className="px-4 py-2 text-left text-sm font-medium text-gray-700 border">Customer Name</th>
                    <th className="px-4 py-2 text-left text-sm font-medium text-gray-700 border">Email</th>
                    <th className="px-4 py-2 text-left text-sm font-medium text-gray-700 border">Phone</th>
                    <th className="px-4 py-2 text-left text-sm font-medium text-gray-700 border">Address</th>
                    <th className="px-4 py-2 text-left text-sm font-medium text-gray-700 border">Tax Reg No.</th>
                    <th className="px-4 py-2 text-left text-sm font-medium text-gray-700 border">Province</th>
                    <th className="px-4 py-2 text-left text-sm font-medium text-gray-700 border">Reg#</th>
                    <th className="px-4 py-2 text-left text-sm font-medium text-gray-700 border">NTN</th>
                    <th className="px-4 py-2 text-left text-sm font-medium text-gray-700 border">Scenario</th>
                    <th className="px-4 py-2 text-left text-sm font-medium text-gray-700 border">Desc</th>
                    <th className="px-4 py-2 text-left text-sm font-medium text-gray-700 border">Actions</th>
                  </tr>
                </thead>
                <tbody>
                  {customers.map((customer) => (
                    <tr key={customer.id} className="hover:bg-gray-50">
                      <td className="px-4 py-2 text-sm text-gray-900 border">{customer.customerName}</td>
                      <td className="px-4 py-2 text-sm text-gray-900 border">{customer.email}</td>
                      <td className="px-4 py-2 text-sm text-gray-900 border">{customer.phone}</td>
                      <td className="px-4 py-2 text-sm text-gray-900 border">{customer.address}</td>
                      <td className="px-4 py-2 text-sm text-gray-900 border">{customer.taxId || '-'}</td>
                      <td className="px-4 py-2 text-sm text-gray-900 border">{customer.province}</td>
                      <td className="px-4 py-2 text-sm text-gray-900 border">{customer.registrationNumber || '-'}</td>
                      <td className="px-4 py-2 text-sm text-gray-900 border">{customer.ntn || '-'}</td>
                      <td className="px-4 py-2 text-sm text-gray-900 border">{customer.scenario}</td>
                      <td className="px-4 py-2 text-sm text-gray-900 border">{customer.description || '-'}</td>
                      <td className="px-4 py-2 text-sm text-gray-900 border">
                        <div className="flex space-x-2">
                          <button
                            onClick={() => handleEdit(customer)}
                            className="text-blue-600 hover:text-blue-800"
                          >
                            Edit
                          </button>
                          <button
                            onClick={() => handleDelete(customer.id)}
                            className="text-red-600 hover:text-red-800"
                          >
                            Delete
                          </button>
                        </div>
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
            {/* Pagination Controls */}
            <div className="flex justify-center items-center mt-4 space-x-2">
              <button
                onClick={() => setCurrentPage((p) => Math.max(1, p - 1))}
                disabled={currentPage === 1}
                className={`px-3 py-1 rounded ${currentPage === 1 ? 'bg-gray-200 text-gray-400' : 'bg-blue-500 text-white hover:bg-blue-600'}`}
              >
                Previous
              </button>
              {Array.from({ length: totalPages }, (_, i) => i + 1).map((page) => (
                <button
                  key={page}
                  onClick={() => setCurrentPage(page)}
                  className={`px-3 py-1 rounded ${currentPage === page ? 'bg-blue-700 text-white' : 'bg-gray-200 text-gray-700 hover:bg-blue-100'}`}
                >
                  {page}
                </button>
              ))}
              <button
                onClick={() => setCurrentPage((p) => Math.min(totalPages, p + 1))}
                disabled={currentPage === totalPages}
                className={`px-3 py-1 rounded ${currentPage === totalPages ? 'bg-gray-200 text-gray-400' : 'bg-blue-500 text-white hover:bg-blue-600'}`}
              >
                Next
              </button>
            </div>
          </>
        )}
      </div>
    </div>
  );
};

export default Customers; 