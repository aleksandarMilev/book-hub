import './ArticleForm.css';

import { MDBCard, MDBCardBody, MDBCol, MDBContainer, MDBRow } from 'mdb-react-ui-kit';
import type { FC } from 'react';
import { useTranslation } from 'react-i18next';

import { useArticleFormik } from '@/features/article/components/form/formik/useArticleFormik.js';
import type { ArticleDetails } from '@/features/article/types/article.js';

type Props = {
  article?: ArticleDetails | null;
  isEditMode?: boolean;
};

const ArticleForm: FC<Props> = ({ article = null, isEditMode = false }) => {
  const { t } = useTranslation('articles');
  const formik = useArticleFormik({ article, isEditMode });

  const submitLabel = formik.isSubmitting
    ? isEditMode
      ? t('form.buttons.submittingEdit')
      : t('form.buttons.submittingCreate')
    : isEditMode
      ? t('form.buttons.submitEdit')
      : t('form.buttons.submitCreate');

  return (
    <div className="article-form-container">
      <MDBContainer fluid>
        <MDBRow className="justify-content-center">
          <MDBCol lg="8">
            <MDBCard className="article-form-card my-4">
              <MDBCardBody className="text-black">
                <h3 className="article-form-title">
                  {isEditMode ? t('form.titleEdit') : t('form.titleCreate')}
                </h3>
                <form onSubmit={formik.handleSubmit}>
                  <fieldset disabled={formik.isSubmitting}>
                    {formik.touched.title && formik.errors.title && (
                      <div className="article-form-error">{formik.errors.title}</div>
                    )}
                    <div className="mb-4 article-form-input">
                      <label htmlFor="title" className="form-label">
                        {t('form.labels.title')}
                      </label>
                      <input
                        id="title"
                        type="text"
                        className={`form-control form-control-lg ${
                          formik.touched.title && formik.errors.title ? 'is-invalid' : ''
                        }`}
                        {...formik.getFieldProps('title')}
                      />
                    </div>
                    {formik.touched.introduction && formik.errors.introduction && (
                      <div className="article-form-error">{formik.errors.introduction}</div>
                    )}
                    <div className="mb-4 article-form-input">
                      <label htmlFor="introduction" className="form-label">
                        {t('form.labels.introduction')}
                      </label>
                      <input
                        id="introduction"
                        type="text"
                        className={`form-control form-control-lg ${
                          formik.touched.introduction && formik.errors.introduction
                            ? 'is-invalid'
                            : ''
                        }`}
                        {...formik.getFieldProps('introduction')}
                      />
                    </div>
                    <div className="mb-4 article-form-input">
                      <label htmlFor="image" className="form-label">
                        {t('form.labels.image')}
                      </label>
                      <input
                        id="image"
                        name="image"
                        type="file"
                        accept=".jpg,.jpeg,.png,.webp,.avif"
                        className="form-control"
                        onChange={(event) => {
                          const file = event.currentTarget.files?.[0] ?? null;
                          formik.setFieldValue('image', file);
                          formik.setFieldTouched('image', true, false);
                        }}
                      />
                      {formik.touched.image && formik.errors.image && (
                        <div className="article-form-error">{formik.errors.image}</div>
                      )}
                    </div>
                    {formik.touched.content && formik.errors.content && (
                      <div className="article-form-error">{formik.errors.content}</div>
                    )}
                    <label htmlFor="content" className="form-label">
                      {t('form.labels.content')}
                    </label>
                    <textarea
                      id="content"
                      rows={18}
                      className={`form-control article-form-textarea ${
                        formik.touched.content && formik.errors.content ? 'is-invalid' : ''
                      }`}
                      {...formik.getFieldProps('content')}
                      placeholder={t('form.placeholders.content')}
                    />
                    <div className="article-form-actions">
                      <button
                        type="button"
                        className="article-form-button article-form-reset"
                        onClick={formik.handleReset}
                        disabled={formik.isSubmitting}
                      >
                        {t('form.buttons.reset')}
                      </button>
                      <button
                        type="submit"
                        className="article-form-button article-form-submit"
                        disabled={formik.isSubmitting}
                      >
                        {submitLabel}
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
