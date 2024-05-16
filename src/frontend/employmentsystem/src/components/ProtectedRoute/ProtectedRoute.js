// ProtectedRoute.js
import React from 'react';
import { Route, Redirect } from 'react-router-dom';

const ProtectedRoute = ({ component: Component, roles, ...rest }) => {
  const isLoggedIn = localStorage.getItem('isLoggedIn') === 'true';
  const userRole = localStorage.getItem('userRole');

  return (
    <Route
      {...rest}
      render={props => {
        if (!isLoggedIn) {
          return <Redirect to="/login" />;
        }

        if (!roles.includes(userRole)) {
          // If the user's role is not included in the allowed roles, redirect to home page
          return <Redirect to="/" />;
        }

        return <Component {...props} />;
      }}
    />
  );
};

export default ProtectedRoute;
