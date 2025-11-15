import {
  MDBBtn,
  MDBCard,
  MDBCardBody,
  MDBCol,
  MDBContainer,
  MDBInput,
  MDBRow,
} from 'mdb-react-ui-kit';
import type { FC } from 'react';

import { useArticleFormik } from '@/features/article/components/form/formik/useArticleFormik.js';
import type { ArticleDetails } from '@/features/article/types/article.js';

const ArticleForm: FC<{
  article?: ArticleDetails | null;
  isEditMode?: boolean;
}> = ({ article = null, isEditMode = false }) => {
  const formik = useArticleFormik({ article, isEditMode });

  return (
    <div className="article-form-container">
      <MDBContainer fluid>
        <MDBRow className="form-row">
          <MDBCol className="form-col">
            <MDBCard className="my-4">
              <MDBCardBody className="text-black">
                <h3 className="mb-5 fw-bold">
                  {isEditMode ? 'Edit Article' : 'Create New Article'}
                </h3>
                <form onSubmit={formik.handleSubmit}>
                  <fieldset disabled={formik.isSubmitting}>
                    <MDBCol md="12">
                      {formik.touched.title && formik.errors.title && (
                        <div className="text-danger mb-2">{formik.errors.title}</div>
                      )}
                      <MDBInput
                        wrapperClass="mb-4"
                        label="Article Title *"
                        size="lg"
                        id="title"
                        type="text"
                        {...formik.getFieldProps('title')}
                        className={formik.touched.title && formik.errors.title ? 'is-invalid' : ''}
                      />
                    </MDBCol>
                    <MDBCol md="12">
                      {formik.touched.introduction && formik.errors.introduction && (
                        <div className="text-danger mb-2">{formik.errors.introduction}</div>
                      )}
                      <MDBInput
                        wrapperClass="mb-4"
                        label="Article Introduction *"
                        size="lg"
                        id="introduction"
                        type="text"
                        {...formik.getFieldProps('introduction')}
                        className={
                          formik.touched.introduction && formik.errors.introduction
                            ? 'is-invalid'
                            : ''
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
                      {formik.touched.content && formik.errors.content && (
                        <div className="text-danger mb-2">{formik.errors.content}</div>
                      )}
                      <label htmlFor="content" className="form-label">
                        <textarea
                          id="content"
                          rows={20}
                          {...formik.getFieldProps('content')}
                          className={`form-control ${
                            formik.touched.content && formik.errors.content ? 'is-invalid' : ''
                          }`}
                          placeholder="Write the content of your article here... *"
                        />
                      </label>
                    </MDBCol>
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
                            ? 'Save Changes'
                            : 'Submit Article'}
                      </MDBBtn>
                    </div>
                  </fieldset>
                </form>
              </MDBCardBody>
            </MDBCard>
          </MDBCol>
        </MDBRow>
      </MDBContainer>
    </div>
  );
};

export default ArticleForm;
