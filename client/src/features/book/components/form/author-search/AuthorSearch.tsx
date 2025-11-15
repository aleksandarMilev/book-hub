import './AuthorSearch.css';

import type { FormikProps } from 'formik';
import type { FC } from 'react';

import { useSearchNames } from '@/features/author/hooks/useCrud.js';
import type { AuthorNames } from '@/features/author/types/author.js';
import type { BookFormValues } from '@/features/book/components/form/formik/useBookFormik.js';

const AuthorSearch: FC<{
  authors: AuthorNames[];
  loading: boolean;
  formik: FormikProps<BookFormValues>;
}> = ({ authors, loading, formik }) => {
  const {
    searchTerm,
    filteredAuthors,
    showDropdown,
    updateSearchTerm,
    selectAuthor,
    showDropdownOnFocus,
  } = useSearchNames(authors);

  return (
    <div className="mb-4">
      <h6 className="fw-bold mb-2">
        Author Name:{' '}
        <span className="fw-normal">(Select known authors, or leave blank if unknown)</span>
      </h6>
      <input
        type="text"
        id="authorName"
        className="form-control"
        placeholder="Search for an author..."
        value={searchTerm}
        onChange={(e) => {
          const value = e.target.value;
          updateSearchTerm(value);

          if (!value) {
            formik.setFieldValue('authorId', null);
          }
        }}
        onFocus={showDropdownOnFocus}
        aria-autocomplete="list"
        autoComplete="off"
      />
      {loading ? (
        <div className="mt-2">Loading...</div>
      ) : (
        showDropdown &&
        searchTerm && (
          <ul className="list-group mt-2 dropdown-list" role="listbox" aria-label="Authors">
            {filteredAuthors.length === 0 ? (
              <li className="list-group-item">No matches found</li>
            ) : (
              filteredAuthors.map((a) => (
                <li
                  key={a.id}
                  className="list-group-item dropdown-item"
                  onClick={() => {
                    formik.setFieldValue('authorId', a.id);
                    selectAuthor(a.name);
                  }}
                  role="option"
                  aria-selected={false}
                >
                  {a.name}
                </li>
              ))
            )}
          </ul>
        )
      )}
    </div>
  );
};

export default AuthorSearch;
