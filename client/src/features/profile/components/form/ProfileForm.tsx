import './ProfileForm.css';

import {
  MDBBtn,
  MDBCard,
  MDBCardBody,
  MDBCol,
  MDBContainer,
  MDBInput,
  MDBRow,
} from 'mdb-react-ui-kit';
import type React from 'react';
import type { FC } from 'react';
import { useTranslation } from 'react-i18next';

import { useProfileFormik } from '@/features/profile/components/form/formik/useProfileFormik.js';
import type { Profile } from '@/features/profile/types/profile.js';

type Props = { profile?: Profile | null };

const ProfileForm: FC<Props> = ({ profile = null }) => {
  const formik = useProfileFormik({ profile });
  const { t } = useTranslation('profiles');

  return (
    <div className="profile-form-container">
      <MDBContainer fluid>
        <MDBRow className="form-row">
          <MDBCol className="form-col">
            <MDBCard className="my-4 profile-details-card">
              <MDBCardBody className="text-black">
                <h3 className="mb-5 fw-bold">{t('form.title')}</h3>
                <form onSubmit={formik.handleSubmit} encType="multipart/form-data">
                  <MDBRow>
                    <MDBCol md="12">
                      {formik.touched.firstName && formik.errors.firstName && (
                        <div className="text-danger mb-2">{formik.errors.firstName}</div>
                      )}
                      <MDBInput
                        wrapperClass="mb-4"
                        label={t('form.labels.firstName')}
                        size="lg"
                        id="firstName"
                        type="text"
                        {...formik.getFieldProps('firstName')}
                        className={
                          formik.touched.firstName && formik.errors.firstName ? 'is-invalid' : ''
                        }
                      />
                    </MDBCol>
                  </MDBRow>
                  <MDBRow>
                    <MDBCol md="12">
                      {formik.touched.lastName && formik.errors.lastName && (
                        <div className="text-danger mb-2">{formik.errors.lastName}</div>
                      )}
                      <MDBInput
                        wrapperClass="mb-4"
                        label={t('form.labels.lastName')}
                        size="lg"
                        id="lastName"
                        type="text"
                        {...formik.getFieldProps('lastName')}
                        className={
                          formik.touched.lastName && formik.errors.lastName ? 'is-invalid' : ''
                        }
                      />
                    </MDBCol>
                  </MDBRow>
                  <MDBRow>
                    <MDBCol md="12">
                      {formik.touched.image && formik.errors.image && (
                        <div className="text-danger mb-2">{formik.errors.image as string}</div>
                      )}
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
                        onChange={(event: React.ChangeEvent<HTMLInputElement>) => {
                          const file = event.currentTarget.files?.[0] ?? null;
                          formik.setFieldValue('image', file);
                          formik.setFieldTouched('image', true, false);
                        }}
                        onBlur={formik.handleBlur}
                      />
                    </MDBCol>
                  </MDBRow>
                  <MDBRow>
                    <MDBCol md="12">
                      {formik.touched.dateOfBirth && formik.errors.dateOfBirth && (
                        <div className="text-danger mb-2">{formik.errors.dateOfBirth}</div>
                      )}
                      <MDBInput
                        wrapperClass="mb-4"
                        label={t('form.labels.dateOfBirth')}
                        size="lg"
                        id="dateOfBirth"
                        type="date"
                        {...formik.getFieldProps('dateOfBirth')}
                        className={
                          formik.touched.dateOfBirth && formik.errors.dateOfBirth
                            ? 'is-invalid'
                            : ''
                        }
                      />
                    </MDBCol>
                  </MDBRow>
                  <MDBRow>
                    <MDBCol md="12">
                      {formik.touched.socialMediaUrl && formik.errors.socialMediaUrl && (
                        <div className="text-danger mb-2">{formik.errors.socialMediaUrl}</div>
                      )}
                      <MDBInput
                        wrapperClass="mb-4"
                        label={t('form.labels.socialMediaUrl')}
                        size="lg"
                        id="socialMediaUrl"
                        type="text"
                        value={formik.values.socialMediaUrl ?? ''}
                        onChange={(e: React.ChangeEvent<HTMLInputElement>) =>
                          formik.setFieldValue('socialMediaUrl', e.target.value || null)
                        }
                        onBlur={formik.handleBlur}
                        className={
                          formik.touched.socialMediaUrl && formik.errors.socialMediaUrl
                            ? 'is-invalid'
                            : ''
                        }
                      />
                    </MDBCol>
                  </MDBRow>
                  <MDBRow>
                    <MDBCol md="12">
                      {formik.touched.biography && formik.errors.biography && (
                        <div className="text-danger mb-2">{formik.errors.biography}</div>
                      )}
                      <textarea
                        id="biography"
                        rows={5}
                        value={formik.values.biography ?? ''}
                        onChange={(e) => formik.setFieldValue('biography', e.target.value || null)}
                        onBlur={formik.handleBlur}
                        className={`form-control ${
                          formik.touched.biography && formik.errors.biography ? 'is-invalid' : ''
                        }`}
                        placeholder={t('form.placeholders.biography')}
                      />
                    </MDBCol>
                  </MDBRow>
                  <div className="form-check mb-4">
                    <input
                      type="checkbox"
                      id="isPrivate"
                      className="form-check-input"
                      checked={formik.values.isPrivate}
                      onChange={formik.handleChange}
                      onBlur={formik.handleBlur}
                    />
                    <label htmlFor="isPrivate" className="form-check-label">
                      {t('form.labels.isPrivate')}
                    </label>
                  </div>
                  <p className="text-danger fw-bold mt-2">{t('form.notes.required')}</p>
                  <div className="d-flex justify-content-end pt-3">
                    <MDBBtn color="light" size="lg" type="button" onClick={formik.handleReset}>
                      {t('form.buttons.reset')}
                    </MDBBtn>
                    <MDBBtn className="ms-2" color="warning" size="lg" type="submit">
                      {formik.isSubmitting
                        ? t('form.buttons.submitting')
                        : t('form.buttons.submit')}
                    </MDBBtn>
                  </div>
                </form>
              </MDBCardBody>
            </MDBCard>
          </MDBCol>
        </MDBRow>
      </MDBContainer>
    </div>
  );
};

export default ProfileForm;
