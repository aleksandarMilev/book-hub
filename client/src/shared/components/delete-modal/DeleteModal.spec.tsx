import { fireEvent, render, screen } from '@testing-library/react';

import DeleteModal from './DeleteModal.js';

describe('Delete Modal Component', () => {
  const toggleModal = vi.fn();
  const deleteHandler = vi.fn();

  beforeEach(() => {
    vi.clearAllMocks();
  });

  it('should not show backdrop when showModal is false', () => {
    const { container } = render(
      <DeleteModal showModal={false} toggleModal={toggleModal} deleteHandler={deleteHandler} />,
    );

    expect(container.querySelector('.delete-backdrop')).toBeNull();

    const modal = container.querySelector('.delete-modal');
    expect(modal).not.toHaveClass('show');
  });

  it('should show backdrop and modal when showModal is true', () => {
    const { container } = render(
      <DeleteModal showModal toggleModal={toggleModal} deleteHandler={deleteHandler} />,
    );

    expect(container.querySelector('.delete-backdrop')).not.toBeNull();

    const modal = container.querySelector('.delete-modal');
    expect(modal).toHaveClass('show');
  });

  it('should use default title and message when none provided', () => {
    render(<DeleteModal showModal toggleModal={toggleModal} deleteHandler={deleteHandler} />);

    expect(screen.getByText(/confirm deletion/i)).toBeInTheDocument();
    expect(
      screen.getByText(
        /are you sure you want to delete this item\? this action cannot be undone\./i,
      ),
    ).toBeInTheDocument();
  });

  it('should render custom title and message when provided', () => {
    render(
      <DeleteModal
        showModal
        toggleModal={toggleModal}
        deleteHandler={deleteHandler}
        title="Delete book"
        message="Are you sure you want to delete this book?"
      />,
    );

    expect(screen.getByText(/delete book/i)).toBeInTheDocument();
    expect(screen.getByText(/are you sure you want to delete this book\?/i)).toBeInTheDocument();
  });

  it('should call toggleModal when Cancel is clicked', async () => {
    render(<DeleteModal showModal toggleModal={toggleModal} deleteHandler={deleteHandler} />);

    fireEvent.click(screen.getByRole('button', { name: /cancel/i }));

    expect(toggleModal).toHaveBeenCalledTimes(1);
    expect(deleteHandler).not.toHaveBeenCalled();
  });

  it('should call deleteHandler when Delete is clicked', async () => {
    render(<DeleteModal showModal toggleModal={toggleModal} deleteHandler={deleteHandler} />);

    fireEvent.click(screen.getByRole('button', { name: /delete/i }));

    expect(deleteHandler).toHaveBeenCalledTimes(1);
  });

  it('should call toggleModal when backdrop is clicked', async () => {
    const { container } = render(
      <DeleteModal showModal toggleModal={toggleModal} deleteHandler={deleteHandler} />,
    );

    const backdrop = container.querySelector('.delete-backdrop');
    expect(backdrop).not.toBeNull();

    fireEvent.click(backdrop!);

    expect(toggleModal).toHaveBeenCalledTimes(1);
  });
});
