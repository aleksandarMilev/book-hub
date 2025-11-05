import type { FC } from 'react';
import { useSearch } from '../../../../hooks/useGenre';

import './GenreSearch.css';
import type { GenreSearchProps, NamedEntity } from '../../../../api/book/types/book';

const GenreSearch: FC<GenreSearchProps> = ({
  genres,
  loading,
  formik,
  selectedGenres,
  setSelectedGenres,
}) => {
  const { searchTerm, filteredGenres, updateSearchTerm } = useSearch(genres, selectedGenres);

  const selectGenre = (genre: NamedEntity) => {
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

  const removeGenre = (genre: NamedEntity) => {
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
        Genres:{' '}
        <span className="fw-normal">
          * (Select known genres, or &ldquo;Other&rdquo; if unknown)
        </span>
      </h6>
      <input
        type="text"
        id="genreSearch"
        className="form-control"
        placeholder="Search for genres..."
        value={searchTerm}
        onChange={(e) => updateSearchTerm(e.target.value)}
        aria-autocomplete="list"
        autoComplete="off"
      />
      {loading ? (
        <div className="mt-2">Loading...</div>
      ) : (
        <div>
          {searchTerm && (
            <ul className="list-group mt-2 genre-list" role="listbox" aria-label="Genres">
              {filteredGenres.length === 0 ? (
                <li className="list-group-item">No matches found</li>
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
                      aria-label={`Remove ${g.name}`}
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
