import type { FormikProps } from 'formik';
import { MDBRadio } from 'mdb-react-ui-kit';
import type { ChangeEvent, FC } from 'react';
import { useTranslation } from 'react-i18next';

import type { AuthorFormValues } from '../validation/authorSchema.js';

const GenderRadio: FC<{ formik: FormikProps<AuthorFormValues> }> = ({ formik }) => {
  const { t } = useTranslation('authors');

  const handleChange = (e: ChangeEvent<HTMLInputElement>) => {
    formik.handleChange(e);
  };

  return (
    <div>
      <label className="author-form-label">{t('form.gender.label')}</label>
      <div className="author-form-radio-control author-form-radio-surface">
        <MDBRadio
          name="gender"
          id="genderMale"
          value="male"
          label={t('form.gender.male')}
          inline
          onChange={handleChange}
          checked={formik.values.gender === 'male'}
        />
        <MDBRadio
          name="gender"
          id="genderFemale"
          value="female"
          label={t('form.gender.female')}
          inline
          onChange={handleChange}
          checked={formik.values.gender === 'female'}
        />
        <MDBRadio
          name="gender"
          id="genderOther"
          value="other"
          label={t('form.gender.other')}
          inline
          onChange={handleChange}
          checked={formik.values.gender === 'other'}
        />
      </div>
      {formik.touched.gender && formik.errors.gender && (
        <div className="text-danger">{formik.errors.gender}</div>
      )}
    </div>
  );
};

export default GenderRadio;
