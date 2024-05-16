import React, { useState, useEffect } from 'react';
import { BrowserRouter as Router, Route, Switch, Link, Redirect } from 'react-router-dom';
import './App.css';
import SignupForm from './components/SharedComponents/SignupForm/SignupForm';
import LoginForm from './components/SharedComponents/LoginForm/LoginForm';
import CreateVacancyForm from './components/EmployerComponents/CreateVacancyForm/CreateVacancyForm';
import EditVacancyForm from './components/EmployerComponents/EditVacancyForm/EditVacancyForm';
import VacanciesList from './components/EmployerComponents/EmployerVacanciesList/EmployerVacanciesList';
import ViewApplicants from './components/EmployerComponents/ViewApplicantsPage/ViewApplicantsPage'; 
import SearchVacancies from './components/ApplicantComponents/ApplicantSearchPage/ApplicantSearchPage'; 
import ApplyPageForm from './components/ApplicantComponents/ApplyPageForm/ApplyPageForm'
import ApplicantApplicationsList from './components/ApplicantComponents/ApplicantApplicationsList/ApplicantApplicationsList';

import ProtectedRoute from './components/ProtectedRoute/ProtectedRoute';

function App() {
  const [isLoggedIn, setIsLoggedIn] = useState(false);
  const [userRole, setUserRole] = useState('');

  useEffect(() => {
    const loggedInStatus = localStorage.getItem('isLoggedIn') === 'true';
    setIsLoggedIn(loggedInStatus);
  }, []);

  useEffect(() => {
    setUserRole(localStorage.getItem('userRole') || '');
  }, [isLoggedIn]); // Update userRole when isLoggedIn changes

  const handleLogout = () => {
    localStorage.clear();
    setIsLoggedIn(false);
  };
  return (
    <Router>
      <div className="App">
        <nav className="navbar">
          <div className="container">
            <div className="navbar-brand">
            <button className="button-style"><Link to="/" className="button-link">Home</Link></button>

                          {isLoggedIn && userRole === 'Employer' && (
                <>
                  <button className="button-style"><Link to="/create-vacancy" className="button-link">Create Vacancy</Link></button>
                  <button className="button-style"><Link to="/vacancies" className="button-link">My Vacancies</Link></button>
                </>
              )}
              {isLoggedIn && userRole === 'Applicant' && (
                <>
                  <button className="button-style"><Link to="/search-vacancies" className="button-link">Search Vacancies</Link></button>
                  <button className="button-style"><Link to="/view-my-applications" className="button-link">My Applications</Link></button>
                </>
                
              )}
            </div>
            <div className="navbar-links">
              <ul>
                {isLoggedIn ? (
                  <>
                    <li>Welcome, {localStorage.getItem('userName')}!</li>
                    <li><button className="button-style" onClick={handleLogout}>Logout</button></li>
                  </>
                ) : (
                  <>
                    <li><button className="button-style"><Link to="/signup" className="button-link">Sign Up</Link></button></li>
                    <li><button className="button-style"><Link to="/login" className="button-link">Login</Link></button></li>
                  </>
                )}
              </ul>
            </div>
          </div>
        </nav>
        <div className="content">
          <Switch>
            <Route path="/signup" component={SignupForm} />
            <Route path="/login">
              <LoginForm onLogin={() => setIsLoggedIn(true)} />
            </Route>
            <ProtectedRoute path="/create-vacancy" component={CreateVacancyForm} roles={['Employer']} />
            <ProtectedRoute path="/vacancies" component={VacanciesList} roles={['Employer']} />
            <ProtectedRoute path="/edit-vacancy/:id" component={EditVacancyForm} roles={['Employer']} />
            <ProtectedRoute path="/view-vacancy-applicants/:id" component={ViewApplicants} roles={['Employer']} />

            <ProtectedRoute path="/search-vacancies" component={SearchVacancies} roles={['Applicant']} />
            <ProtectedRoute path="/view-my-applications" component={ApplicantApplicationsList} roles={['Applicant']} />
            <ProtectedRoute path="/apply/:vacancyId" component={ApplyPageForm} roles={['Applicant']} />

            <Route path="/" exact>
              <h1>Welcome to the Job Portal</h1>
            </Route>
            <Route path="*">
              <Redirect to="/" />
            </Route>
          </Switch>
        </div>
      </div>
    </Router>
  );
}

export default App;
