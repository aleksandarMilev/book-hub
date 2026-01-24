import type { FormikProps } from 'formik';
import { MDBBtn, MDBTextArea } from 'mdb-react-ui-kit';
import type { FC } from 'react';
import { useTranslation } from 'react-i18next';

type Props = {
  formik: FormikProps<{
    chatId: string;
    message: string;
  }>;
  isEditMode: boolean;
  handleCancelEdit: () => void;
};

const SendForm: FC<Props> = ({ formik, isEditMode, handleCancelEdit }) => {
  const { t } = useTranslation('chats');
  const hasError = formik.touched.message && !!formik.errors.message;

  return (
    <form onSubmit={formik.handleSubmit} className="chat-send-form">
      {isEditMode && (
        <div className="chat-edit-alert alert alert-warning">
          {t('sendForm.editingAlert')}{' '}
          <span className="chat-cancel-button" onClick={handleCancelEdit}>
            {t('sendForm.cancel')}
          </span>
        </div>
      )}
      <div className="mb-3">
        <MDBTextArea
          className={`chat-send-textarea form-outline ${hasError ? 'is-invalid' : ''}`}
          label={t('sendForm.messageLabel')}
          name="message"
          value={formik.values.message}
          onChange={formik.handleChange}
          onBlur={formik.handleBlur}
          rows={4}
        />
        {hasError && (
          <div className="chat-invalid-feedback invalid-feedback d-block">
            {formik.errors.message}
          </div>
        )}
      </div>
      <MDBBtn
        type="submit"
        color="primary"
        className="chat-send-btn mt-3"
        disabled={formik.isSubmitting || (formik.submitCount > 0 && !formik.isValid)}
      >
        {isEditMode ? t('sendForm.buttonEdit') : t('sendForm.buttonSend')}
      </MDBBtn>
    </form>
  );
};

export default SendForm;
