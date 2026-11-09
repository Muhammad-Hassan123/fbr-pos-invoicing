import React, { useState, useEffect } from 'react';
import { Plus, Save, FolderOpen, Trash2, RotateCcw, Search } from 'lucide-react';
import toast from 'react-hot-toast';
import { taxRatesAPI } from '../../services/api';

const TaxRates = () => {
  const [taxRates, setTaxRates] = useState([]);
  const [loading, setLoading] = useState(true);
  const [searchTerm, setSearchTerm] = useState('');
  const [currentPage, setCurrentPage] = useState(1);
  const [totalPages, setTotalPages] = useState(1);
  const [selectedTaxRate, setSelectedTaxRate] = useState(null);
  const [isEditing, setIsEditing] = useState(false);

  const [formData, setFormData] = useState({
    name: '',
    percentage: ''
  });

  useEffect(() => {
    fetchTaxRates();
  }, [currentPage, searchTerm]);

  const fetchTaxRates = async () => {
    try {
      setLoading(true);
      const response = await taxRatesAPI.getAll({
        page: currentPage,
        pageSize: 10,
        searchTerm: searchTerm || undefined
      });
      setTaxRates(response.data.items);
      setTotalPages(response.data.totalPages);
    } catch (error) {
      toast.error('Failed to fetch tax rates');
      console.error('Error fetching tax rates:', error);
    } finally {
      setLoading(false);
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      if (isEditing) {
        await taxRatesAPI.update(formData.id, formData);
        toast.success('Tax rate updated successfully');
      } else {
        await taxRatesAPI.create(formData);
        toast.success('Tax rate added successfully');
      }
      resetForm();
      fetchTaxRates();
    } catch (error) {
      toast.error(isEditing ? 'Failed to update tax rate' : 'Failed to add tax rate');
      console.error('Error saving tax rate:', error);
    }
  };

  const handleEdit = (taxRate) => {
    setSelectedTaxRate(taxRate);
    setFormData({
      id: taxRate.id,
      name: taxRate.name || '',
      percentage: taxRate.percentage || ''
    });
    setIsEditing(true);
  };

  const handleDelete = async (id) => {
    if (!window.confirm('Are you sure you want to delete this tax rate?')) {
      return;
    }
    try {
      await taxRatesAPI.delete(id);
      toast.success('Tax rate deleted successfully');
      fetchTaxRates();
    } catch (error) {
      toast.error('Failed to delete tax rate');
      console.error('Error deleting tax rate:', error);
    }
  };

  const resetForm = () => {
    setFormData({
      name: '',
      percentage: ''
    });
    setSelectedTaxRate(null);
    setIsEditing(false);
  };

  const handleSearch = (e) => {
    e.preventDefault();
    setCurrentPage(1);
    fetchTaxRates();
  };

  return (
    <div className="p-6 space-y-6">
      {/* Header */}
      <div className="flex items-center justify-between">
        <h1 className="text-2xl font-bold text-gray-800">Tax Rates</h1>
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
            onClick={fetchTaxRates}
            className="p-2 bg-green-500 text-white rounded hover:bg-green-600"
          >
            <RotateCcw className="h-4 w-4" />
          </button>
        </div>
      </div>

      {/* Tax Information Form */}
      <div className="bg-blue-50 p-6 rounded-lg">
        <h2 className="text-lg font-semibold text-gray-800 mb-4">Tax Information</h2>
        <form onSubmit={handleSubmit} className="grid grid-cols-1 md:grid-cols-2 gap-4">
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">
              Tax <span className="text-red-500">*</span>
            </label>
            <input
              type="text"
              value={formData.name}
              onChange={(e) => setFormData({...formData, name: e.target.value})}
              placeholder="e.g., General Sales Tax"
              className="w-full p-2 border border-gray-300 rounded focus:ring-2 focus:ring-blue-500 focus:border-transparent"
              required
            />
          </div>
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">
              Percentage <span className="text-red-500">*</span>
            </label>
            <input
              type="number"
              step="0.01"
              value={formData.percentage}
              onChange={(e) => setFormData({...formData, percentage: e.target.value})}
              placeholder="e.g., 18"
              className="w-full p-2 border border-gray-300 rounded focus:ring-2 focus:ring-blue-500 focus:border-transparent"
              required
            />
          </div>
          <div className="md:col-span-2 flex space-x-4">
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
              placeholder="Search tax rates..."
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
            <p className="mt-2 text-gray-600">Loading tax rates...</p>
          </div>
        ) : taxRates.length === 0 ? (
          <div className="text-center py-8">
            <p className="text-gray-600">No tax rates found</p>
          </div>
        ) : (
          <>
            <div className="overflow-x-auto">
              <table className="min-w-full bg-white border border-gray-300">
                <thead>
                  <tr className="bg-gray-100">
                    <th className="px-4 py-2 text-left text-sm font-medium text-gray-700 border">Tax</th>
                    <th className="px-4 py-2 text-left text-sm font-medium text-gray-700 border">Pct</th>
                    <th className="px-4 py-2 text-left text-sm font-medium text-gray-700 border">Actions</th>
                  </tr>
                </thead>
                <tbody>
                  {taxRates.map((taxRate) => (
                    <tr key={taxRate.id} className="hover:bg-gray-50">
                      <td className="px-4 py-2 text-sm text-gray-900 border">{taxRate.name}</td>
                      <td className="px-4 py-2 text-sm text-gray-900 border">{taxRate.percentage}%</td>
                      <td className="px-4 py-2 text-sm text-gray-900 border">
                        <div className="flex space-x-2">
                          <button
                            onClick={() => handleEdit(taxRate)}
                            className="text-blue-600 hover:text-blue-800"
                          >
                            Edit
                          </button>
                          <button
                            onClick={() => handleDelete(taxRate.id)}
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

export default TaxRates; 