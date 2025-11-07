import type { FormikProps } from 'formik';
import { MDBTextArea, MDBBtn } from 'mdb-react-ui-kit';
import type { FC } from 'react';

const SendForm: FC<{
  formik: FormikProps<{
    chatId: number | string;
    message: string;
  }>;
  isEditMode: boolean;
  handleCancelEdit: () => void;
}> = ({ formik, isEditMode, handleCancelEdit }) => {
  const hasError = formik.touched.message && !!formik.errors.message;

  return (
    <form onSubmit={formik.handleSubmit}>
      {isEditMode && (
        <div className="alert alert-warning">
          You are editing a message.{' '}
          <span className="cancel-button" onClick={handleCancelEdit}>
            Cancel
          </span>
        </div>
      )}
      <div className="mb-3">
        <MDBTextArea
          className={`form-outline ${hasError ? 'is-invalid' : ''}`}
          label="Type your message"
          id="textAreaExample"
          name="message"
          value={formik.values.message}
          onChange={formik.handleChange}
          rows={4}
        />
        {hasError && <div className="invalid-feedback d-block">{formik.errors.message}</div>}
      </div>
      <MDBBtn
        type="submit"
        color="primary"
        className="mt-3"
        disabled={formik.isSubmitting || !formik.isValid}
      >
        {isEditMode ? 'Update Message' : 'Send Message'}
      </MDBBtn>
    </form>
  );
};

export default SendForm;
