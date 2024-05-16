import React, { useState } from 'react';
import axios from 'axios';
import { useHistory, useParams } from 'react-router-dom';
import { API_URL } from '../../../config';
import './ApplyPageForm.css';

const ApplyPageForm = () => {
  const { vacancyId } = useParams();
  const history = useHistory();

  const [formData, setFormData] = useState({
    email: '',
    fullName: '',
    coverLetter: '',
    additionalInformation: '',
    resume: null
  });

  const [successMessage, setSuccessMessage] = useState('');
  const [errorMessage, setErrorMessage] = useState('');

  const handleChange = (e) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  const handleFileChange = (e) => {
    setFormData({ ...formData, resume: e.target.files[0] });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      const token = localStorage.getItem('token');
      const config = {
        headers: {
          'Authorization': `Bearer ${token}`,
          'Content-Type': 'multipart/form-data'
        }
      };

      const formDataToSend = new FormData();
      formDataToSend.append('vacancyId', vacancyId);
      formDataToSend.append('email', formData.email);
      formDataToSend.append('fullName', formData.fullName);
      formDataToSend.append('coverLetter', formData.coverLetter);
      formDataToSend.append('additionalInformation', formData.additionalInformation);
      formDataToSend.append('resume', formData.resume);

      const response = await axios.post(`${API_URL}/VacancyApplication/Apply`, formDataToSend, config);
      setSuccessMessage('Application submitted successfully');
      setErrorMessage('');
      console.log('Application submitted successfully:', response.data);
      // Redirect to a success page or display a success message

    } catch (err) {
      setErrorMessage("Failed to submit application. "+ err.response.data.errors[0].message);
      setSuccessMessage('');
      if (err.response && err.response.status === 401) {
        history.push('/login');
      }
    }
  };

  return (
    <div className="apply-page">
      <h2>Apply for Vacancy</h2>
      {successMessage && <div className="success-message">{successMessage}</div>}
      {errorMessage && <div className="error-message">{errorMessage}</div>}
      <form onSubmit={handleSubmit}>
        <div>
          <label>Email:</label>
          <input type="email" name="email" value={formData.email} onChange={handleChange} required />
        </div>
        <div>
          <label>Full Name:</label>
          <input type="text" name="fullName" value={formData.fullName} onChange={handleChange} required />
        </div>
        <div>
          <label>Cover Letter:</label>
          <textarea name="coverLetter" value={formData.coverLetter} onChange={handleChange} required />
        </div>
        <div>
          <label>Additional Information:</label>
          <textarea name="additionalInformation" value={formData.additionalInformation} onChange={handleChange} />
        </div>
        <div>
          <label>Resume:</label>
          <input type="file" name="resume" onChange={handleFileChange} required />
        </div>
        <button type="submit">Apply</button>
      </form>
    </div>
  );
};

export default ApplyPageForm;
