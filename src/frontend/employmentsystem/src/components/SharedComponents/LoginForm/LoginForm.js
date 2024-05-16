import React, { useState } from 'react';
import axios from 'axios';
import { useHistory } from 'react-router-dom'; // Import useHistory hook
import { API_URL } from '../../../config';
import './LoginForm.css'; // Import your CSS file

function Login({ onLogin }) {
  const [formData, setFormData] = useState({
    username: '',
    password: ''
  });
  const [error, setError] = useState(null); // State for error message

  const history = useHistory(); // Initialize useHistory

  const handleChange = (e) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      const response = await axios.post(API_URL+'/Account/Login', formData);
      const { success, payload, message } = response.data;

      if (success) {
        const { token } = payload;
        const { role } = payload;
        const { name } = payload;
        const { userId } = payload;

        localStorage.setItem('token', token);
        localStorage.setItem('userId', userId);
        localStorage.setItem('userRole', role);
        localStorage.setItem('userName', name);
        localStorage.setItem('isLoggedIn', 'true'); // Store as string

        // Call the onLogin callback function passed from the parent component
        onLogin();
        // Redirect to dashboard or another route
        history.push('/'); // Redirect to home page

      } 
    } catch (error) {
      setError(error.response.data.errors[0].message); // Set error message
    }
  };

  return (
    <div className="login-form">
      <h2>Login</h2>
      <form onSubmit={handleSubmit}>
        <input type="text" name="username" placeholder="Username" value={formData.username} onChange={handleChange} />
        <input type="password" name="password" placeholder="Password" value={formData.password} onChange={handleChange} />
        {error && <div className="error">{error}</div>} {/* Display error message */}
        <button type="submit">Login</button>
      </form>
    </div>
  );
}

export default Login;
