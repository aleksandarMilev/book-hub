import './BookForm.css';

import {
  MDBCard,
  MDBCardBody,
  MDBCardImage,
  MDBCol,
  MDBContainer,
  MDBInput,
  MDBRow,
} from 'mdb-react-ui-kit';
import type { FC } from 'react';
import { useTranslation } from 'react-i18next';

import image from '@/features/book/components/form/assets/create-book.jpg';
import AuthorSearch from '@/features/book/components/form/author-search/AuthorSearch';
import { useBookFormik } from '@/features/book/components/form/formik/useBookFormik';
import GenreSearch from '@/features/book/components/form/genre-search/GenreSearch';
import type { BookDetails } from '@/features/book/types/book';

type Props = {
  bookData?: BookDetails | null;
  isEditMode?: boolean;
};

const BookForm: FC<Props> = ({ bookData = null, isEditMode = false }) => {
  const { t } = useTranslation('books');
  const {
    formik,
    authors,
    authorsLoading,
    genres,
    genresLoading,
    selectedGenres,
    setSelectedGenres,
  } = useBookFormik({ bookData, isEditMode });

  const submitLabel = formik.isSubmitting
    ? isEditMode
      ? t('form.buttons.submittingEdit')
      : t('form.buttons.submittingCreate')
    : isEditMode
      ? t('form.buttons.submitEdit')
      : t('form.buttons.submitCreate');

  return (
    <MDBContainer fluid className="book-form-container">
      <MDBRow className="d-flex justify-content-center align-items-center h-100">
        <MDBCol>
          <MDBCard className="my-4 book-form-card">
            <MDBRow className="g-0">
              <MDBCol md="6" className="book-form-image-col d-none d-md-block">
                <MDBCardImage src={image} alt="Book" className="book-form-image" fluid />
              </MDBCol>
              <MDBCol md="6">
                <MDBCardBody className="book-form-body text-black d-flex flex-column justify-content-center">
                  <h3 className="mb-5 fw-bold book-form-title">
                    {isEditMode ? t('form.titleEdit') : t('form.titleCreate')}
                  </h3>
                  <form onSubmit={formik.handleSubmit}>
                    <MDBRow>
                      <MDBCol md="12">
                        {formik.touched.title && formik.errors.title && (
                          <div className="text-danger mb-2">{formik.errors.title}</div>
                        )}
                        <label htmlFor="title" className="form-label book-form-image-label">
                          {t('form.labels.title')}
                        </label>
                        <MDBInput
                          wrapperClass="mb-4"
                          label=""
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
                              : t('validation.genres.min', {
                                  field: t('validation.fields.genres'),
                                })}
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
                        {formik.touched.image && formik.errors.image && (
                          <div className="text-danger mb-2">{formik.errors.image as string}</div>
                        )}
                        <label htmlFor="image" className="form-label book-form-image-label">
                          {t('form.labels.image')}
                        </label>
                        <input
                          id="image"
                          name="image"
                          type="file"
                          accept=".jpg,.jpeg,.png,.webp,.avif"
                          className={`form-control book-form-file-input ${
                            formik.touched.image && formik.errors.image ? 'is-invalid' : ''
                          }`}
                          onChange={(event) => {
                            const file = event.currentTarget.files?.[0] ?? null;
                            formik.setFieldValue('image', file);
                            formik.setFieldTouched('image', true, false);
                          }}
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
                        <label htmlFor="publishedDate" className="form-label book-form-image-label">
                          {t('form.labels.publishedDate')}
                        </label>
                        <MDBInput
                          wrapperClass="mb-4"
                          label=""
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
                        <label
                          htmlFor="shortDescription"
                          className="form-label book-form-image-label"
                        >
                          {t('form.labels.shortDescription')}
                        </label>
                        <MDBInput
                          wrapperClass="mb-4"
                          label=""
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
                        <label
                          htmlFor="longDescription"
                          className="form-label book-form-image-label"
                        >
                          {t('form.labels.longDescription')}
                        </label>
                        <textarea
                          id="longDescription"
                          rows={8}
                          {...formik.getFieldProps('longDescription')}
                          className={`form-control book-form-textarea ${
                            formik.touched.longDescription && formik.errors.longDescription
                              ? 'is-invalid'
                              : ''
                          }`}
                          placeholder={t('form.placeholders.longDescription')}
                        />
                      </MDBCol>
                    </MDBRow>

                    <p className="text-danger fw-bold mt-2 book-form-required-note">
                      {t('form.requiredNote')}
                    </p>
                    <div className="d-flex justify-content-end pt-3 book-form-actions">
                      <button
                        className="book-form-btn book-form-btn-light"
                        type="button"
                        onClick={formik.handleReset}
                      >
                        {t('form.buttons.reset')}
                      </button>

                      <button
                        className="book-form-btn book-form-btn-warning"
                        type="submit"
                        disabled={formik.isSubmitting}
                      >
                        {submitLabel}
                      </button>
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


