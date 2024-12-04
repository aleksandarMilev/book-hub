import { useNavigate } from 'react-router-dom'
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

import * as useArticle from '../../../hooks/useArticle'
import { routes } from '../../../common/constants/api'

export default function ArticleForm({ article = null, isEditMode = false }) {
    const navigate = useNavigate()

    const createHandler = useArticle.useCreate()
    const editHandler = useArticle.useEdit()

    const validationSchema = Yup.object({
        title: Yup
            .string()
            .min(10, 'Title must be at least 10 characters long')
            .max(100, 'Title must be less than 100 characters')
            .required('Title is required'),
        introduction: Yup
            .string()
            .min(10, 'Introduction must be at least 10 characters long')
            .max(500, 'Introduction must be less than 500 characters')
            .required('Introduction is required'),
        imageUrl: Yup
            .string()
            .url()
            .min(10, 'Image URL must be at least 10 characters long')
            .max(2000, 'Image URL must be less than 2 000 characters')
            .nullable(),
        content: Yup
            .string()
            .min(100, 'Content must be at least 100 characters long')
            .max(5000, 'Content must be less than 5 000 characters')
            .required('Content is required')
    })

    const formik = useFormik({
        initialValues: {
            title: article?.title || '',
            introduction: article?.introduction || '',
            imageUrl: article?.imageUrl || '',
            content: article?.content || ''
        },
        validationSchema,
        onSubmit: async (values) => {
            if (isEditMode) {
                await editHandler(article.id, values) 
            } else {
                await createHandler(values)
            }

            navigate(routes.home)
        }
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
                                                className={formik.touched.introduction && formik.errors.introduction ? 'is-invalid' : ''}
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
                                                className={formik.touched.imageUrl && formik.errors.imageUrl ? 'is-invalid' : ''}
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
