import { fireEvent, render, screen } from '@testing-library/react';
import type { Mock } from 'vitest';

import Pagination from './Pagination.js';

describe('Pagination Component', () => {
  const onPageChange = vi.fn();

  beforeEach(() => {
    vi.clearAllMocks();
  });

  it('should return null when totalPages is 1 or less', () => {
    const { container } = render(
      <Pagination page={1} totalPages={1} onPageChange={onPageChange} />,
    );

    expect(container.firstChild).toBeNull();
  });

  it('should disable Previous on first page and enables Next', () => {
    render(<Pagination page={1} totalPages={5} onPageChange={onPageChange} />);

    const prevButton = screen.getByRole('button', { name: /previous page/i });
    const nextButton = screen.getByRole('button', { name: /next page/i });

    expect(prevButton).toBeDisabled();
    expect(nextButton).not.toBeDisabled();
  });

  it('should disable Next on last page and enables Previous', () => {
    render(<Pagination page={5} totalPages={5} onPageChange={onPageChange} />);

    const prevButton = screen.getByRole('button', { name: /previous page/i });
    const nextButton = screen.getByRole('button', { name: /next page/i });

    expect(prevButton).not.toBeDisabled();
    expect(nextButton).toBeDisabled();
  });

  it('should disable both buttons when disabled prop is true', () => {
    render(<Pagination page={3} totalPages={5} disabled onPageChange={onPageChange} />);

    const prevButton = screen.getByRole('button', { name: /previous page/i });
    const nextButton = screen.getByRole('button', { name: /next page/i });

    expect(prevButton).toBeDisabled();
    expect(nextButton).toBeDisabled();
  });

  it('should call onPageChange with previous page when Previous is clicked', async () => {
    render(<Pagination page={3} totalPages={5} onPageChange={onPageChange} />);

    const prevButton = screen.getByRole('button', { name: /previous page/i });
    fireEvent.click(prevButton);

    expect(onPageChange).toHaveBeenCalledWith(2);
  });

  it('should call onPageChange with next page when Next is clicked', async () => {
    render(<Pagination page={3} totalPages={5} onPageChange={onPageChange} />);

    const nextButton = screen.getByRole('button', { name: /next page/i });
    fireEvent.click(nextButton);

    expect(onPageChange).toHaveBeenCalledWith(4);
  });

  it('should scroll to top when page changes', () => {
    const scrollToMock = window.scrollTo as unknown as Mock;

    const { rerender } = render(<Pagination page={1} totalPages={5} onPageChange={onPageChange} />);

    expect(scrollToMock).toHaveBeenCalledTimes(1);

    rerender(<Pagination page={2} totalPages={5} onPageChange={onPageChange} />);

    expect(scrollToMock).toHaveBeenCalledTimes(2);
    expect(scrollToMock).toHaveBeenLastCalledWith({ top: 0, behavior: 'smooth' });
  });
});
