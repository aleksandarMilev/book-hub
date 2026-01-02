import './ProfileForm.css';

import { MDBCard, MDBCardBody, MDBCol, MDBContainer, MDBRow } from 'mdb-react-ui-kit';
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

                <form onSubmit={formik.handleSubmit} encType="multipart/form-data" noValidate>
                  <MDBRow>
                    <MDBCol md="12">
                      <div className="mb-4">
                        <label htmlFor="firstName" className="form-label">
                          {t('form.labels.firstName')}
                        </label>
                        {formik.touched.firstName && formik.errors.firstName && (
                          <div className="text-danger mb-2">{formik.errors.firstName}</div>
                        )}
                        <input
                          id="firstName"
                          type="text"
                          className={`form-control form-control-lg ${
                            formik.touched.firstName && formik.errors.firstName ? 'is-invalid' : ''
                          }`}
                          {...formik.getFieldProps('firstName')}
                        />
                      </div>
                    </MDBCol>
                  </MDBRow>

                  <MDBRow>
                    <MDBCol md="12">
                      <div className="mb-4">
                        <label htmlFor="lastName" className="form-label">
                          {t('form.labels.lastName')}
                        </label>
                        {formik.touched.lastName && formik.errors.lastName && (
                          <div className="text-danger mb-2">{formik.errors.lastName}</div>
                        )}
                        <input
                          id="lastName"
                          type="text"
                          className={`form-control form-control-lg ${
                            formik.touched.lastName && formik.errors.lastName ? 'is-invalid' : ''
                          }`}
                          {...formik.getFieldProps('lastName')}
                        />
                      </div>
                    </MDBCol>
                  </MDBRow>

                  <MDBRow>
                    <MDBCol md="12">
                      <div className="mb-4">
                        <label htmlFor="image" className="form-label">
                          {t('form.labels.image')}
                        </label>
                        {formik.touched.image && formik.errors.image && (
                          <div className="text-danger mb-2">{formik.errors.image as string}</div>
                        )}
                        <input
                          id="image"
                          name="image"
                          type="file"
                          disabled={formik.values.removeImage}
                          accept=".jpg,.jpeg,.png,.webp,.avif"
                          className={`form-control ${
                            formik.touched.image && formik.errors.image ? 'is-invalid' : ''
                          }`}
                          onChange={(event: React.ChangeEvent<HTMLInputElement>) => {
                            const file = event.currentTarget.files?.[0] ?? null;
                            formik.setFieldValue('image', file);
                            formik.setFieldTouched('image', true, false);

                            if (file) {
                              formik.setFieldValue('removeImage', false);
                            }
                          }}
                          onBlur={formik.handleBlur}
                        />
                      </div>

                      {profile?.imagePath &&
                        profile.imagePath !== '/images/profiles/default.jpg' && (
                          <div className="form-check mt-2 mb-4">
                            <input
                              type="checkbox"
                              id="removeImage"
                              name="removeImage"
                              className="form-check-input"
                              checked={formik.values.removeImage}
                              onChange={(e) => {
                                const checked = e.target.checked;
                                formik.setFieldValue('removeImage', checked);

                                if (checked) {
                                  formik.setFieldValue('image', null);
                                  formik.setFieldTouched('image', false, false);
                                }
                              }}
                              onBlur={formik.handleBlur}
                            />
                            <label htmlFor="removeImage" className="form-check-label">
                              {t('form.labels.removeImage')}
                            </label>
                          </div>
                        )}
                    </MDBCol>
                  </MDBRow>

                  <MDBRow>
                    <MDBCol md="12">
                      <div className="mb-4">
                        <label htmlFor="dateOfBirth" className="form-label">
                          {t('form.labels.dateOfBirth')}
                        </label>
                        {formik.touched.dateOfBirth && formik.errors.dateOfBirth && (
                          <div className="text-danger mb-2">{formik.errors.dateOfBirth}</div>
                        )}
                        <input
                          id="dateOfBirth"
                          type="date"
                          className={`form-control form-control-lg ${
                            formik.touched.dateOfBirth && formik.errors.dateOfBirth
                              ? 'is-invalid'
                              : ''
                          }`}
                          {...formik.getFieldProps('dateOfBirth')}
                        />
                      </div>
                    </MDBCol>
                  </MDBRow>

                  <MDBRow>
                    <MDBCol md="12">
                      <div className="mb-4">
                        <label htmlFor="socialMediaUrl" className="form-label">
                          {t('form.labels.socialMediaUrl')}
                        </label>
                        {formik.touched.socialMediaUrl && formik.errors.socialMediaUrl && (
                          <div className="text-danger mb-2">{formik.errors.socialMediaUrl}</div>
                        )}
                        <input
                          id="socialMediaUrl"
                          type="text"
                          className={`form-control form-control-lg ${
                            formik.touched.socialMediaUrl && formik.errors.socialMediaUrl
                              ? 'is-invalid'
                              : ''
                          }`}
                          value={formik.values.socialMediaUrl ?? ''}
                          onChange={(e: React.ChangeEvent<HTMLInputElement>) =>
                            formik.setFieldValue('socialMediaUrl', e.target.value || null)
                          }
                          onBlur={formik.handleBlur}
                        />
                      </div>
                    </MDBCol>
                  </MDBRow>

                  <MDBRow>
                    <MDBCol md="12">
                      <div className="mb-4">
                        <label htmlFor="biography" className="form-label">
                          {t('form.labels.biography')}
                        </label>
                        {formik.touched.biography && formik.errors.biography && (
                          <div className="text-danger mb-2">{formik.errors.biography}</div>
                        )}
                        <textarea
                          id="biography"
                          rows={5}
                          value={formik.values.biography ?? ''}
                          onChange={(e) =>
                            formik.setFieldValue('biography', e.target.value || null)
                          }
                          onBlur={formik.handleBlur}
                          className={`form-control ${
                            formik.touched.biography && formik.errors.biography ? 'is-invalid' : ''
                          }`}
                          placeholder={t('form.placeholders.biography')}
                        />
                      </div>
                    </MDBCol>
                  </MDBRow>

                  <div className="form-check mb-4">
                    <input
                      type="checkbox"
                      id="isPrivate"
                      name="isPrivate"
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

                  <div className="profile-form-actions">
                    <button
                      type="button"
                      className="profile-form-button profile-form-reset"
                      onClick={formik.handleReset}
                      disabled={formik.isSubmitting}
                    >
                      {t('form.buttons.reset')}
                    </button>
                    <button
                      type="submit"
                      className="profile-form-button profile-form-submit"
                      disabled={formik.isSubmitting}
                    >
                      {formik.isSubmitting
                        ? t('form.buttons.submitting')
                        : t('form.buttons.submit')}
                    </button>
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
