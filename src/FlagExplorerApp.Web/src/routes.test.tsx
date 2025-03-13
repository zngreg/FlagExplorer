import React from 'react';
import { render, screen } from '@testing-library/react';
import AppRoutes from './routes';

// Mock components
jest.mock('./pages/Home', () => () => <div>Home Component</div>);
jest.mock('./pages/CountryDetails', () => () => <div>Country Details Component</div>);

describe('AppRoutes', () => {
  it('renders Home component for the "/" route', () => {
    window.history.pushState({}, 'Home Page', '/');
    
    render(<AppRoutes />);
    
    expect(screen.getByText('Home Component')).toBeInTheDocument();
  });

  it('renders CountryDetails component for the "/country/:name" route', () => {
    window.history.pushState({}, 'Country Details Page', '/country/Germany');

    render(<AppRoutes />);

    expect(screen.getByText('Country Details Component')).toBeInTheDocument();
  });

  it('does not render components for unknown routes', () => {
    window.history.pushState({}, 'Unknown Page', '/unknown');

    render(<AppRoutes />);

    expect(screen.queryByText('Home Component')).not.toBeInTheDocument();
    expect(screen.queryByText('Country Details Component')).not.toBeInTheDocument();
  });
});