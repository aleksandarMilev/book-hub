import type { FormikProps } from 'formik';
import type { Nationality } from '../../../../../api/nationality/types/nationality';
import type { NationalitySearchValues } from './nationalitySearchValues';

export interface NationalitySearchProps {
  nationalities: Nationality[];
  loading: boolean;
  formik: FormikProps<NationalitySearchValues>;
}
