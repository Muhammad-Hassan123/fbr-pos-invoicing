import React from 'react';
import { Lock } from 'lucide-react';

const ChangePassword = () => {
  return (
    <div className="p-6">
      <div className="mb-6">
        <div className="flex items-center space-x-3">
          <Lock className="h-8 w-8 text-indigo-600" />
          <h1 className="text-2xl font-bold text-gray-900">Change Password</h1>
        </div>
      </div>
      
      <div className="bg-white rounded-lg shadow p-8 text-center">
        <div className="text-gray-500 mb-4">
          <p>Password change form will be implemented here.</p>
          <p>This will allow users to update their account password securely.</p>
        </div>
      </div>
    </div>
  );
};

export default ChangePassword; 