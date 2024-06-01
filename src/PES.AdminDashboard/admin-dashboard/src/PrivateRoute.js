import React from 'react';
import { Navigate, Outlet } from 'react-router-dom';

const PrivateRoute = () => {
  const auth = localStorage.getItem('authToken'); // Check if the user is authenticated

  return auth ? <Outlet /> : <Navigate to="/login" />;
};

export default PrivateRoute;