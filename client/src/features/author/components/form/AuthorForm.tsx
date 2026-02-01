import './AuthorForm.css';

import type { FormikProps } from 'formik';
import {
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
import { useAuthorFormik } from '@/features/author/components/form/formik/useAuthorFormik';
import GenderRadio from '@/features/author/components/form/gender-radio/GenderRadio';
import NationalitySearch from '@/features/author/components/form/nationality-search/NationalitySearch';
import type { AuthorFormValues } from '@/features/author/components/form/validation/authorSchema';
import { useAll } from '@/features/author/hooks/useNationality';
import type { AuthorDetails } from '@/features/author/types/author';

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
            <MDBRow className="g-0 author-form-row">
              <MDBCol md="6" className="d-none d-md-block author-form-image-col">
                <MDBCardImage src={image} alt="Author" className="author-form-image" fluid />
              </MDBCol>
              <MDBCol md="6">
                <MDBCardBody className="text-black d-flex flex-column justify-content-center">
                  <h3 className="mb-4 fw-bold">
                    {isEditMode ? t('form.titleEdit') : t('form.titleCreate')}
                  </h3>
                  <form onSubmit={formik.handleSubmit} className="author-form">
                    <div className="author-form-field">
                      <label htmlFor="name" className="author-form-label">
                        {t('form.labels.name')}
                      </label>
                      <MDBInput
                        wrapperClass="author-form-control"
                        size="lg"
                        id="name"
                        type="text"
                        {...formik.getFieldProps('name')}
                        className={formik.touched.name && formik.errors.name ? 'is-invalid' : ''}
                      />
                      {formik.touched.name && formik.errors.name && (
                        <div className="text-danger">{formik.errors.name}</div>
                      )}
                    </div>
                    <div className="author-form-field">
                      <label htmlFor="penName" className="author-form-label">
                        {t('form.labels.penName')}
                      </label>
                      <MDBInput
                        wrapperClass="author-form-control"
                        size="lg"
                        id="penName"
                        type="text"
                        {...formik.getFieldProps('penName')}
                        className={
                          formik.touched.penName && formik.errors.penName ? 'is-invalid' : ''
                        }
                      />
                      {formik.touched.penName && formik.errors.penName && (
                        <div className="text-danger">{formik.errors.penName}</div>
                      )}
                    </div>
                    <div className="author-form-field">
                      <label htmlFor="image" className="author-form-label">
                        {t('form.labels.image')}
                      </label>
                      <input
                        id="image"
                        name="image"
                        type="file"
                        accept=".jpg,.jpeg,.png,.webp,.avif"
                        className={`form-control author-form-file-input ${
                          formik.touched.image && formik.errors.image ? 'is-invalid' : ''
                        }`}
                        onChange={(event) => {
                          const file = event.currentTarget.files?.[0] ?? null;
                          formik.setFieldValue('image', file);
                          formik.setFieldTouched('image', true, false);
                        }}
                      />
                      {formik.touched.image && formik.errors.image && (
                        <div className="text-danger">{formik.errors.image}</div>
                      )}
                    </div>
                    <div className="author-form-field">
                      <label htmlFor="bornAt" className="author-form-label">
                        {t('form.labels.bornAt')}
                      </label>
                      <MDBInput
                        wrapperClass="author-form-control"
                        size="lg"
                        id="bornAt"
                        type="date"
                        {...formik.getFieldProps('bornAt')}
                        className={
                          formik.touched.bornAt && formik.errors.bornAt ? 'is-invalid' : ''
                        }
                      />
                      {formik.touched.bornAt && formik.errors.bornAt && (
                        <div className="text-danger">{formik.errors.bornAt}</div>
                      )}
                    </div>
                    <div className="author-form-field">
                      <label htmlFor="diedAt" className="author-form-label">
                        {t('form.labels.diedAt')}
                      </label>
                      <MDBInput
                        wrapperClass="author-form-control"
                        size="lg"
                        id="diedAt"
                        type="date"
                        {...formik.getFieldProps('diedAt')}
                        className={
                          formik.touched.diedAt && formik.errors.diedAt ? 'is-invalid' : ''
                        }
                      />
                      {formik.touched.diedAt && formik.errors.diedAt && (
                        <div className="text-danger">{formik.errors.diedAt}</div>
                      )}
                    </div>
                    <div className="author-form-field">
                      <div className="author-form-gender">
                        <GenderRadio formik={formik as FormikProps<AuthorFormValues>} />
                      </div>
                    </div>
                    <div className="author-form-field">
                      <div className="author-form-nationality">
                        <NationalitySearch
                          nationalities={nationalities}
                          loading={isFetching}
                          formik={formik as FormikProps<AuthorFormValues>}
                        />
                      </div>
                    </div>
                    <div className="author-form-field">
                      <label htmlFor="biography" className="author-form-label">
                        {t('form.labels.biography')}
                      </label>
                      <textarea
                        id="biography"
                        rows={6}
                        {...formik.getFieldProps('biography')}
                        className={`form-control author-form-textarea ${
                          formik.touched.biography && formik.errors.biography ? 'is-invalid' : ''
                        }`}
                        placeholder={t('form.placeholders.biography')}
                      />
                      {formik.touched.biography && formik.errors.biography && (
                        <div className="text-danger">{formik.errors.biography}</div>
                      )}
                    </div>
                    <p className="text-danger fw-bold author-form-required-note">
                      {t('form.requiredNote')}
                    </p>
                    <div className="author-form-actions">
                      <button
                        type="button"
                        className="author-form-btn author-form-btn--reset"
                        onClick={formik.handleReset}
                        disabled={formik.isSubmitting}
                      >
                        {t('form.buttons.reset')}
                      </button>
                      <button
                        type="submit"
                        className="author-form-btn author-form-btn--submit"
                        disabled={formik.isSubmitting}
                      >
                        {submitLabel}
                      </button>
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


