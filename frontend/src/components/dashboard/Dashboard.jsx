import React, { useState } from 'react';
import { Link, Outlet, useLocation, useNavigate } from 'react-router-dom';
import { useAuth } from '../../contexts/AuthContext';
import { 
  Users, 
  Package, 
  FileText, 
  Percent,
  BarChart3,
  Settings,
  ChevronDown,
  ChevronRight,
  LogOut,
  User
} from 'lucide-react';

const Dashboard = () => {
  const { user, logout } = useAuth();
  const location = useLocation();
  const navigate = useNavigate();
  const [expandedSections, setExpandedSections] = useState({
    systemManagement: true,
    transactions: true,
    dashboards: true,
    utilities: true
  });

  const toggleSection = (section) => {
    setExpandedSections(prev => ({
      ...prev,
      [section]: !prev[section]
    }));
  };

  const handleLogout = () => {
    logout();
    navigate('/login');
  };

  const menuItems = [
    {
      section: 'systemManagement',
      title: 'System Management',
      items: [
        { name: 'Customer', path: '/customers', icon: Users },
        { name: 'Product', path: '/products', icon: Package },
        { name: 'Tax Rates', path: '/tax-rates', icon: Percent }
      ]
    },
    {
      section: 'transactions',
      title: 'Transactions',
      items: [
        { name: 'Invoice', path: '/invoices', icon: FileText },
        { name: 'Debet Note', path: '/debit-notes', icon: FileText },
        { name: 'Payments', path: '/payments', icon: FileText }
      ]
    },
    {
      section: 'dashboards',
      title: 'Dashboards',
      items: [
        { name: 'Invoice List', path: '/invoice-list', icon: BarChart3 },
        { name: 'Debet Not List', path: '/debit-note-list', icon: BarChart3 },
        { name: 'Payment List', path: '/payment-list', icon: BarChart3 },
        { name: 'Api Log', path: '/api-logs', icon: BarChart3 }
      ]
    },
    {
      section: 'utilities',
      title: 'Utilities',
      items: [
        { name: 'Change Password', path: '/change-password', icon: Settings }
      ]
    }
  ];

  const isActive = (path) => {
    return location.pathname === path || location.pathname.startsWith(path + '/');
  };

  return (
    <div className="flex h-screen bg-gray-100">
      {/* Left Sidebar */}
      <div className="w-64 bg-white shadow-lg relative">
        {/* Header */}
        <div className="p-4 border-b border-gray-200">
          <h1 className="text-xl font-bold text-gray-800">Menu</h1>
        </div>

        {/* Menu Items */}
        <div className="p-2 pb-24"> {/* Add bottom padding so menu items don't overlap logout */}
          {menuItems.map((section) => (
            <div key={section.section} className="mb-2">
              <button
                onClick={() => toggleSection(section.section)}
                className="w-full flex items-center justify-between p-2 text-left text-gray-700 hover:bg-gray-100 rounded"
              >
                <span className="font-medium">{section.title}</span>
                {expandedSections[section.section] ? (
                  <ChevronDown className="h-4 w-4" />
                ) : (
                  <ChevronRight className="h-4 w-4" />
                )}
              </button>
              
              {expandedSections[section.section] && (
                <div className="ml-4 space-y-1">
                  {section.items.map((item) => {
                    const Icon = item.icon;
                    return (
                      <Link
                        key={item.path}
                        to={item.path}
                        className={`flex items-center p-2 rounded text-sm transition-colors ${
                          isActive(item.path)
                            ? 'bg-blue-100 text-blue-700 font-medium'
                            : 'text-gray-600 hover:bg-gray-50'
                        }`}
                      >
                        <Icon className="h-4 w-4 mr-2" />
                        {item.name}
                      </Link>
                    );
                  })}
                </div>
              )}
            </div>
          ))}
        </div>

        {/* User Info and Logout */}
        <div className="absolute bottom-0 left-0 right-0 p-4 border-t border-gray-200 bg-white">
          <div className="flex items-center mb-3">
            <User className="h-5 w-5 text-gray-500 mr-2" />
            <span className="text-sm text-gray-700">{user?.firstName} {user?.lastName}</span>
          </div>
          <button
            onClick={handleLogout}
            className="w-full flex items-center justify-center p-2 text-red-600 hover:bg-red-50 rounded text-sm"
          >
            <LogOut className="h-4 w-4 mr-2" />
            Logout
          </button>
        </div>
      </div>

      {/* Main Content Area */}
      <div className="flex-1 flex flex-col overflow-hidden">
        {/* Top Bar */}
        <div className="bg-white shadow-sm border-b border-gray-200 px-6 py-3">
          <div className="flex items-center justify-between">
            <div className="flex items-center space-x-2">
              <span className="text-gray-500">Customer</span>
              <span className="text-gray-400">{' > '}</span>
              <span className="font-medium text-gray-800">
                {location.pathname === '/' && 'Dashboard'}
                {location.pathname === '/customers' && 'Customer'}
                {location.pathname === '/products' && 'Product'}
                {location.pathname === '/tax-rates' && 'Tax Rates'}
                {location.pathname === '/invoices' && 'Invoice'}
                {location.pathname === '/api-logs' && 'Api Log'}
                {location.pathname === '/change-password' && 'Change Password'}
              </span>
            </div>
          </div>
        </div>

        {/* Content */}
        <div className="flex-1 overflow-auto bg-gray-50">
          <Outlet />
        </div>
      </div>
    </div>
  );
};

export default Dashboard; 