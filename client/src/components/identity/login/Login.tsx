import { type FC } from 'react';
import { Link } from 'react-router-dom';
import { MDBBtn, MDBContainer, MDBRow, MDBCol, MDBInput, MDBCheckbox } from 'mdb-react-ui-kit';
import { routes } from '../../../common/constants/api';
import loginImage from '../../../assets/images/login.webp';
import './Login.css';
import { useLoginFormik } from './formik/useLoginFormik';

const Login: FC = () => {
  const formik = useLoginFormik();

  return (
    <MDBContainer fluid className="p-3 my-5 h-custom">
      <MDBRow>
        <MDBCol col="10" md="6">
          <img
            src={loginImage}
            className="img-fluid"
            alt="Login"
            style={{ width: '100%', height: 'auto', display: 'block' }}
          />
        </MDBCol>
        <MDBCol col="4" md="6" style={{ marginTop: '3.4em' }}>
          <div className="d-flex flex-row align-items-center justify-content-center">
            <p className="lead fw-normal mb-3 me-3">Sign in</p>
          </div>
          <form onSubmit={formik.handleSubmit}>
            <div className="mb-4">
              {formik.touched.credentials && formik.errors.credentials && (
                <div className="text-danger mb-2">{formik.errors.credentials}</div>
              )}
              <MDBInput
                label="Username or Email"
                id="credentials"
                type="text"
                size="lg"
                {...formik.getFieldProps('credentials')}
              />
            </div>
            <div className="mb-4">
              {formik.touched.password && formik.errors.password && (
                <div className="text-danger mb-2">{formik.errors.password}</div>
              )}
              <MDBInput
                label="Password"
                id="password"
                type="password"
                size="lg"
                {...formik.getFieldProps('password')}
              />
            </div>
            <div className="d-flex justify-content-between mb-4">
              <MDBCheckbox
                id="rememberMe"
                name="rememberMe"
                label="Remember me"
                checked={formik.values.rememberMe}
                onChange={formik.handleChange}
                onBlur={formik.handleBlur}
              />
            </div>
            <div className="text-center text-md-start mt-4 pt-2">
              <MDBBtn className="mb-0 px-5" size="lg" type="submit">
                Login
              </MDBBtn>
              <p className="small fw-bold mt-2 pt-1 mb-2">
                Don't have an account?
                <Link to={routes.register} className="link-danger">
                  {' '}
                  Register
                </Link>
              </p>
            </div>
          </form>
        </MDBCol>
      </MDBRow>
    </MDBContainer>
  );
};

export default Login;
