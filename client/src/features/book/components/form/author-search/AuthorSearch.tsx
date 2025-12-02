import './AuthorSearch.css';

import type { FormikProps } from 'formik';
import type { FC } from 'react';
import { useTranslation } from 'react-i18next';

import { useSearchNames } from '@/features/author/hooks/useCrud.js';
import type { AuthorNames } from '@/features/author/types/author.js';
import type { BookFormValues } from '@/features/book/components/form/validation/bookSchema.js';

type Props = {
  authors: AuthorNames[];
  loading: boolean;
  formik: FormikProps<BookFormValues>;
};

const AuthorSearch: FC<Props> = ({ authors, loading, formik }) => {
  const { t } = useTranslation('books');

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
        {t('form.authorSearch.label')}{' '}
        <span className="fw-normal">({t('form.authorSearch.helper')})</span>
      </h6>
      <input
        type="text"
        id="authorName"
        className="form-control"
        placeholder={t('form.authorSearch.placeholder')}
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
        aria-label={t('form.authorSearch.ariaLabel')}
        autoComplete="off"
      />
      {loading ? (
        <div className="mt-2">{t('form.authorSearch.loading')}</div>
      ) : (
        showDropdown &&
        searchTerm && (
          <ul
            className="list-group mt-2 dropdown-list"
            role="listbox"
            aria-label={t('form.authorSearch.ariaLabel')}
          >
            {filteredAuthors.length === 0 ? (
              <li className="list-group-item">{t('form.authorSearch.noMatches')}</li>
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
