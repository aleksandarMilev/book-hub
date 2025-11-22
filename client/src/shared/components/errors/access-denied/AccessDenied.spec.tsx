import { render, screen } from '@testing-library/react';
import { MemoryRouter, Route, Routes } from 'react-router-dom';

import { routes } from '@/shared/lib/constants/api.js';

import AccessDenied from './AccessDenied.js';

const renderWithRouteState = (state?: { message: string }) => {
  render(
    <MemoryRouter initialEntries={[{ pathname: '/forbidden', state }]}>
      <Routes>
        <Route path="/forbidden" element={<AccessDenied />} />
      </Routes>
    </MemoryRouter>,
  );
};

describe('Access Denied Component', () => {
  it('should render default message when no state provided', () => {
    renderWithRouteState();

    expect(screen.getByText(/sorry, you cannot access this resource\./i)).toBeInTheDocument();
  });

  it('should render custom message from location state', () => {
    renderWithRouteState({ message: 'Custom access denied message' });

    expect(screen.getByText(/custom access denied message/i)).toBeInTheDocument();
  });

  it('should render Go Home link pointing to home route', () => {
    renderWithRouteState();

    const link = screen.getByRole('link', { name: /go home/i });
    expect(link).toBeInTheDocument();
    expect(link).toHaveAttribute('href', routes.home);
  });
});
