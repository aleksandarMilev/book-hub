import type { FC } from 'react';
import {
  MDBBtn,
  MDBContainer,
  MDBCard,
  MDBCardBody,
  MDBCardImage,
  MDBRow,
  MDBCol,
  MDBInput,
} from 'mdb-react-ui-kit';

import bookImage from '../../../assets/images/create-book.jpg';

import './BookForm.css';
import type { BookFormProps } from '../../../api/book/types/book';
import AuthorSearch from './author-search/AuthorSearch';
import GenreSearch from './genre-search/GenreSearch';
import { useBookFormik } from './formik/useBookFormik';

const BookForm: FC<BookFormProps> = ({ bookData = null, isEditMode = false }) => {
  const {
    formik,
    authors,
    authorsLoading,
    genres,
    genresLoading,
    selectedGenres,
    setSelectedGenres,
  } = useBookFormik({ bookData, isEditMode });

  return (
    <MDBContainer fluid className="book-form-container">
      <MDBRow className="d-flex justify-content-center align-items-center h-100">
        <MDBCol>
          <MDBCard className="my-4 book-form-card">
            <MDBRow className="g-0">
              <MDBCol md="6" className="book-form-image-col">
                <MDBCardImage src={bookImage} alt="Book" className="book-form-image" fluid />
              </MDBCol>
              <MDBCol md="6">
                <MDBCardBody className="text-black d-flex flex-column justify-content-center">
                  <h3 className="mb-5 fw-bold">
                    {isEditMode ? 'Edit Book' : 'Add a New Book to Our Collection!'}
                  </h3>
                  <form onSubmit={formik.handleSubmit}>
                    <MDBRow>
                      <MDBCol md="12">
                        {formik.touched.title && formik.errors.title && (
                          <div className="text-danger mb-2">{formik.errors.title}</div>
                        )}
                        <MDBInput
                          wrapperClass="mb-4"
                          label="Title *"
                          size="lg"
                          id="title"
                          type="text"
                          {...formik.getFieldProps('title')}
                          className={
                            formik.touched.title && formik.errors.title ? 'is-invalid' : ''
                          }
                        />
                      </MDBCol>
                    </MDBRow>
                    <MDBRow>
                      <MDBCol md="12">
                        <AuthorSearch authors={authors} loading={authorsLoading} formik={formik} />
                      </MDBCol>
                    </MDBRow>
                    <MDBRow>
                      <MDBCol md="12">
                        {formik.touched.genres && formik.errors.genres && (
                          <div className="text-danger mb-2">
                            {typeof formik.errors.genres === 'string'
                              ? formik.errors.genres
                              : 'Please select at least one genre'}
                          </div>
                        )}
                        <GenreSearch
                          genres={genres}
                          loading={genresLoading}
                          formik={formik}
                          selectedGenres={selectedGenres}
                          setSelectedGenres={setSelectedGenres}
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
                    </MDBRow>
                    <MDBRow>
                      <MDBCol md="12">
                        {formik.touched.publishedDate && formik.errors.publishedDate && (
                          <div className="text-danger mb-2">
                            {formik.errors.publishedDate as string}
                          </div>
                        )}
                        <MDBInput
                          wrapperClass="mb-4"
                          label="Published Date"
                          size="lg"
                          id="publishedDate"
                          type="date"
                          {...formik.getFieldProps('publishedDate')}
                          className={
                            formik.touched.publishedDate && formik.errors.publishedDate
                              ? 'is-invalid'
                              : ''
                          }
                        />
                      </MDBCol>
                    </MDBRow>
                    <MDBRow>
                      <MDBCol md="12">
                        {formik.touched.shortDescription && formik.errors.shortDescription && (
                          <div className="text-danger mb-2">{formik.errors.shortDescription}</div>
                        )}
                        <MDBInput
                          wrapperClass="mb-4"
                          label="Short Description *"
                          size="lg"
                          id="shortDescription"
                          type="text"
                          {...formik.getFieldProps('shortDescription')}
                          className={
                            formik.touched.shortDescription && formik.errors.shortDescription
                              ? 'is-invalid'
                              : ''
                          }
                        />
                      </MDBCol>
                    </MDBRow>
                    <MDBRow>
                      <MDBCol md="12">
                        {formik.touched.longDescription && formik.errors.longDescription && (
                          <div className="text-danger mb-2">{formik.errors.longDescription}</div>
                        )}
                        <textarea
                          id="longDescription"
                          rows={10}
                          {...formik.getFieldProps('longDescription')}
                          className={`form-control ${
                            formik.touched.longDescription && formik.errors.longDescription
                              ? 'is-invalid'
                              : ''
                          }`}
                          placeholder="Write the book's full description here... *"
                        />
                      </MDBCol>
                    </MDBRow>
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
                        {isEditMode
                          ? formik.isSubmitting
                            ? 'Saving...'
                            : 'Update Book'
                          : formik.isSubmitting
                            ? 'Submitting...'
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

export default BookForm;
