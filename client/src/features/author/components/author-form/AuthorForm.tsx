import './AuthorForm.css';

import image from '@assets/create-author.jpg';
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

import type { AuthorDetails } from '@/features/author/types/author';
import { useAll } from '@/features/nationality/hooks/useCrud';

import { type AuthorFormValues, useAuthorFormik } from './formik/useAuthorFormik';
import GenderRadio from './gender-radio/GenderRadio';
import NationalitySearch from './nationality-search/NationalitySearch';

const AuthorForm: FC<{ authorData?: AuthorDetails | null; isEditMode?: boolean }> = ({
  authorData = null,
  isEditMode = false,
}) => {
  const formik = useAuthorFormik({ authorData, isEditMode });
  const { nationalities, isFetching } = useAll();

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
                    {isEditMode ? 'Edit Author' : 'Add a New Author to Support Our Community!'}
                  </h3>
                  <form onSubmit={formik.handleSubmit}>
                    <MDBCol md="12">
                      {formik.touched.name && formik.errors.name && (
                        <div className="text-danger mb-2">{formik.errors.name}</div>
                      )}
                      <MDBInput
                        wrapperClass="mb-4"
                        label="Name *"
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
                        label="Pen Name"
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
                      {formik.touched.imageUrl && formik.errors.imageUrl && (
                        <div className="text-danger mb-2">{formik.errors.imageUrl}</div>
                      )}
                      <MDBInput
                        wrapperClass="mb-4"
                        label="Image URL"
                        size="lg"
                        id="imageUrl"
                        type="text"
                        {...formik.getFieldProps('imageUrl')}
                        className={
                          formik.touched.imageUrl && formik.errors.imageUrl ? 'is-invalid' : ''
                        }
                      />
                    </MDBCol>
                    <MDBCol md="12">
                      {formik.touched.bornAt && formik.errors.bornAt && (
                        <div className="text-danger mb-2">{formik.errors.bornAt}</div>
                      )}
                      <MDBInput
                        wrapperClass="mb-4"
                        label="Date of Birth"
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
                        label="Date of Death (if applicable)"
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
                        placeholder="Write the author's biography here..."
                      />
                    </MDBCol>
                    <p className="text-danger fw-bold mt-2">Fields marked with * are required</p>
                    <div className="d-flex justify-content-end pt-3">
                      <MDBBtn color="light" size="lg" type="button" onClick={formik.handleReset}>
                        Reset All
                      </MDBBtn>
                      <MDBBtn
                        className="ms-2"
                        color="warning"
                        size="lg"
                        type="submit"
                        disabled={formik.isSubmitting}
                      >
                        {formik.isSubmitting
                          ? isEditMode
                            ? 'Saving...'
                            : 'Submitting...'
                          : isEditMode
                            ? 'Update Author'
                            : 'Submit Form'}
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
