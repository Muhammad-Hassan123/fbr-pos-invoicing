import React from 'react';
import { Activity } from 'lucide-react';

const ApiLogs = () => {
  return (
    <div className="p-6">
      <div className="mb-6">
        <div className="flex items-center space-x-3">
          <Activity className="h-8 w-8 text-red-600" />
          <h1 className="text-2xl font-bold text-gray-900">FBR API Logs</h1>
        </div>
      </div>
      
      <div className="bg-white rounded-lg shadow p-8 text-center">
        <div className="text-gray-500 mb-4">
          <p>FBR API integration logs will be displayed here.</p>
          <p>This will show all API calls, responses, and error tracking.</p>
        </div>
      </div>
    </div>
  );
};

export default ApiLogs; 