import './Login.css';

import { MDBCheckbox, MDBCol, MDBContainer, MDBRow } from 'mdb-react-ui-kit';
import type { FC } from 'react';
import { Link } from 'react-router-dom';

import image from '@/features/identity/components/login/assets/login.webp';
import { routes } from '@/shared/lib/constants/api';

import { useLoginPage } from '../../hooks/useLoginPage.js';

const Login: FC = () => {
  const { t, formik } = useLoginPage();

  return (
    <MDBContainer fluid className="identity-auth login-page">
      <MDBRow className="login-row">
        <MDBCol md="6" className="login-image-wrapper">
          <img src={image} className="login-image" alt="Login" />
        </MDBCol>
        <MDBCol md="6" className="login-form-wrapper">
          <div className="login-form-card">
            <h2 className="login-title">{t('login.title')}</h2>
            <form onSubmit={formik.handleSubmit} noValidate>
              <div className="auth-field">
                <label htmlFor="credentials" className="auth-label">
                  {t('login.labels.credentials')}
                </label>
                {formik.touched.credentials && formik.errors.credentials && (
                  <div className="auth-error">{formik.errors.credentials}</div>
                )}
                <input
                  id="credentials"
                  type="text"
                  className={`auth-input ${
                    formik.touched.credentials && formik.errors.credentials ? 'is-invalid' : ''
                  }`}
                  {...formik.getFieldProps('credentials')}
                />
              </div>
              <div className="auth-field">
                <label htmlFor="password" className="auth-label">
                  {t('login.labels.password')}
                </label>
                {formik.touched.password && formik.errors.password && (
                  <div className="auth-error">{formik.errors.password}</div>
                )}
                <input
                  id="password"
                  type="password"
                  className={`auth-input ${
                    formik.touched.password && formik.errors.password ? 'is-invalid' : ''
                  }`}
                  {...formik.getFieldProps('password')}
                />
              </div>
              <div className="auth-remember">
                <MDBCheckbox
                  id="rememberMe"
                  name="rememberMe"
                  label={t('login.labels.rememberMe')}
                  checked={formik.values.rememberMe}
                  onChange={formik.handleChange}
                  onBlur={formik.handleBlur}
                />
              </div>
              <button
                className="auth-submit-btn"
                type="submit"
                disabled={formik.isSubmitting}
                aria-busy={formik.isSubmitting}
              >
                {t('login.buttons.submit')}
              </button>
              <p className="auth-secondary-text">
                <Link to="/forgot-password" className="auth-secondary-link">
                  {t('login.links.forgotPassword') ?? 'Forgot password?'}
                </Link>
              </p>
              <p className="auth-secondary-text">
                {t('login.links.noAccount')}{' '}
                <Link to={routes.register} className="auth-secondary-link">
                  {t('login.links.register')}
                </Link>
              </p>
            </form>
          </div>
        </MDBCol>
      </MDBRow>
    </MDBContainer>
  );
};

export default Login;
