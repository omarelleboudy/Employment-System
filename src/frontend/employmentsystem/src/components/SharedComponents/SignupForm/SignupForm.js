import React, { useState } from 'react';
import axios from 'axios';
import RoleDropdown from './RoleDropdown';
import { API_URL } from '../../../config';
import './SignupForm.css'; // Import your CSS file


function SignupForm({ history }) {
  const [formData, setFormData] = useState({
    username: '',
    name: '',
    email: '',
    password: '',
    role: '1', // Default role is 'Applicant'
  });
  const [error, setError] = useState(null); // State for error message

  const handleChange = (e) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  const handleRoleChange = (value) => {
    setFormData({ ...formData, role: value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      const response = await axios.post(API_URL+'/Account/Register', formData);
      console.log(response.data);
      // Redirect to login page
      history.push('/login');
    } catch (error) {
      setError(error.response.data.errors[0].message); // Set error message
    }
  };

  return (
    <div className="signup-form">
      <h2>Sign Up</h2>
      <form onSubmit={handleSubmit}>
        <input type="text" name="username" placeholder="Username" value={formData.username} onChange={handleChange} />
        <input type="text" name="name" placeholder="Name" value={formData.name} onChange={handleChange} />
        <input type="email" name="email" placeholder="Email" value={formData.email} onChange={handleChange} />
        <input type="password" name="password" placeholder="Password" value={formData.password} onChange={handleChange} />
        <RoleDropdown value={formData.role} onChange={handleRoleChange} />
        {error && <div className="error">{error}</div>} {/* Display error message */}
        <button type="submit">Sign Up</button>
      </form>
    </div>
  );
}

export default SignupForm;
