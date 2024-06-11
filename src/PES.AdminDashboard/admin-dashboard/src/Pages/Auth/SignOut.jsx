import { useNavigate } from 'react-router-dom';

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
    <button onClick={handleSignOut}>
      Sign Out
    </button>
  );
};

export default SignOut;