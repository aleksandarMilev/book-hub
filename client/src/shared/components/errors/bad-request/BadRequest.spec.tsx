import { render, screen } from '@testing-library/react';
import { MemoryRouter, Route, Routes } from 'react-router-dom';

import { routes } from '@/shared/lib/constants/api.js';

import BadRequest from './BadRequest.js';

const renderWithRouteState = (state?: { message: string }) => {
  render(
    <MemoryRouter initialEntries={[{ pathname: '/bad-request', state }]}>
      <Routes>
        <Route path="/bad-request" element={<BadRequest />} />
      </Routes>
    </MemoryRouter>,
  );
};

describe('Bad Request Component', () => {
  it('should render default message when no state provided', () => {
    renderWithRouteState();

    expect(
      screen.getByText(/something went wrong with your request\. please try again\./i),
    ).toBeInTheDocument();
  });

  it('should render custom message from location state', () => {
    renderWithRouteState({ message: 'Custom bad request message' });

    expect(screen.getByText(/custom bad request message/i)).toBeInTheDocument();
  });

  it('should render Go Home link pointing to home route', () => {
    renderWithRouteState();

    const link = screen.getByRole('link', { name: /go home/i });
    expect(link).toBeInTheDocument();
    expect(link).toHaveAttribute('href', routes.home);
  });
});
