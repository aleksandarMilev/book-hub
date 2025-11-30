import './AuthorForm.css';

import type { FormikProps } from 'formik';
import {
  MDBBtn,
  MDBCard,
  MDBCardBody,
  MDBCardImage,
  MDBCol,
  MDBContainer,
  MDBInput,
  MDBRow,
} from 'mdb-react-ui-kit';
import type { FC } from 'react';
import { useTranslation } from 'react-i18next';

import image from '@/features/author/components/form/assets/create-author.jpg';
import { useAuthorFormik } from '@/features/author/components/form/formik/useAuthorFormik.js';
import GenderRadio from '@/features/author/components/form/gender-radio/GenderRadio.js';
import NationalitySearch from '@/features/author/components/form/nationality-search/NationalitySearch.js';
import type { AuthorFormValues } from '@/features/author/components/form/validation/authorSchema.js';
import { useAll } from '@/features/author/hooks/useNationality.js';
import type { AuthorDetails } from '@/features/author/types/author.js';

type Props = { authorData?: AuthorDetails | null; isEditMode?: boolean };

const AuthorForm: FC<Props> = ({ authorData = null, isEditMode = false }) => {
  const { t } = useTranslation('authors');
  const formik = useAuthorFormik({ authorData, isEditMode });
  const { nationalities, isFetching } = useAll();

  const submitLabel = formik.isSubmitting
    ? isEditMode
      ? t('form.buttons.submittingEdit')
      : t('form.buttons.submittingCreate')
    : isEditMode
      ? t('form.buttons.submitEdit')
      : t('form.buttons.submitCreate');

  return (
    <MDBContainer fluid className="author-form-container">
      <MDBRow className="d-flex justify-content-center align-items-center h-100">
        <MDBCol>
          <MDBCard className="my-4 author-form-card">
            <MDBRow className="g-0">
              <MDBCol md="6" className="d-none d-md-block">
                <MDBCardImage src={image} alt="Author" className="author-form-image" fluid />
              </MDBCol>
              <MDBCol md="6">
                <MDBCardBody className="text-black d-flex flex-column justify-content-center">
                  <h3 className="mb-5 fw-bold">
                    {isEditMode ? t('form.titleEdit') : t('form.titleCreate')}
                  </h3>
                  <form onSubmit={formik.handleSubmit}>
                    <MDBCol md="12">
                      {formik.touched.name && formik.errors.name && (
                        <div className="text-danger mb-2">{formik.errors.name}</div>
                      )}
                      <MDBInput
                        wrapperClass="mb-4"
                        label={t('form.labels.name')}
                        size="lg"
                        id="name"
                        type="text"
                        {...formik.getFieldProps('name')}
                        className={formik.touched.name && formik.errors.name ? 'is-invalid' : ''}
                      />
                    </MDBCol>
                    <MDBCol md="12">
                      <MDBInput
                        wrapperClass="mb-4"
                        label={t('form.labels.penName')}
                        size="lg"
                        id="penName"
                        type="text"
                        {...formik.getFieldProps('penName')}
                        className={
                          formik.touched.penName && formik.errors.penName ? 'is-invalid' : ''
                        }
                      />
                    </MDBCol>
                    <MDBCol md="12">
                      <label htmlFor="image" className="form-label">
                        {t('form.labels.image')}
                      </label>
                      <input
                        id="image"
                        name="image"
                        type="file"
                        accept=".jpg,.jpeg,.png,.webp,.avif"
                        className={`form-control ${
                          formik.touched.image && formik.errors.image ? 'is-invalid' : ''
                        }`}
                        onChange={(event) => {
                          const file = event.currentTarget.files?.[0] ?? null;
                          formik.setFieldValue('image', file);
                          formik.setFieldTouched('image', true, false);
                        }}
                      />
                      {formik.touched.image && formik.errors.image && (
                        <div className="text-danger mb-2">{formik.errors.image}</div>
                      )}
                    </MDBCol>
                    <MDBCol md="12">
                      {formik.touched.bornAt && formik.errors.bornAt && (
                        <div className="text-danger mb-2">{formik.errors.bornAt}</div>
                      )}
                      <MDBInput
                        wrapperClass="mb-4"
                        label={t('form.labels.bornAt')}
                        size="lg"
                        id="bornAt"
                        type="date"
                        {...formik.getFieldProps('bornAt')}
                        className={
                          formik.touched.bornAt && formik.errors.bornAt ? 'is-invalid' : ''
                        }
                      />
                    </MDBCol>
                    <MDBCol md="12">
                      {formik.touched.diedAt && formik.errors.diedAt && (
                        <div className="text-danger mb-2">{formik.errors.diedAt}</div>
                      )}
                      <MDBInput
                        wrapperClass="mb-4"
                        label={t('form.labels.diedAt')}
                        size="lg"
                        id="diedAt"
                        type="date"
                        {...formik.getFieldProps('diedAt')}
                        className={
                          formik.touched.diedAt && formik.errors.diedAt ? 'is-invalid' : ''
                        }
                      />
                    </MDBCol>
                    <GenderRadio formik={formik as FormikProps<AuthorFormValues>} />
                    <NationalitySearch
                      nationalities={nationalities}
                      loading={isFetching}
                      formik={formik as FormikProps<AuthorFormValues>}
                    />
                    <MDBCol md="12">
                      {formik.touched.biography && formik.errors.biography && (
                        <div className="text-danger mb-2">{formik.errors.biography}</div>
                      )}
                      <textarea
                        id="biography"
                        rows={6}
                        {...formik.getFieldProps('biography')}
                        className={`form-control ${
                          formik.touched.biography && formik.errors.biography ? 'is-invalid' : ''
                        }`}
                        placeholder={t('form.placeholders.biography')}
                      />
                    </MDBCol>
                    <p className="text-danger fw-bold mt-2">{t('form.requiredNote')}</p>
                    <div className="d-flex justify-content-end pt-3">
                      <MDBBtn color="light" size="lg" type="button" onClick={formik.handleReset}>
                        {t('form.buttons.reset')}
                      </MDBBtn>
                      <MDBBtn
                        className="ms-2"
                        color="warning"
                        size="lg"
                        type="submit"
                        disabled={formik.isSubmitting}
                      >
                        {submitLabel}
                      </MDBBtn>
                    </div>
                  </form>
                </MDBCardBody>
              </MDBCol>
            </MDBRow>
          </MDBCard>
        </MDBCol>
      </MDBRow>
    </MDBContainer>
  );
};

export default AuthorForm;
