import React, { useState } from 'react';
import axios from 'axios';
import './ApplicantSearchPage.css'; // Import your CSS file
import { API_URL } from '../../../config';
import { useHistory, Link } from 'react-router-dom'; // Import useHistory and Link

const ApplicantSearchPage = () => {
  const [searchQuery, setSearchQuery] = useState('');
  const [searchResults, setSearchResults] = useState([]);
  const [error, setError] = useState(null);
  const history = useHistory(); // Initialize useHistory

  const handleSearch = async () => {
    try {
      const token = localStorage.getItem('token'); // Retrieve the token from localStorage
      const response = await axios.get(`${API_URL}/Vacancy/Search?vacancyTitle=${searchQuery}`, {
        headers: {
          'Authorization': `Bearer ${token}` // Include the token in the request headers
        }
      });
      const { payload } = response.data;
      setSearchResults(payload);
      setError(null);
    } catch (err) {
      setSearchResults([]);
      if (err.response && err.response.status === 401) {
        // Redirect to the login page if the response status is 401
        history.push('/login');
      }
    }
  };

  return (
    <div className="applicant-search-page">
      <h2>Search for Vacancies</h2>
      <div className="search-form">
        <input type="text" value={searchQuery} onChange={(e) => setSearchQuery(e.target.value)} />
        <button onClick={handleSearch}>Search</button>
      </div>
      {error && <div className="error">{error}</div>}
      {searchResults.length > 0 ? (
        <div className="search-results">
          <h3>Search Results</h3>
          <ul>
            {searchResults.map(vacancy => (
              <li key={vacancy.vacancyId}>
                <h4>{vacancy.title}</h4>
                <p>Employer Name: {vacancy.employerName}</p>

                <p>Description: {vacancy.description}</p>
                <p>Max Applicants: {vacancy.maxApplicants}</p>
                <p>Current Applicants: {vacancy.currentApplicants}</p>
                <p>Created At: {new Date(vacancy.createdAt).toLocaleDateString()}</p>
                <p>Expires At: {new Date(vacancy.expiredAt).toLocaleDateString()}</p>
                <p>Status: {vacancy.isArchived ? 'Archived' : 'Active'}</p>
                <p></p>
                <button><Link to={`/apply/${vacancy.vacancyId}`} className="apply-link">Apply</Link></button>
              </li>
            ))}
          </ul>
        </div>
      ) : (
        <p>No vacancies found.</p>
      )}
    </div>
  );
};

export default ApplicantSearchPage;
