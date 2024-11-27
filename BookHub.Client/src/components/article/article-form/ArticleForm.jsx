import * as Yup from 'yup'
import { useFormik } from 'formik'
import {
    MDBBtn,
    MDBContainer,
    MDBCard,
    MDBCardBody,
    MDBRow,
    MDBCol,
    MDBInput
} from 'mdb-react-ui-kit'

import './ArticleForm.css'

export default function ArticleForm() {
    const createHandler = (values) => {}

    const validationSchema = Yup.object({
        title: Yup
            .string()
            .min(3, 'Title must be at least 3 characters long')
            .max(100, 'Title must be less than 100 characters')
            .required('Title is required'),
        content: Yup
            .string()
            .min(20, 'Content must be at least 20 characters long')
            .required('Content is required')
    })

    const formik = useFormik({
        initialValues: {
            title: '',
            content: ''
        },
        validationSchema,
        onSubmit: createHandler
    })

    return (
        <div className="article-form-container">
            <MDBContainer fluid>
                <MDBRow className="form-row">
                    <MDBCol className="form-col">
                        <MDBCard className="my-4">
                            <MDBCardBody className="text-black">
                                <h3 className="mb-5 fw-bold">Create New Article</h3>
                                <form onSubmit={formik.handleSubmit}>
                                    <MDBRow>
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
                                    </MDBRow>
                                    <MDBRow>
                                        <MDBCol md="12">
                                            {formik.touched.content && formik.errors.content && (
                                                <div className="text-danger mb-2">{formik.errors.content}</div>
                                            )}
                                            <textarea
                                                id="content"
                                                rows="20" 
                                                {...formik.getFieldProps('content')}
                                                className={`form-control ${formik.touched.content && formik.errors.content ? 'is-invalid' : ''}`}
                                                placeholder="Write the content of your article here... *"
                                            />
                                        </MDBCol>
                                    </MDBRow>
                                    <div className="d-flex justify-content-end pt-3">
                                        <MDBBtn color="light" size="lg" onClick={formik.handleReset}>
                                            Reset All
                                        </MDBBtn>
                                        <MDBBtn className="ms-2" color="warning" size="lg" type="submit">
                                            Submit Article
                                        </MDBBtn>
                                    </div>
                                </form>
                            </MDBCardBody>
                        </MDBCard>
                    </MDBCol>
                </MDBRow>
            </MDBContainer>
        </div>
    )
}
