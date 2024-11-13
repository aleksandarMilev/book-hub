import { useState } from 'react'
import { useNavigate, useParams } from 'react-router-dom'
import * as Yup from 'yup'
import { useFormik } from 'formik'
import { Container, Row, Col, Card, Form, Button } from 'react-bootstrap'

import * as useBook from '../../../hooks/useBook'
import { routes } from '../../../common/constants/api'
import { UserContext } from '../../../contexts/userContext'

import DefaultSpinner from '../../common/default-spinner/DefaultSpinner'

export default function EditBook() {
    const { id } = useParams()
    const { userId } = useState(UserContext)
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
        book && book.userId === userId 
            ? 
            (isFetching ? (
                <div className="edit-book-spinner-container">
                    <DefaultSpinner />
                </div> 
            ) : (
                <Container fluid className="edit-book-container">
                    <Row className="justify-content-center">
                        <Col md={8}>
                            <Card className="edit-book-card">
                                <Card.Body>
                                    <h3 className="edit-book-title">Edit Book</h3>
                                    <Form onSubmit={formik.handleSubmit}>
                                        <Form.Group className="mb-3">
                                            <Form.Label className="edit-book-form-label">Title</Form.Label>
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
                                                <div className="edit-book-error">{formik.errors.title}</div>
                                            )}
                                        </Form.Group>
                                        <Form.Group className="mb-3">
                                            <Form.Label className="edit-book-form-label">Author</Form.Label>
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
                                                <div className="edit-book-error">{formik.errors.author}</div>
                                            )}
                                        </Form.Group>
                                        <Form.Group className="mb-3">
                                            <Form.Label className="edit-book-form-label">Image Url</Form.Label>
                                            <Form.Control
                                                as="textarea"
                                                placeholder="Enter the image URL"
                                                name="imageUrl" 
                                                value={formik.values.imageUrl}
                                                onChange={formik.handleChange}
                                                onBlur={formik.handleBlur}
                                            />
                                            {formik.touched.imageUrl && formik.errors.imageUrl && (
                                                <div className="edit-book-error">{formik.errors.imageUrl}</div>
                                            )}
                                        </Form.Group>
                                        <Form.Group className="mb-3">
                                            <Form.Label className="edit-book-form-label">Description</Form.Label>
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
                                                <div className="edit-book-error">{formik.errors.description}</div>
                                            )}
                                        </Form.Group>
                                        <div className="edit-book-submit-button">
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
            ) : (
                <div className="edit-book-alert-container">
                    <div className="alert alert-danger edit-book-alert" role="alert">
                        <h4 className="edit-book-alert-heading">Oops!</h4>
                        <p>The book you are looking for was not found.</p>
                    </div>
                </div>
            )
        )
}
