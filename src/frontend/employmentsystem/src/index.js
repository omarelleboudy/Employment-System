import React from 'react';
import App from './App';
import { createRoot } from 'react-dom/client'; // Import createRoot from react-dom/client

// Ensure that the 'root' element exists in your HTML file
const rootElement = document.getElementById('root');

// Check if the root element exists before rendering the app
if (rootElement) {
  createRoot(rootElement).render(<App />); // Use createRoot from react-dom/client
} else {
  console.error("Root element with id 'root' not found in the document.");
}
