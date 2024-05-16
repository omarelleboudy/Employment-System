import React, { useState, useEffect } from 'react';
import axios from 'axios';
import './ApplicantApplicationsList.css';
import { API_URL } from '../../../config';
import { useHistory } from 'react-router-dom';
import VacancyDetailsModal from './VacancyDetailsModal'; // Import the modal component

const ApplicantApplicationsList = () => {
  const [applications, setApplications] = useState([]);
  const [error, setError] = useState(null);
  const [selectedVacancy, setSelectedVacancy] = useState(null); // State to manage the selected vacancy
  const history = useHistory();

  useEffect(() => {
    const fetchApplications = async () => {
      try {
        const token = localStorage.getItem('token');
        const response = await axios.get(`${API_URL}/VacancyApplication/GetApplicantApplications`, {
          headers: {
            'Authorization': `Bearer ${token}`
          },
          withCredentials: true
        });
        setApplications(response.data.payload);
      } catch (err) {
        if (err.response && err.response.status === 401) {
          history.push('/login');
        }
      }
    };

    fetchApplications();
  }, []);

  const handleViewDetails = async (vacancyId) => {
    try {
      const token = localStorage.getItem('token');
      const response = await axios.get(`${API_URL}/Vacancy/Get`, {
        params: { vacancyId },
        headers: {
          'Authorization': `Bearer ${token}`
        },
        withCredentials: true
      });
      setSelectedVacancy(response.data.payload); // Set the selected vacancy to display in the modal
    } catch (err) {
      setError(err.response ? err.response.data : 'Error fetching vacancy details');
    }
  };

  const handleDownloadResume = (base64Data, contentType, filename) => {
    const blob = b64toBlob(base64Data, contentType);
    const url = URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url;
    a.download = filename;
    document.body.appendChild(a);
    a.click();
    window.URL.revokeObjectURL(url);
    document.body.removeChild(a);
  };

  const b64toBlob = (b64Data, contentType = '', sliceSize = 512) => {
    const byteCharacters = atob(b64Data);
    const byteArrays = [];

    for (let offset = 0; offset < byteCharacters.length; offset += sliceSize) {
      const slice = byteCharacters.slice(offset, offset + sliceSize);
      const byteNumbers = new Array(slice.length);
      for (let i = 0; i < slice.length; i++) {
        byteNumbers[i] = slice.charCodeAt(i);
      }
      const byteArray = new Uint8Array(byteNumbers);
      byteArrays.push(byteArray);
    }

    const blob = new Blob(byteArrays, { type: contentType });
    return blob;
  };

  return (
    <div className="applications-list">
      <h2>Your Applications</h2>
      {error && <div className="error">{JSON.stringify(error)}</div>}
      {applications.length > 0 ? (
        <ul>
          {applications.map(application => (
            <li key={application.applicationId}>
              <h3>{application.vacancyName}</h3>
              <p>Email: {application.email}</p>
              <p>Cover Letter: {application.coverLetter}</p>
              <p>Additional Information: {application.additionalInformation}</p>
              <p>Created At: {new Date(application.createdAt).toLocaleDateString()}</p>
              <button className="button-style" onClick={() => handleViewDetails(application.vacancyId)}>View Details</button>
              <button className="button-style" onClick={() => handleDownloadResume(application.resumeAsBase64, application.resumeContentType, `resume_${application.applicationId}`)}>Download Resume</button>
            </li>
          ))}
        </ul>
      ) : (
        <p>No applications found.</p>
      )}
      {selectedVacancy && <VacancyDetailsModal vacancy={selectedVacancy} onClose={() => setSelectedVacancy(null)} />}
    </div>
  );
};

export default ApplicantApplicationsList;
