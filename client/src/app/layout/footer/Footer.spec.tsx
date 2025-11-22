import { render, screen } from '@testing-library/react';
import { MemoryRouter } from 'react-router-dom';

import Footer from '@/app/layout/footer/Footer.js';

const renderFooter = () => {
  render(
    <MemoryRouter>
      <Footer />
    </MemoryRouter>,
  );
};

describe('Footer Component', () => {
  beforeEach(() => {
    renderFooter();
  });

  it('should render the heading', () => {
    expect(screen.getByText(/get connected with us/i)).toBeInTheDocument();
  });

  it('should render the description', () => {
    expect(screen.getByText(/discover books, authors, articles/i)).toBeInTheDocument();
  });

  it('should render the application links', () => {
    const links = [
      'home',
      'books',
      'authors',
      'articles',
      'chats',
      'my profile',
      'top users',
      'register',
      'login',
    ] as const;

    for (const link of links) {
      expect(screen.getByRole('link', { name: new RegExp(link, 'i') })).toBeInTheDocument();
    }
  });

  it('should render the newsletter form', () => {
    expect(screen.getByPlaceholderText(/your email/i)).toBeInTheDocument();
    expect(screen.getByRole('button', { name: /subscribe/i })).toBeInTheDocument();
  });

  it('should render the link to GitHub', () => {
    const githubLink = screen.getByRole('link', { name: /open source project/i });

    expect(githubLink).toHaveAttribute('href', 'https://github.com/aleksandarMilev/book-hub');
    expect(githubLink).toHaveAttribute('target', '_blank');
    expect(githubLink).toHaveAttribute('rel', expect.stringContaining('noopener'));
  });

  it('should render the current year', () => {
    expect(screen.getByText(new RegExp(String(new Date().getFullYear()), 'i'))).toBeInTheDocument();
  });
});
