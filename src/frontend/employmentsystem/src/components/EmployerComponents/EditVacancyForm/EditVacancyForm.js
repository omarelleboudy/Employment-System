import React, { useState, useEffect } from 'react';
import axios from 'axios';
import './EditVacancyForm.css';
import { API_URL } from '../../../config';

const EditVacancyForm = ({ match, history }) => {

  const [formData, setFormData] = useState({
    id: '',
    title: '',
    employerId: localStorage.getItem("userId"),
    description: '',
    daysAvailable: '',
    maxApplicants: ''
  });
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchVacancy = async () => {
      try {
        const token = localStorage.getItem('token');
        const response = await axios.get(`${API_URL}/Vacancy/GetVacanciesByEmployer`, {
          params: { employerId: localStorage.getItem('userId'), includeExpired: true },
          headers: {
            'Authorization': `Bearer ${token}`
          }
        });

        const vacancy = response.data.payload.find(v => v.vacancyId === match.params.id);
        setFormData({
          id: vacancy.vacancyId,
          title: vacancy.title,
          employerId: vacancy.employerId,
          description: vacancy.description,
          daysAvailable: vacancy.daysAvailable,
          maxApplicants: vacancy.maxApplicants
        });
      } catch (err) {
        setError(err.response ? err.response.data : 'Error fetching vacancy');
        if (err.response && err.response.status === 401) {
          // Redirect to the login page if the response status is 401
          history.push('/login');
        }
      }
    };

    fetchVacancy();
  }, [match.params.id]);

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
      const token = localStorage.getItem('token');

      await axios.put(`${API_URL}/Vacancy/Update`, formData, {
        headers: {
          'Authorization': `Bearer ${token}`
        }
      });

      history.push('/vacancies');
    } catch (err) {
      setError(err.response ? err.response.data : 'Error updating vacancy');
      if (err.response && err.response.status === 401) {
        // Redirect to the login page if the response status is 401
        history.push('/login');
      }
    }
  };

  return (
    <div className="edit-vacancy-form">
      <h2>Edit Vacancy</h2>
      {error && <div className="error">{error}</div>}
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
        <button type="submit">Update Vacancy</button>
      </form>
    </div>
  );
};

export default EditVacancyForm;
