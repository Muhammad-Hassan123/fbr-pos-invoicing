import { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { 
  Users, 
  Package, 
  FileText, 
  TrendingUp,
  Plus,
  Eye,
  Activity
} from 'lucide-react';
import { customersAPI, productsAPI, invoicesAPI } from '../../services/api';

const DashboardHome = () => {
  const [stats, setStats] = useState({
    customers: 0,
    products: 0,
    invoices: 0,
    totalSales: 0
  });
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    loadStats();
  }, []);

  const loadStats = async () => {
    try {
      const [customersRes, productsRes, invoicesRes] = await Promise.all([
        customersAPI.getCustomers({ pageSize: 1 }),
        productsAPI.getProducts({ pageSize: 1 }),
        invoicesAPI.getInvoices({ pageSize: 1 })
      ]);

      setStats({
        customers: customersRes.data.totalCount || 0,
        products: productsRes.data.totalCount || 0,
        invoices: invoicesRes.data.totalCount || 0,
        totalSales: 0 // This would be calculated from invoices
      });
    } catch (error) {
      console.error('Error loading stats:', error);
    } finally {
      setLoading(false);
    }
  };

  const quickActions = [
    {
      name: 'New Customer',
      href: '/customers',
      icon: Users,
      description: 'Add a new customer to the system',
      color: 'bg-blue-500'
    },
    {
      name: 'New Product',
      href: '/products',
      icon: Package,
      description: 'Add a new product to inventory',
      color: 'bg-green-500'
    },
    {
      name: 'New Invoice',
      href: '/invoices/create',
      icon: FileText,
      description: 'Create a new invoice',
      color: 'bg-purple-500'
    },
    {
      name: 'View Reports',
      href: '/api-logs',
      icon: Activity,
      description: 'View system reports and logs',
      color: 'bg-orange-500'
    }
  ];

  if (loading) {
    return (
      <div className="flex items-center justify-center h-64">
        <div className="animate-spin rounded-full h-8 w-8 border-b-2 border-indigo-600"></div>
      </div>
    );
  }

  return (
    <div className="space-y-6">
      {/* Welcome Section */}
      <div className="bg-white rounded-lg shadow p-6">
        <h1 className="text-2xl font-bold text-gray-900">Welcome to FBR POS System</h1>
        <p className="text-gray-600 mt-2">
          Manage your customers, products, invoices, and FBR integration all in one place.
        </p>
      </div>

      {/* Stats Cards */}
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
        <div className="bg-white rounded-lg shadow p-6">
          <div className="flex items-center">
            <div className="flex-shrink-0">
              <Users className="h-8 w-8 text-blue-600" />
            </div>
            <div className="ml-4">
              <p className="text-sm font-medium text-gray-500">Total Customers</p>
              <p className="text-2xl font-semibold text-gray-900">{stats.customers}</p>
            </div>
          </div>
        </div>

        <div className="bg-white rounded-lg shadow p-6">
          <div className="flex items-center">
            <div className="flex-shrink-0">
              <Package className="h-8 w-8 text-green-600" />
            </div>
            <div className="ml-4">
              <p className="text-sm font-medium text-gray-500">Total Products</p>
              <p className="text-2xl font-semibold text-gray-900">{stats.products}</p>
            </div>
          </div>
        </div>

        <div className="bg-white rounded-lg shadow p-6">
          <div className="flex items-center">
            <div className="flex-shrink-0">
              <FileText className="h-8 w-8 text-purple-600" />
            </div>
            <div className="ml-4">
              <p className="text-sm font-medium text-gray-500">Total Invoices</p>
              <p className="text-2xl font-semibold text-gray-900">{stats.invoices}</p>
            </div>
          </div>
        </div>

        <div className="bg-white rounded-lg shadow p-6">
          <div className="flex items-center">
            <div className="flex-shrink-0">
              <TrendingUp className="h-8 w-8 text-orange-600" />
            </div>
            <div className="ml-4">
              <p className="text-sm font-medium text-gray-500">Total Sales</p>
              <p className="text-2xl font-semibold text-gray-900">Rs. {stats.totalSales.toLocaleString()}</p>
            </div>
          </div>
        </div>
      </div>

      {/* Quick Actions */}
      <div className="bg-white rounded-lg shadow">
        <div className="px-6 py-4 border-b border-gray-200">
          <h2 className="text-lg font-medium text-gray-900">Quick Actions</h2>
        </div>
        <div className="p-6">
          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-4">
            {quickActions.map((action) => (
              <Link
                key={action.name}
                to={action.href}
                className="group relative bg-white border border-gray-200 rounded-lg p-4 hover:border-gray-300 hover:shadow-md transition-all duration-200"
              >
                <div className="flex items-center">
                  <div className={`flex-shrink-0 p-2 rounded-lg ${action.color} bg-opacity-10`}>
                    <action.icon className={`h-6 w-6 ${action.color.replace('bg-', 'text-')}`} />
                  </div>
                  <div className="ml-4">
                    <h3 className="text-sm font-medium text-gray-900 group-hover:text-indigo-600">
                      {action.name}
                    </h3>
                    <p className="text-xs text-gray-500">{action.description}</p>
                  </div>
                </div>
              </Link>
            ))}
          </div>
        </div>
      </div>

      {/* Recent Activity */}
      <div className="bg-white rounded-lg shadow">
        <div className="px-6 py-4 border-b border-gray-200">
          <h2 className="text-lg font-medium text-gray-900">Recent Activity</h2>
        </div>
        <div className="p-6">
          <div className="text-center text-gray-500 py-8">
            <Activity className="h-12 w-12 mx-auto text-gray-300 mb-4" />
            <p>No recent activity to display</p>
            <p className="text-sm">Start by creating your first invoice or adding customers</p>
          </div>
        </div>
      </div>
    </div>
  );
};

export default DashboardHome; 