import './Register.css';

import {
  MDBCard,
  MDBCardBody,
  MDBCardImage,
  MDBCol,
  MDBContainer,
  MDBIcon,
  MDBRow,
} from 'mdb-react-ui-kit';
import type { FC } from 'react';
import { useTranslation } from 'react-i18next';

import image from '@/features/identity/components/register/assets/register.webp';
import { useRegisterFormik } from '@/features/identity/components/register/formik/useRegisterFormik.js';

const Register: FC = () => {
  const formik = useRegisterFormik();
  const { t } = useTranslation('identity');

  const isSubmitting = formik.isSubmitting;

  return (
    <MDBContainer fluid className="identity-auth register-page">
      <MDBCard className="text-black register-card m-5">
        <MDBCardBody className="register-body">
          <MDBRow className="align-items-center">
            <MDBCol
              md="10"
              lg="6"
              className="order-2 order-lg-1 d-flex flex-column align-items-center"
            >
              <h2 className="register-title">{t('register.title')}</h2>
              <form
                onSubmit={formik.handleSubmit}
                className="w-100"
                noValidate
                encType="multipart/form-data"
              >
                <div className="auth-field">
                  <label htmlFor="firstName" className="auth-label">
                    {t('register.labels.firstName')}
                  </label>
                  {formik.touched.firstName && formik.errors.firstName && (
                    <div className="auth-error">{formik.errors.firstName}</div>
                  )}
                  <div className="register-field-row">
                    <MDBIcon fas icon="user" className="register-icon" />
                    <input
                      id="firstName"
                      type="text"
                      className={`auth-input ${
                        formik.touched.firstName && formik.errors.firstName ? 'is-invalid' : ''
                      }`}
                      {...formik.getFieldProps('firstName')}
                    />
                  </div>
                </div>
                <div className="auth-field">
                  <label htmlFor="lastName" className="auth-label">
                    {t('register.labels.lastName')}
                  </label>
                  {formik.touched.lastName && formik.errors.lastName && (
                    <div className="auth-error">{formik.errors.lastName}</div>
                  )}
                  <div className="register-field-row">
                    <MDBIcon fas icon="user-tag" className="register-icon" />
                    <input
                      id="lastName"
                      type="text"
                      className={`auth-input ${
                        formik.touched.lastName && formik.errors.lastName ? 'is-invalid' : ''
                      }`}
                      {...formik.getFieldProps('lastName')}
                    />
                  </div>
                </div>
                <div className="auth-field">
                  <label htmlFor="username" className="auth-label">
                    {t('register.labels.username')}
                  </label>
                  {formik.touched.username && formik.errors.username && (
                    <div className="auth-error">{formik.errors.username}</div>
                  )}
                  <div className="register-field-row">
                    <MDBIcon fas icon="user-circle" className="register-icon" />
                    <input
                      id="username"
                      type="text"
                      className={`auth-input ${
                        formik.touched.username && formik.errors.username ? 'is-invalid' : ''
                      }`}
                      {...formik.getFieldProps('username')}
                    />
                  </div>
                </div>
                <div className="auth-field">
                  <label htmlFor="email" className="auth-label">
                    {t('register.labels.email')}
                  </label>
                  {formik.touched.email && formik.errors.email && (
                    <div className="auth-error">{formik.errors.email}</div>
                  )}
                  <div className="register-field-row">
                    <MDBIcon fas icon="envelope" className="register-icon" />
                    <input
                      id="email"
                      type="email"
                      className={`auth-input ${
                        formik.touched.email && formik.errors.email ? 'is-invalid' : ''
                      }`}
                      {...formik.getFieldProps('email')}
                    />
                  </div>
                </div>
                <div className="auth-field">
                  <label htmlFor="password" className="auth-label">
                    {t('register.labels.password')}
                  </label>
                  {formik.touched.password && formik.errors.password && (
                    <div className="auth-error">{formik.errors.password}</div>
                  )}
                  <div className="register-field-row">
                    <MDBIcon fas icon="lock" className="register-icon" />
                    <input
                      id="password"
                      type="password"
                      className={`auth-input ${
                        formik.touched.password && formik.errors.password ? 'is-invalid' : ''
                      }`}
                      {...formik.getFieldProps('password')}
                    />
                  </div>
                </div>
                <div className="auth-field">
                  <label htmlFor="confirmPassword" className="auth-label">
                    {t('register.labels.confirmPassword')}
                  </label>
                  {formik.touched.confirmPassword && formik.errors.confirmPassword && (
                    <div className="auth-error">{formik.errors.confirmPassword}</div>
                  )}
                  <div className="register-field-row">
                    <MDBIcon fas icon="key" className="register-icon" />
                    <input
                      id="confirmPassword"
                      type="password"
                      className={`auth-input ${
                        formik.touched.confirmPassword && formik.errors.confirmPassword
                          ? 'is-invalid'
                          : ''
                      }`}
                      {...formik.getFieldProps('confirmPassword')}
                    />
                  </div>
                </div>
                <div className="auth-field">
                  <label htmlFor="dateOfBirth" className="auth-label">
                    {t('register.labels.dateOfBirth')}
                  </label>
                  {formik.touched.dateOfBirth && formik.errors.dateOfBirth && (
                    <div className="auth-error">{formik.errors.dateOfBirth}</div>
                  )}
                  <div className="register-field-row">
                    <MDBIcon fas icon="calendar-alt" className="register-icon" />
                    <input
                      id="dateOfBirth"
                      type="date"
                      className={`auth-input ${
                        formik.touched.dateOfBirth && formik.errors.dateOfBirth ? 'is-invalid' : ''
                      }`}
                      {...formik.getFieldProps('dateOfBirth')}
                    />
                  </div>
                </div>
                <div className="auth-field">
                  <label htmlFor="socialMediaUrl" className="auth-label">
                    {t('register.labels.socialMediaUrl')}
                  </label>
                  {formik.touched.socialMediaUrl && formik.errors.socialMediaUrl && (
                    <div className="auth-error">{formik.errors.socialMediaUrl}</div>
                  )}
                  <div className="register-field-row">
                    <MDBIcon fas icon="link" className="register-icon" />
                    <input
                      id="socialMediaUrl"
                      type="url"
                      className={`auth-input ${
                        formik.touched.socialMediaUrl && formik.errors.socialMediaUrl
                          ? 'is-invalid'
                          : ''
                      }`}
                      {...formik.getFieldProps('socialMediaUrl')}
                    />
                  </div>
                </div>
                <div className="auth-field">
                  <label htmlFor="image" className="auth-label">
                    {t('register.labels.image')}
                  </label>
                  {formik.touched.image && formik.errors.image && (
                    <div className="auth-error">{formik.errors.image as string}</div>
                  )}
                  <div className="register-field-row register-field-row-file">
                    <MDBIcon fas icon="image" className="register-icon" />
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
                  </div>
                </div>
                <div className="auth-field">
                  <label htmlFor="biography" className="auth-label">
                    {t('register.labels.biography')}
                  </label>
                  {formik.touched.biography && formik.errors.biography && (
                    <div className="auth-error">{formik.errors.biography}</div>
                  )}
                  <textarea
                    id="biography"
                    rows={4}
                    className={`form-control ${
                      formik.touched.biography && formik.errors.biography ? 'is-invalid' : ''
                    }`}
                    {...formik.getFieldProps('biography')}
                    placeholder={t('register.placeholders.biography')}
                  />
                </div>
                <div className="auth-field">
                  <div className="form-check">
                    <input
                      id="isPrivate"
                      type="checkbox"
                      className="form-check-input"
                      checked={formik.values.isPrivate}
                      onChange={(e) => formik.setFieldValue('isPrivate', e.target.checked)}
                    />
                    <label htmlFor="isPrivate" className="form-check-label auth-label ms-2">
                      {t('register.labels.isPrivate')}
                    </label>
                  </div>
                </div>
                <button
                  className="auth-submit-btn"
                  type="submit"
                  disabled={isSubmitting}
                  aria-busy={isSubmitting}
                >
                  {isSubmitting ? t('register.buttons.submitting') : t('register.buttons.submit')}
                </button>
              </form>
            </MDBCol>
            <MDBCol
              md="10"
              lg="6"
              className="order-1 order-lg-2 d-flex align-items-center justify-content-center"
            >
              <MDBCardImage src={image} alt="Register" className="register-image" />
            </MDBCol>
          </MDBRow>
        </MDBCardBody>
      </MDBCard>
    </MDBContainer>
  );
};

export default Register;
