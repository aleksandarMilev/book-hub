import type React from 'react';
import { useNavigate, useParams } from 'react-router-dom';

import * as hooks from '@/features/genre/hooks/useCrud';
import { routes } from '@/shared/lib/constants/api';
import { toIntId } from '@/shared/lib/utils';

export function useGenreDetailsPage() {
  const navigate = useNavigate();
  const { id } = useParams<{ id: string }>();
  const parsedId = toIntId(id);
  const disable = !parsedId;

  const { data: genre, isFetching, error } = hooks.useDetails(parsedId, disable);

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
}
