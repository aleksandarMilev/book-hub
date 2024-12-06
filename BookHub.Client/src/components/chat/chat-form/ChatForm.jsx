import { useNavigate } from 'react-router-dom'
import * as Yup from 'yup'
import { useFormik } from 'formik'
import { 
    Col, 
    Button, 
    Row, 
    Card, 
    Form, 
    Container 
} from 'react-bootstrap'

import { routes } from '../../../common/constants/api'
import * as useChat from '../../../hooks/useChat'

import './ChatForm.css'

export default function ChatForm({ chatData = null, isEditMode = false }) {
    const navigate = useNavigate()

    const createHandler = useChat.useCreate()
    const editHandler = useChat.useEdit()

    const validationSchema = Yup.object({
        name: Yup
            .string()
            .min(1)
            .max(200)
            .required('Name is required!'),
        imageUrl: Yup
            .string()
            .url()
            .min(10)
            .max(2000)
            .nullable()
    })

    const formik = useFormik({
        initialValues: {
            name: chatData?.name || '',
            imageUrl: chatData?.imageUrl || ''
        },
        validationSchema,
        onSubmit: async (values, { setErrors }) => {
            try {
                if (isEditMode) {
                    const isSuccessfullyEdited = await editHandler(chatData.id, { ...values }) 

                    if(isSuccessfullyEdited){
                        // navigate(routes.chat + `/${chatData.id}`)
                        navigate(routes.home)
                    }
                } else {
                    const chatId = await createHandler(values)

                    if (chatId) {
                        // navigate(routes.chat + `/${chatId}`)
                        navigate(routes.home)
                    }
                }
            } catch (error) {
                setErrors({ submit: 'Something went wrong, please try again!' })
            }
        }
    })

    return (
        <Container>
          <Row className="vh-100 d-flex justify-content-center align-items-center">
            <Col md={8} lg={6} xs={12}>
              <Card className="shadow">
                <Card.Body>
                  <h2 className="fw-bold text-uppercase mb-4 text-center">
                    {isEditMode ? 'Edit Chat' : 'Create New Chat'}
                  </h2>
                  <Form onSubmit={formik.handleSubmit}>
                    <Form.Group className="mb-3" controlId="name">
                      <Form.Label>Name</Form.Label>
                      <Form.Control
                        type="text"
                        placeholder="Enter chat name"
                        value={formik.values.name}
                        onChange={formik.handleChange}
                        onBlur={formik.handleBlur}
                        isInvalid={formik.touched.name && formik.errors.name}
                      />
                      {formik.touched.name && formik.errors.name && (
                        <Form.Control.Feedback type="invalid">
                          {formik.errors.name}
                        </Form.Control.Feedback>
                      )}
                    </Form.Group>

                    <Form.Group className="mb-3" controlId="imageUrl">
                      <Form.Label>Image URL</Form.Label>
                      <Form.Control
                        type="text"
                        placeholder="Enter chat image URL (optional)"
                        value={formik.values.imageUrl}
                        onChange={formik.handleChange}
                        onBlur={formik.handleBlur}
                        isInvalid={formik.touched.imageUrl && formik.errors.imageUrl}
                      />
                      {formik.touched.imageUrl && formik.errors.imageUrl && (
                        <Form.Control.Feedback type="invalid">
                          {formik.errors.imageUrl}
                        </Form.Control.Feedback>
                      )}
                    </Form.Group>
                    <div className="d-grid mt-3">
                      <Button variant="primary" type="submit">
                        {isEditMode ? 'Update Chat' : 'Create Chat'}
                      </Button>
                    </div>

                    {formik.errors.submit && (
                      <div className="text-danger mt-3 text-center">
                        {formik.errors.submit}
                      </div>
                    )}
                  </Form>
                </Card.Body>
              </Card>
            </Col>
          </Row>
        </Container>
    )
}
