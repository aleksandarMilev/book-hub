import './ArticleForm.css';

import { MDBCard, MDBCardBody, MDBCol, MDBContainer, MDBInput, MDBRow } from 'mdb-react-ui-kit';
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
        <MDBRow className="justify-content-center">
          <MDBCol lg="8">
            <MDBCard className="article-form-card my-4">
              <MDBCardBody className="text-black">
                <h3 className="article-form-title">
                  {isEditMode ? 'Edit Article' : 'Create New Article'}
                </h3>
                <form onSubmit={formik.handleSubmit}>
                  <fieldset disabled={formik.isSubmitting}>
                    {formik.touched.title && formik.errors.title && (
                      <div className="article-form-error">{formik.errors.title}</div>
                    )}
                    <MDBInput
                      wrapperClass="mb-4 article-form-input"
                      label="Article Title *"
                      size="lg"
                      id="title"
                      type="text"
                      {...formik.getFieldProps('title')}
                      className={formik.touched.title && formik.errors.title ? 'is-invalid' : ''}
                    />
                    {formik.touched.introduction && formik.errors.introduction && (
                      <div className="article-form-error">{formik.errors.introduction}</div>
                    )}
                    <MDBInput
                      wrapperClass="mb-4 article-form-input"
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
                    {formik.touched.imageUrl && formik.errors.imageUrl && (
                      <div className="article-form-error">{formik.errors.imageUrl}</div>
                    )}
                    <MDBInput
                      wrapperClass="mb-4 article-form-input"
                      label="Image URL"
                      size="lg"
                      id="imageUrl"
                      type="text"
                      {...formik.getFieldProps('imageUrl')}
                      className={
                        formik.touched.imageUrl && formik.errors.imageUrl ? 'is-invalid' : ''
                      }
                    />
                    {formik.touched.content && formik.errors.content && (
                      <div className="article-form-error">{formik.errors.content}</div>
                    )}
                    <textarea
                      id="content"
                      rows={18}
                      {...formik.getFieldProps('content')}
                      className={`form-control article-form-textarea ${
                        formik.touched.content && formik.errors.content ? 'is-invalid' : ''
                      }`}
                      placeholder="Write the content of your article here... *"
                    />
                    <div className="article-form-actions">
                      <button
                        type="button"
                        className="article-form-button article-form-reset"
                        onClick={formik.handleReset}
                        disabled={formik.isSubmitting}
                      >
                        Reset
                      </button>
                      <button
                        type="submit"
                        className="article-form-button article-form-submit"
                        disabled={formik.isSubmitting}
                      >
                        {formik.isSubmitting
                          ? isEditMode
                            ? 'Saving...'
                            : 'Submitting...'
                          : isEditMode
                            ? 'Save Changes'
                            : 'Submit Article'}
                      </button>
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
