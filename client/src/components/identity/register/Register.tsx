import {
  MDBBtn,
  MDBCard,
  MDBCardBody,
  MDBCardImage,
  MDBCol,
  MDBContainer,
  MDBIcon,
  MDBInput,
  MDBRow,
} from 'mdb-react-ui-kit';
import { type FC } from 'react';

import image from '@/assets/register.webp';
import { useRegisterFormik } from '@/components/identity/register/formik/useRegisterFormik';

const Register: FC = () => {
  const formik = useRegisterFormik();

  return (
    <MDBContainer fluid>
      <MDBCard className="text-black m-5" style={{ borderRadius: '25px' }}>
        <MDBCardBody>
          <MDBRow>
            <MDBCol
              md="10"
              lg="6"
              className="order-2 order-lg-1 d-flex flex-column align-items-center"
            >
              <p className="text-center h1 fw-bold mb-5 mx-1 mx-md-4 mt-4">Sign up</p>
              <form onSubmit={formik.handleSubmit} className="w-100">
                {formik.touched.username && formik.errors.username && (
                  <div className="text-danger mb-3">{formik.errors.username}</div>
                )}
                <div className="d-flex flex-row align-items-center mb-4">
                  <MDBIcon fas icon="user me-3" size="lg" />
                  <MDBInput
                    label="Your Name"
                    id="username"
                    type="text"
                    {...formik.getFieldProps('username')}
                    className="w-100"
                  />
                </div>
                {formik.touched.email && formik.errors.email && (
                  <div className="text-danger mb-3">{formik.errors.email}</div>
                )}
                <div className="d-flex flex-row align-items-center mb-4">
                  <MDBIcon fas icon="envelope me-3" size="lg" />
                  <MDBInput
                    label="Your Email"
                    id="email"
                    type="email"
                    {...formik.getFieldProps('email')}
                    className="w-100"
                  />
                </div>
                {formik.touched.password && formik.errors.password && (
                  <div className="text-danger mb-3">{formik.errors.password}</div>
                )}
                <div className="d-flex flex-row align-items-center mb-4">
                  <MDBIcon fas icon="lock me-3" size="lg" />
                  <MDBInput
                    label="Password"
                    id="password"
                    type="password"
                    {...formik.getFieldProps('password')}
                    className="w-100"
                  />
                </div>
                {formik.touched.confirmPassword && formik.errors.confirmPassword && (
                  <div className="text-danger mb-3">{formik.errors.confirmPassword}</div>
                )}
                <div className="d-flex flex-row align-items-center mb-4">
                  <MDBIcon fas icon="key me-3" size="lg" />
                  <MDBInput
                    label="Repeat your password"
                    id="confirmPassword"
                    type="password"
                    {...formik.getFieldProps('confirmPassword')}
                    className="w-100"
                  />
                </div>
                <MDBBtn className="mb-4" size="lg" type="submit" disabled={formik.isSubmitting}>
                  {formik.isSubmitting ? 'Registering...' : 'Register'}
                </MDBBtn>
              </form>
            </MDBCol>
            <MDBCol md="10" lg="6" className="order-1 order-lg-2 d-flex align-items-center">
              <MDBCardImage src={image} fluid alt="Register" />
            </MDBCol>
          </MDBRow>
        </MDBCardBody>
      </MDBCard>
    </MDBContainer>
  );
};

export default Register;
