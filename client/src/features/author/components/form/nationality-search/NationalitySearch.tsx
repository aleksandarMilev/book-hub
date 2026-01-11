import type { FormikProps } from 'formik';
import type { FC } from 'react';

import type { AuthorFormValues } from '@/features/author/components/form/validation/authorSchema.js';
import { useSearchNationalities } from '@/features/author/hooks/useNationality.js';
import type { Nationality } from '@/features/author/types/author.js';

type Props = {
  nationalities: Nationality[];
  loading: boolean;
  formik: FormikProps<AuthorFormValues>;
};

const NationalitySearch: FC<Props> = ({ nationalities, loading, formik }) => {
  const {
    t,
    searchTerm,
    filteredNationalities,
    showDropdown,
    updateSearchTerm,
    selectNationality,
    showDropdownOnFocus,
    hideDropdownOnBlur,
  } = useSearchNationalities(nationalities, formik.values.nationality);

  return (
    <div>
      <label htmlFor="nationality" className="author-form-label">
        {t('form.nationality.label')}{' '}
        <span className="fw-normal">{t('form.nationality.helper')}</span>
      </label>
      <input
        type="text"
        id="nationality"
        className="form-control author-form-nationality-input"
        placeholder={t('form.nationality.placeholder')}
        value={searchTerm}
        onChange={(e) => updateSearchTerm(e.target.value)}
        onFocus={showDropdownOnFocus}
        onBlur={hideDropdownOnBlur}
      />
      {loading ? (
        <div className="mt-2">{t('form.nationality.loading')}</div>
      ) : (
        showDropdown &&
        searchTerm && (
          <ul className="author-form-nationality-dropdown">
            {filteredNationalities.length === 0 ? (
              <li className="author-form-nationality-item">{t('form.nationality.noMatches')}</li>
            ) : (
              filteredNationalities.map((n) => (
                <li
                  key={n.id}
                  className="author-form-nationality-item"
                  onMouseDown={(e) => e.preventDefault()}
                  onClick={() => {
                    formik.setFieldValue('nationality', n.id);
                    selectNationality(n);
                  }}
                >
                  {n.name}
                </li>
              ))
            )}
          </ul>
        )
      )}
    </div>
  );
};

export default NationalitySearch;
