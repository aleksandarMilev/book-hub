import type React from 'react';
import { useNavigate, useParams } from 'react-router-dom';

import { useDetails } from '@/features/genre/hooks/useCrud.js';
import { routes } from '@/shared/lib/constants/api.js';
import { toIntId } from '@/shared/lib/utils/utils.js';

export const useGenreDetailsPage = () => {
  const navigate = useNavigate();
  const { id } = useParams<{ id: string }>();
  const parsedId = toIntId(id);
  const disable = !parsedId;

  const { genre, isFetching, error } = useDetails(parsedId, disable);

  const handleAllBooksClick = (e: React.MouseEvent) => {
    e.preventDefault();
    if (!genre || !parsedId) {
      return;
    }

    navigate(routes.book, { state: { genreId: parsedId, genreName: genre.name } });
  };

  return {
    genre,
    isFetching,
    error,
    handleAllBooksClick,
  };
};
