import { useNavigate } from 'react-router-dom';
import { Button } from 'antd';
import { LogoutOutlined } from '@ant-design/icons';

const SignOut = () => {
  const navigate = useNavigate();

  const handleSignOut = () => {
    // Clear the user's session data from local storage
    localStorage.removeItem('authToken');
    localStorage.removeItem('userId');
    // ... remove any other user-specific data from local storage

    // Navigate to the sign-in page
    navigate('/');
  };


  return (
    <div className="min-h-screen flex flex-col items-center justify-center bg-green-50 p-4">
    <div className="bg-white shadow-md rounded-lg p-10 text-center">
      <h1 className="text-3xl font-bold text-primary mb-4">Admin Logout</h1>
      <p className="text-lg text-gray-700 mb-6">
        You have been successfully logged out from Organic Veggies & Fruits Foods admin panel.
      </p>
      <Button 
        type="primary" 
        icon={<LogoutOutlined />} 
        size="large" 
        onClick={handleSignOut}
        className="bg-primary hover:bg-secondary text-white"
      >
        Sign Out
      </Button>
    </div>
  </div>
  );
};

export default SignOut;