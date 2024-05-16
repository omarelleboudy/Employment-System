import React from 'react';

function RoleDropdown({ value, onChange }) {
  const handleSelectChange = (e) => {
    onChange(e.target.value);
  };

  return (
    <select value={value} onChange={handleSelectChange}>
      <option value="1">Applicant</option>
      <option value="2">Employer</option>
    </select>
  );
}

export default RoleDropdown;
