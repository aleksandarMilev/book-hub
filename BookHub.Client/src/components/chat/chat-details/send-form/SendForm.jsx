import {
    MDBTextArea,
    MDBBtn
} from "mdb-react-ui-kit"

export default function SendForm({formik, isEditMode, handleCancelEdit}){
    return(
        <form onSubmit={formik.handleSubmit}>
            {isEditMode && (
                <div className="alert alert-warning">
                    You are editing a message.{" "}
                    <span
                        className="cancel-button"
                        onClick={handleCancelEdit}
                    >
                        Cancel
                    </span>
                </div>
            )}
            <MDBTextArea
                className="form-outline"
                label="Type your message"
                id="textAreaExample"
                name="message"
                value={formik.values.message}
                onChange={formik.handleChange}
                rows={4}
                isInvalid={
                    formik.touched.message && formik.errors.message
                }
            />
            {formik.touched.message && formik.errors.message && (
                <div className="text-danger">{formik.errors.message}</div>
            )}
            <MDBBtn
                type="submit"
                color="primary"
                className="mt-3"
                disabled={formik.isSubmitting || !formik.isValid}
            >
                {isEditMode ? "Update Message" : "Send Message"}
            </MDBBtn>
        </form>
    )
} 