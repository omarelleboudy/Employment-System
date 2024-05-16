import React, { useState } from 'react';
import axios from 'axios';
import { useHistory } from 'react-router-dom'; // Import useHistory hook
import './CreateVacancyForm.css';
import { API_URL } from '../../../config';

const CreateVacancyForm = () => {
  const [formData, setFormData] = useState({
    title: '',
    employerId: localStorage.getItem("userId"),
    description: '',
    daysAvailable: '',
    maxApplicants: ''
  });

  const [response, setResponse] = useState(null);
  const [error, setError] = useState(null);
  
  const history = useHistory(); // Initialize useHistory

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData({
      ...formData,
      [name]: value
    });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      const result = await axios.post(API_URL+'/Vacancy/Create', formData, {
        headers: {
          'Authorization': `Bearer ${localStorage.getItem("token")}`
        }
      });
      setResponse(result.data);
      setError(null);
    } catch (err) {
      setError(err.response ? err.response.data : 'Error creating vacancy');
      setResponse(null);
      if (err.response && err.response.status === 401) {
        // Redirect to the login page if the response status is 401
        history.push('/login');
      }
    }
  };

  return (
    <div className="create-vacancy-form">
      <h2>Create Vacancy</h2>
      <form onSubmit={handleSubmit}>
        <div>
          <label>Title:</label>
          <input type="text" name="title" value={formData.title} onChange={handleChange} required />
        </div>
        <div>
          <label>Description:</label>
          <textarea name="description" value={formData.description} onChange={handleChange} required />
        </div>
        <div>
          <label>Days Available:</label>
          <input type="number" name="daysAvailable" value={formData.daysAvailable} onChange={handleChange} required />
        </div>
        <div>
          <label>Max Applicants:</label>
          <input type="number" name="maxApplicants" value={formData.maxApplicants} onChange={handleChange} required />
        </div>
        <button type="submit">Create Vacancy</button>
      </form>

      {response && (
        <div className="response">
          <h3>Vacancy Created Successfully</h3>
        </div>
      )}

      {error && (
        <div className="error">
          <h3>Error</h3>
          <p>{error}</p>
        </div>
      )}
    </div>
  );
};

export default CreateVacancyForm;
