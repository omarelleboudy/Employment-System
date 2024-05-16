import React from 'react';
import './VacancyDetailsModal.css';

const VacancyDetailsModal = ({ vacancy, onClose }) => {
  if (!vacancy) return null;

  return (
    <div className="modal-overlay">
      <div className="modal-content">
        <h2>{vacancy.title}</h2>
        <p>Description: {vacancy.description}</p>
        <p>Employer: {vacancy.employerName}</p>
        <p>Max Applicants: {vacancy.maxApplicants}</p>
        <p>Current Applicants: {vacancy.currentApplicants}</p>
        <p>Created At: {new Date(vacancy.createdAt).toLocaleDateString()}</p>
        <p>Expired At: {new Date(vacancy.expiredAt).toLocaleDateString()}</p>
        <p>Status: {vacancy.isArchived ? 'Archived' : 'Active'}</p>
        <button className="button-style" onClick={onClose}>Close</button>
      </div>
    </div>
  );
};

export default VacancyDetailsModal;
