import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { Link } from 'react-router-dom';
import './EmployerVacanciesList.css';
import { API_URL } from '../../../config';
import { useHistory } from 'react-router-dom'; // Import useHistory hook

const VacanciesList = () => {
  const [vacancies, setVacancies] = useState([]);
  const [error, setError] = useState(null);

  const history = useHistory(); // Initialize useHistory

  useEffect(() => {
    const fetchVacancies = async () => {
      try {
        const token = localStorage.getItem('token');
        const employerId = localStorage.getItem('userId');
        const response = await axios.get(`${API_URL}/Vacancy/GetVacanciesByEmployer`, {
          params: { employerId, includeExpired: true },
          headers: {
            'Authorization': `Bearer ${token}`
          },
          withCredentials: true
        });
        setVacancies(response.data.payload);
      } catch (err) {
        if(err.response.payload != null)
            setError(err.response ? err.response.data : 'Error fetching vacancies');
        if (err.response && err.response.status === 401) {
          // Redirect to the login page if the response status is 401
          history.push('/login');
        }
      }
    };

    fetchVacancies();
  }, []);

  const handleDelete = async (id) => {
    try {
      const token = localStorage.getItem('token');
      await axios.delete(`${API_URL}/Vacancy/Deactivate`, {
        params: { vacancyId: id },
        headers: {
          'Authorization': `Bearer ${token}`
        },
        withCredentials: true
      });
      setVacancies(vacancies.filter(vacancy => vacancy.vacancyId !== id));
    } catch (err) {
      setError(err.response ? err.response.data : 'Error deleting vacancy');
      if (err.response && err.response.status === 401) {
        // Redirect to the login page if the response status is 401
        history.push('/login');
      }
    }
  };

  return (
    <div className="vacancies-list">
      <h2>Your Vacancies</h2>
      {error && <div className="error">{JSON.stringify(error)}</div>}
      {vacancies.length > 0 ? (
        <ul>
          {vacancies.map(vacancy => (
            <li key={vacancy.vacancyId}>
              <h3>{vacancy.title}</h3>
              <p>{vacancy.description}</p>
              <p>Max Applicants: {vacancy.maxApplicants}</p>
              <p>Current Applicants: {vacancy.currentApplicants}</p>
              <p>Created At: {new Date(vacancy.createdAt).toLocaleDateString()}</p>
              <p>Updated At: {new Date(vacancy.updatedAt).toLocaleDateString()}</p>
              <p>Expired At: {new Date(vacancy.expiredAt).toLocaleDateString()}</p>
              <p>Status: {vacancy.isArchived ? 'Archived' : 'Active'}</p>
              <button className="button-style"><Link to={`/edit-vacancy/${vacancy.vacancyId}`} className="button-link">Edit</Link></button>
              <button className="button-style"><Link to={`/view-vacancy-applicants/${vacancy.vacancyId}`} className="button-link">View Applicants</Link></button>
              <button onClick={() => handleDelete(vacancy.vacancyId)} className="button-style delete-button">Delete</button>
            </li>
          ))}
        </ul>
      ) : (
        <p>No vacancies found.</p>
      )}
    </div>
  );
};

export default VacanciesList;
