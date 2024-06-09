import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';

function Login() {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const navigate = useNavigate();
  const handleLogin = async (event) => {
    event.preventDefault();
    try {
      const response = await fetch('https://localhost:7187/api/v1/Auth/login', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({ email: username, password: password }),
      });

   
      console.log(response);
      // console.log(response.json())
      const data = await response.json();
      console.log(data);
      const token = data["token"]["accessToken"];
      // const { token } = data;
      localStorage.setItem('authToken', token);
      if (data['role'] != 'Administrator') {
        console.error('Login failed:');
      } else {

        navigate('/dashboard');
      }
    } catch (error) {
      console.error('Login failed:', error);
      //alert('Login failed. Please check your credentials and try again.');
    }
  };

  return (
    <div className="flex items-center justify-center min-h-screen bg-green-50">
  <div className="w-full max-w-md p-8 space-y-8 bg-white rounded-lg shadow-md">
    <div className="flex flex-col items-center">
      <svg className="w-12 h-12 text-green-500" fill="currentColor" viewBox="0 0 20 20">
        <path
          fillRule="evenodd"
          d="M10 2a8 8 0 018 8 8.1 8.1 0 01-.94 3.83l.53 2.12a1 1 0 01-1.22 1.22l-2.12-.53A8.1 8.1 0 0110 18a8 8 0 110-16zm2.83 9.83a1 1 0 10-1.66 1.34 3 3 0 01-4.34 0 1 1 0 00-1.66 1.34 5 5 0 007.66 0 1 1 0 00-1.66-1.34zm.17-6.66A1 1 0 1110 6a1 1 0 013 0 1 1 0 01-.17.17 1 1 0 01-1.66-.34z"
          clipRule="evenodd"
        />
      </svg>
      <h2 className="mt-6 text-3xl font-extrabold text-gray-900">Welcome to Organic Store</h2>
      <p className="text-gray-600">Sign in to access the best organic products</p>
    </div>
    <form className="mt-8 space-y-6" onSubmit={handleLogin}>
      <div className="rounded-md shadow-sm -space-y-px">
        <div>
          <label htmlFor="username" className="sr-only">Username</label>
          <input
            id="username"
            name="username"
            type="text"
            required
            className="relative block w-full px-3 py-2 border border-gray-300 placeholder-gray-500 text-gray-900 rounded-t-md focus:outline-none focus:ring-green-500 focus:border-green-500 focus:z-10 sm:text-sm"
            placeholder="Username"
            value={username}
            onChange={(e) => setUsername(e.target.value)}
          />
        </div>
        <div>
          <label htmlFor="password" className="sr-only">Password</label>
          <input
            id="password"
            name="password"
            type="password"
            required
            className="relative block w-full px-3 py-2 border border-gray-300 placeholder-gray-500 text-gray-900 rounded-b-md focus:outline-none focus:ring-green-500 focus:border-green-500 focus:z-10 sm:text-sm"
            placeholder="Password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
          />
        </div>
      </div>
      <div>
        <button
          type="submit"
          className="group relative flex justify-center w-full px-4 py-2 text-sm font-medium text-white bg-green-600 border border-transparent rounded-md hover:bg-green-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-green-500"
        >
          Sign In
        </button>
      </div>
    </form>
  </div>
</div>
  );
}

export default Login;