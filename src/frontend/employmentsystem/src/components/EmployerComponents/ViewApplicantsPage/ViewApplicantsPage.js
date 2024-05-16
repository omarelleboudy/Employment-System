import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { withRouter } from 'react-router-dom';
import './ViewApplicantsPage.css';
import { API_URL } from '../../../config';

const ViewApplicantsPage = ({ match, history }) => {
  const [applicants, setApplicants] = useState([]);
  const [error, setError] = useState(null);

  const applicationStatus = 
  {
    "1" : "Submitted",
    "2" : "Accepted",
    "3" : "Rejected",
    "4" : "Shortlisted",
    "5" : "Interviewing",
    "6" : "Hired"
  }
  useEffect(() => {
    const fetchApplicants = async () => {
      try {
        const token = localStorage.getItem('token');
        const response = await axios.get(`${API_URL}/VacancyApplication/GetVacancyApplications`, {
          params: { vacancyId: match.params.id },
          headers: {
            'Authorization': `Bearer ${token}`
          },
          withCredentials: true
        });
        setApplicants(response.data.payload);
      } catch (err) {
        if (err.response && err.response.status === 401) {
          history.push('/login');
        }
      }
    };

    fetchApplicants();
  }, [match.params.vacancyId, history]);

  const handleDownloadResume = (resumeBase64, applicantName) => {
    const byteCharacters = atob(resumeBase64);
    const byteNumbers = new Array(byteCharacters.length);
    for (let i = 0; i < byteCharacters.length; i++) {
      byteNumbers[i] = byteCharacters.charCodeAt(i);
    }
    const byteArray = new Uint8Array(byteNumbers);
    const blob = new Blob([byteArray], { type: 'application/pdf' });

    const url = URL.createObjectURL(blob);
    const link = document.createElement('a');
    link.href = url;
    link.setAttribute('download', applicantName + ' Resume.pdf');
    document.body.appendChild(link);
    link.click();

    URL.revokeObjectURL(url);
    document.body.removeChild(link);
  };

  return (
    <div className="view-applicants-page">
      <h2>Applicants for Vacancy: {match.params.vacancyId}</h2>
      {error && <div className="error">{JSON.stringify(error)}</div>}
      {applicants.length > 0 ? (
        <ul>
          {applicants.map(applicant => (
            <li key={applicant.applicationId}>
              <h3>{applicant.name}</h3>
              <p>Email: {applicant.email}</p>
              <p>Cover Letter: {applicant.coverLetter}</p>
              <p>Additional Information: {applicant.additionalInformation}</p>
              <p>Status: {applicationStatus[applicant.status]}</p>
              <p>Applied At: {new Date(applicant.createdAt).toLocaleDateString()}</p>
              <button onClick={() => handleDownloadResume(applicant.resumeAsBase64, applicant.name)}>Download Resume</button>
            </li>
          ))}
        </ul>
      ) : (
        <p>No applicants found for this vacancy.</p>
      )}
      <button onClick={() => history.goBack()}>Back</button>
    </div>
  );
};

export default withRouter(ViewApplicantsPage);
