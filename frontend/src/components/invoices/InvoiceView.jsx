import React from 'react';
import { ArrowLeft } from 'lucide-react';
import { Link } from 'react-router-dom';

const InvoiceView = () => {
  return (
    <div className="p-6">
      <div className="mb-6">
        <Link to="/invoices" className="flex items-center text-blue-600 hover:text-blue-700 mb-4">
          <ArrowLeft className="h-4 w-4 mr-2" />
          Back to Invoices
        </Link>
        <h1 className="text-2xl font-bold text-gray-900">Invoice Details</h1>
      </div>
      
      <div className="bg-white rounded-lg shadow p-8 text-center">
        <div className="text-gray-500 mb-4">
          <p>Invoice details and print view will be implemented here.</p>
          <p>This will show complete invoice information and FBR integration status.</p>
        </div>
        <Link
          to="/invoices"
          className="bg-blue-600 text-white px-4 py-2 rounded-lg hover:bg-blue-700"
        >
          Back to Invoices
        </Link>
      </div>
    </div>
  );
};

export default InvoiceView; 