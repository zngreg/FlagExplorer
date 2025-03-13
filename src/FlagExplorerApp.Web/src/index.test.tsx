// Mock ReactDOM.createRoot at the top level
jest.mock('react-dom/client', () => ({
  createRoot: jest.fn(),
}));

import React from 'react';
import ReactDOM from 'react-dom/client';
import App from './App';
import './index.css';

describe('index.tsx', () => {
  let renderMock: jest.Mock;

  beforeEach(() => {
    renderMock = jest.fn();
    (ReactDOM.createRoot as jest.Mock).mockReturnValue({
      render: renderMock,
    });
  });

  it('renders App component without crashing', () => {
    // Set up the DOM element for React to render into
    document.body.innerHTML = '<div id="root"></div>';

    // Require the index file (this executes the render logic)
    require('./index');

    // Assertions
    expect(ReactDOM.createRoot).toHaveBeenCalledWith(document.getElementById('root'));
    expect(renderMock).toHaveBeenCalledWith(
      <React.StrictMode>
        <App />
      </React.StrictMode>
    );
  });
});