import { render, screen } from '@testing-library/react';
import { MemoryRouter, Route, Routes } from 'react-router-dom';

import { routes } from '@/shared/lib/constants/api.js';

import NotFound from './NotFound.js';

const renderWithRouteState = (state?: { message: string }) => {
  render(
    <MemoryRouter initialEntries={[{ pathname: '/not-found', state }]}>
      <Routes>
        <Route path="/not-found" element={<NotFound />} />
      </Routes>
    </MemoryRouter>,
  );
};

describe('Not Found Component', () => {
  it('should render default message when no state provided', () => {
    renderWithRouteState();

    expect(screen.getByText(/we couldn't find what you're looking for/i)).toBeInTheDocument();
  });

  it('should render custom message from location state', () => {
    renderWithRouteState({ message: 'Custom not found message' });

    expect(screen.getByText(/custom not found message/i)).toBeInTheDocument();
  });

  it('should render Go Home link pointing to home route', () => {
    renderWithRouteState();

    const link = screen.getByRole('link', { name: /go home/i });
    expect(link).toBeInTheDocument();
    expect(link).toHaveAttribute('href', routes.home);
  });
});
