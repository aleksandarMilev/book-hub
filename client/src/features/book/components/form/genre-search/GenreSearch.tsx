import './GenreSearch.css';

import type { FormikProps } from 'formik';
import type React from 'react';
import type { FC } from 'react';
import { useTranslation } from 'react-i18next';

import type { BookFormValues } from '@/features/book/components/form/validation/bookSchema.js';
import { useSearch } from '@/features/genre/hooks/useCrud.js';
import type { GenreName } from '@/features/genre/types/genre.js';

type Props = {
  genres: GenreName[];
  loading: boolean;
  formik: FormikProps<BookFormValues>;
  selectedGenres: GenreName[];
  setSelectedGenres: React.Dispatch<React.SetStateAction<GenreName[]>>;
};

const GenreSearch: FC<Props> = ({ genres, loading, formik, selectedGenres, setSelectedGenres }) => {
  const { t } = useTranslation('books');
  const { searchTerm, filteredGenres, updateSearchTerm } = useSearch(genres, selectedGenres);

  const selectGenre = (genre: GenreName) => {
    if (selectedGenres.some((g) => g.id === genre.id)) {
      updateSearchTerm('');
      return;
    }

    const newSelected = [...selectedGenres, genre];
    setSelectedGenres(newSelected);

    formik.setFieldValue(
      'genres',
      newSelected.map((g) => g.id),
    );

    updateSearchTerm('');
  };

  const removeGenre = (genre: GenreName) => {
    const newSelected = selectedGenres.filter((g) => g.id !== genre.id);
    setSelectedGenres(newSelected);

    formik.setFieldValue(
      'genres',
      newSelected.map((g) => g.id),
    );
  };

  return (
    <div className="mb-4">
      <h6 className="fw-bold mb-2">
        {t('validation.fields.genres')}:&nbsp;
        <span className="fw-normal">{t('form.genreSearch.helper')}</span>
      </h6>
      <input
        type="text"
        id="genreSearch"
        className="form-control"
        placeholder={t('form.genreSearch.placeholder')}
        value={searchTerm}
        onChange={(e) => updateSearchTerm(e.target.value)}
        aria-autocomplete="list"
        aria-label={t('form.genreSearch.ariaLabel')}
        autoComplete="off"
      />
      {loading ? (
        <div className="mt-2">{t('form.genreSearch.loading')}</div>
      ) : (
        <div>
          {searchTerm && (
            <ul
              className="list-group mt-2 genre-list"
              role="listbox"
              aria-label={t('form.genreSearch.ariaLabel')}
            >
              {filteredGenres.length === 0 ? (
                <li className="list-group-item">{t('form.genreSearch.noMatches')}</li>
              ) : (
                filteredGenres.map((g) => (
                  <li
                    key={g.id}
                    className="list-group-item genre-item"
                    onClick={() => selectGenre(g)}
                    role="option"
                    aria-selected={false}
                  >
                    {g.name}
                  </li>
                ))
              )}
            </ul>
          )}
          <div className="selected-genres mt-3">
            {selectedGenres.length > 0 && (
              <div className="d-flex flex-wrap">
                {selectedGenres.map((g) => (
                  <span key={g.id} className="badge badge-secondary m-1">
                    {g.name}
                    <button
                      type="button"
                      className="badge-close"
                      onClick={() => removeGenre(g)}
                      aria-label={t('form.genreSearch.removeGenreAria', { name: g.name })}
                    >
                      &times;
                    </button>
                  </span>
                ))}
              </div>
            )}
          </div>
        </div>
      )}
    </div>
  );
};

export default GenreSearch;
