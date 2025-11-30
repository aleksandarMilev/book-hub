import type { FormikProps } from 'formik';
import type { ChangeEvent, FC } from 'react';
import { useTranslation } from 'react-i18next';

import type { AuthorFormValues } from '@/features/author/components/form/validation/authorSchema.js';
import { useSearchNationalities } from '@/features/author/hooks/useNationality.js';
import type { Nationality } from '@/features/author/types/author.js';

type Props = {
  nationalities: Nationality[];
  loading: boolean;
  formik: FormikProps<AuthorFormValues>;
};

const NationalitySearch: FC<Props> = ({ nationalities, loading, formik }) => {
  const { t } = useTranslation('authors');
  const {
    searchTerm,
    filteredNationalities,
    showDropdown,
    updateSearchTerm,
    selectNationality,
    showDropdownOnFocus,
  } = useSearchNationalities(nationalities);

  const handleInputChange = (e: ChangeEvent<HTMLInputElement>) => {
    updateSearchTerm(e.target.value);
  };

  return (
    <div className="mb-4">
      <h6 className="fw-bold mb-2">
        {t('form.nationality.label')}{' '}
        <span className="fw-normal">{t('form.nationality.helper')}</span>
      </h6>
      <input
        type="text"
        id="nationality"
        className="form-control"
        placeholder={t('form.nationality.placeholder')}
        value={searchTerm}
        onChange={handleInputChange}
        onFocus={showDropdownOnFocus}
      />
      {loading ? (
        <div className="mt-2">{t('form.nationality.loading')}</div>
      ) : (
        showDropdown &&
        searchTerm && (
          <ul className="list-group mt-2" style={{ maxHeight: '200px', overflowY: 'auto' }}>
            {filteredNationalities.length === 0 ? (
              <li className="list-group-item">{t('form.nationality.noMatches')}</li>
            ) : (
              filteredNationalities.map((n) => (
                <li
                  key={n.id}
                  className="list-group-item"
                  onClick={() => {
                    formik.setFieldValue('nationality', n.id);
                    selectNationality(n);
                  }}
                  style={{
                    cursor: 'pointer',
                    padding: '10px',
                    borderBottom: '1px solid #ddd',
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
