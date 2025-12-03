import type React from 'react';
import { useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';

import { useDetails } from '@/features/genre/hooks/useCrud.js';
import { routes } from '@/shared/lib/constants/api.js';
import { slugify } from '@/shared/lib/utils/utils.js';

export const useGenreDetailsPage = () => {
  const navigate = useNavigate();
  const { id, slug } = useParams<{ id: string; slug: string }>();
  const { genre, isFetching, error } = useDetails(id);

  useEffect(() => {
    if (!genre || !id) {
      return;
    }

    const canonicalSlug = slugify(genre.name);

    if (!slug || slug !== canonicalSlug) {
      navigate(`${routes.genres}/${id}/${canonicalSlug}`, { replace: true });
    }
  }, [genre, id, slug, navigate]);

  const handleAllBooksClick = (e: React.MouseEvent) => {
    e.preventDefault();
    if (!genre || !id) {
      return;
    }

    navigate(routes.book, { state: { genreId: id, genreName: genre.name } });
  };

  return {
    genre,
    isFetching,
    error,
    handleAllBooksClick,
  };
};
