import { useNavigate } from 'react-router-dom'
import * as Yup from 'yup'
import { useFormik } from 'formik'

import { useMessage } from '../../../contexts/messageContext'
import { routes } from '../../../common/constants/api'
import * as useChat from '../../../hooks/useChat'

import image from '../../../assets/images/chat.avif'

import './ChatForm.css'

export default function ChatForm({ chatData = null, isEditMode = false }) {
    const navigate = useNavigate()

    const createHandler = useChat.useCreate()
    const editHandler = useChat.useEdit()

    const { showMessage } = useMessage()

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
        onSubmit: async (values) => {
            try {
                if (isEditMode) {
                    const isSuccessfullyEdited = await editHandler(chatData.id, { ...values }) 

                    if(isSuccessfullyEdited){
                        showMessage(`You have successfuly edited ${values.name}`, true)
                        navigate(routes.chat + `/${chatData.id}`)
                    }
                } else {
                    const chatId = await createHandler(values)

                    if (chatId) {
                        showMessage(`You have successfuly created ${values.name}`, true)
                        navigate(routes.chat + `/${chatId}`)
                    }
                }
            } catch (error) {
                showMessage(`Something went wrong while creating your chat, please try again!`, false)
            }
        }
    })

    return (
        <div className="chat-form-container">
            <div className="chat-form-card">
                <div className="chat-form-image">
                    <img src={image} alt="Chat Illustration" />
                </div>
                <h2 className="chat-form-title">
                    {isEditMode ? 'Edit Chat' : 'Create New Chat'}
                </h2>
                <form className="chat-form" onSubmit={formik.handleSubmit}>
                    <div className="form-group">
                        <label htmlFor="name">Name</label>
                        <input
                            type="text"
                            id="name"
                            name="name"
                            placeholder="Enter chat name"
                            value={formik.values.name}
                            onChange={formik.handleChange}
                            onBlur={formik.handleBlur}
                            className={`form-input ${formik.touched.name && formik.errors.name ? 'invalid' : ''}`}
                        />
                        {formik.touched.name && formik.errors.name && (
                            <div className="error-message">{formik.errors.name}</div>
                        )}
                    </div>
                    <div className="form-group">
                        <label htmlFor="imageUrl">Image URL</label>
                        <input
                            type="text"
                            id="imageUrl"
                            name="imageUrl"
                            placeholder="Enter chat image URL (optional)"
                            value={formik.values.imageUrl}
                            onChange={formik.handleChange}
                            onBlur={formik.handleBlur}
                            className={`form-input ${formik.touched.imageUrl && formik.errors.imageUrl ? 'invalid' : ''}`}
                        />
                        {formik.touched.imageUrl && formik.errors.imageUrl && (
                            <div className="error-message">{formik.errors.imageUrl}</div>
                        )}
                    </div>
                    <button type="submit" className="form-submit-btn">
                        {isEditMode ? 'Update Chat' : 'Create Chat'}
                    </button>
                    {formik.errors.submit && (
                        <div className="error-message form-submit-error">
                            {formik.errors.submit}
                        </div>
                    )}
                </form>
            </div>
        </div>
    )
}
