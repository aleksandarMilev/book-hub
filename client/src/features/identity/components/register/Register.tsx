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

  return (
    <MDBContainer fluid className="register-page">
      <MDBCard className="text-black register-card m-5">
        <MDBCardBody className="register-body">
          <MDBRow className="align-items-center">
            <MDBCol
              md="10"
              lg="6"
              className="order-2 order-lg-1 d-flex flex-column align-items-center"
            >
              <h2 className="register-title">{t('register.title')}</h2>
              <form onSubmit={formik.handleSubmit} className="w-100" noValidate>
                <div className="auth-field">
                  <label htmlFor="username" className="auth-label">
                    {t('register.labels.username')}
                  </label>
                  {formik.touched.username && formik.errors.username && (
                    <div className="auth-error">{formik.errors.username}</div>
                  )}
                  <div className="register-field-row">
                    <MDBIcon fas icon="user" className="register-icon" />
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
                <button
                  className="auth-submit-btn"
                  type="submit"
                  disabled={formik.isSubmitting}
                  aria-busy={formik.isSubmitting}
                >
                  {formik.isSubmitting
                    ? t('register.buttons.submitting')
                    : t('register.buttons.submit')}
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
