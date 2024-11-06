import { useNavigate, useParams } from 'react-router-dom'
import * as Yup from 'yup'
import { useFormik } from 'formik'
import { Container, Row, Col, Card, Form, Button } from 'react-bootstrap'

import * as useBook from '../../hooks/useBook'
import { routes } from '../../common/constants/api'

import DefaultSpinner from '../common/DefaultSpinner'

export default function EditBook() {
    const { id } = useParams()
    const { book, isFetching } = useBook.useGetDetails(id)
    
    const navigate = useNavigate()
    const editHandler = useBook.useEdit()

    const validationSchema = Yup.object({
        title: Yup.string().required('Title is required!'),
        author: Yup.string().required('Author is required!'),
        description: Yup.string().required('Description is required!'),
        imageUrl: Yup.string().required('Image Url is required!')
    })

    const formik = useFormik({
        initialValues: {
            title: book?.title || '',
            author: book?.author || '',
            description: book?.description || '',
            imageUrl: book?.imageUrl || ''
        },
        enableReinitialize: true, 
        validationSchema,
        onSubmit: async (values, { setErrors }) => {
            try {
                await editHandler(id, values.title, values.author, values.imageUrl, values.description)
                navigate(routes.books + `/${id}`)
            } catch (error) {
                setErrors({ title: error.message })
            }
        }
    })

    return (
        isFetching ? (
                <div className="d-flex justify-content-center align-items-center" style={{ minHeight: '50vh' }}>
                    <DefaultSpinner />
                </div> 
            ) : (
                <Container fluid className="mt-5">
                    <Row className="justify-content-center">
                        <Col md={8}>
                            <Card className="shadow-lg">
                                <Card.Body>
                                    <h3 className="text-center mb-4">Edit Book</h3>
                                    <Form onSubmit={formik.handleSubmit}>
                                        <Form.Group className="mb-3">
                                            <Form.Label>Title</Form.Label>
                                            <Form.Control
                                                type="text"
                                                placeholder="Enter book title"
                                                required
                                                name="title" 
                                                value={formik.values.title}
                                                onChange={formik.handleChange}
                                                onBlur={formik.handleBlur}
                                            />
                                            {formik.touched.title && formik.errors.title && (
                                                <div className="text-danger mb-3">{formik.errors.title}</div>
                                            )}
                                        </Form.Group>
                                        <Form.Group className="mb-3">
                                            <Form.Label>Author</Form.Label>
                                            <Form.Control
                                                type="text"
                                                placeholder="Enter author's name"
                                                required
                                                name="author" 
                                                value={formik.values.author}
                                                onChange={formik.handleChange}
                                                onBlur={formik.handleBlur}
                                            />
                                            {formik.touched.author && formik.errors.author && (
                                                <div className="text-danger mb-3">{formik.errors.author}</div>
                                            )}
                                        </Form.Group>
                                        <Form.Group className="mb-3">
                                            <Form.Label>Image Url</Form.Label>
                                            <Form.Control
                                                as="textarea"
                                                placeholder="Enter the image URL"
                                                name="imageUrl" 
                                                value={formik.values.imageUrl}
                                                onChange={formik.handleChange}
                                                onBlur={formik.handleBlur}
                                            />
                                            {formik.touched.imageUrl && formik.errors.imageUrl && (
                                                <div className="text-danger mb-3">{formik.errors.imageUrl}</div>
                                            )}
                                        </Form.Group>
                                        <Form.Group className="mb-3">
                                            <Form.Label>Description</Form.Label>
                                            <Form.Control
                                                as="textarea"
                                                rows={7}
                                                placeholder="Enter book description"
                                                name="description" 
                                                value={formik.values.description}
                                                onChange={formik.handleChange}
                                                onBlur={formik.handleBlur}
                                            />
                                            {formik.touched.description && formik.errors.description && (
                                                <div className="text-danger mb-3">{formik.errors.description}</div>
                                            )}
                                        </Form.Group>
                                        <div className="text-center">
                                            <Button variant="primary" type="submit">
                                                Edit
                                            </Button>
                                        </div>
                                    </Form>
                                </Card.Body>
                            </Card>
                        </Col>
                    </Row>
                </Container>
        )
    )
}
