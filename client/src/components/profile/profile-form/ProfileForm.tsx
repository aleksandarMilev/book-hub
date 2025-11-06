import type { FC } from 'react';
import {
  MDBBtn,
  MDBContainer,
  MDBCard,
  MDBCardBody,
  MDBRow,
  MDBCol,
  MDBInput,
} from 'mdb-react-ui-kit';
import { useProfileFormik } from './formik/useProfileFormik';
import type { Profile } from '../../../api/profile/types/profile';
import './ProfileForm.css';

const ProfileForm: FC<{ profile?: Profile | null; isEditMode?: boolean }> = ({
  profile = null,
  isEditMode = false,
}) => {
  const formik = useProfileFormik({ profile, isEditMode });

  return (
    <div className="profile-form-container">
      <MDBContainer fluid>
        <MDBRow className="form-row">
          <MDBCol className="form-col">
            <MDBCard className="my-4 profile-details-card">
              <MDBCardBody className="text-black">
                <h3 className="mb-5 fw-bold">
                  {isEditMode ? 'Edit Your Profile' : 'Create Your Profile'}
                </h3>
                <form onSubmit={formik.handleSubmit}>
                  <MDBRow>
                    <MDBCol md="12">
                      {formik.touched.firstName && formik.errors.firstName && (
                        <div className="text-danger mb-2">{formik.errors.firstName}</div>
                      )}
                      <MDBInput
                        wrapperClass="mb-4"
                        label="First Name *"
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
                        label="Last Name *"
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
                      {formik.touched.imageUrl && formik.errors.imageUrl && (
                        <div className="text-danger mb-2">{formik.errors.imageUrl}</div>
                      )}
                      <MDBInput
                        wrapperClass="mb-4"
                        label="Image URL *"
                        size="lg"
                        id="imageUrl"
                        type="text"
                        value={formik.values.imageUrl ?? ''}
                        onChange={(e) => formik.setFieldValue('imageUrl', e.target.value || null)}
                        onBlur={formik.handleBlur}
                        className={
                          formik.touched.imageUrl && formik.errors.imageUrl ? 'is-invalid' : ''
                        }
                      />
                    </MDBCol>
                  </MDBRow>
                  <MDBRow>
                    <MDBCol md="12">
                      {formik.touched.phoneNumber && formik.errors.phoneNumber && (
                        <div className="text-danger mb-2">{formik.errors.phoneNumber}</div>
                      )}
                      <MDBInput
                        wrapperClass="mb-4"
                        label="Phone Number *"
                        size="lg"
                        id="phoneNumber"
                        type="text"
                        {...formik.getFieldProps('phoneNumber')}
                        className={
                          formik.touched.phoneNumber && formik.errors.phoneNumber
                            ? 'is-invalid'
                            : ''
                        }
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
                        label="Date of Birth *"
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
                        label="Social Media URL"
                        size="lg"
                        id="socialMediaUrl"
                        type="text"
                        value={formik.values.socialMediaUrl ?? ''}
                        onChange={(e) =>
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
                        className={`form-control ${formik.touched.biography && formik.errors.biography ? 'is-invalid' : ''}`}
                        placeholder="Write a short biography... (Optional)"
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
                      Keep my profile private
                    </label>
                  </div>
                  <p className="text-danger fw-bold mt-2">Fields marked with * are required</p>
                  <div className="d-flex justify-content-end pt-3">
                    <MDBBtn color="light" size="lg" type="button" onClick={formik.handleReset}>
                      Reset All
                    </MDBBtn>
                    <MDBBtn className="ms-2" color="warning" size="lg" type="submit">
                      {formik.isSubmitting
                        ? isEditMode
                          ? 'Saving...'
                          : 'Submitting...'
                        : isEditMode
                          ? 'Update Profile'
                          : 'Create Profile'}
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
