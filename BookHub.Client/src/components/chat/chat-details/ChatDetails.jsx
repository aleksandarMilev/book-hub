import { useContext } from "react"
import { useParams, useNavigate } from "react-router-dom"
import * as Yup from 'yup'
import { useFormik } from 'formik'
import {
    MDBContainer,
    MDBRow,
    MDBCol,
    MDBCard,
    MDBCardHeader,
    MDBCardBody,
    MDBIcon,
    MDBTextArea,
    MDBBtn
} from "mdb-react-ui-kit"

import * as useChat from "../../../hooks/useChat"
import { utcToLocal } from '../../../common/functions/utcToLocal'
import { useMessage } from '../../../contexts/messageContext'
import { routes } from "../../../common/constants/api"
import { UserContext } from "../../../contexts/userContext"

import DefaultSpinner from "../../common/default-spinner/DefaultSpinner"

import './ChatDetails.css'

export default function ChatDetails() {
    const { id } = useParams()
    const navigate = useNavigate()

    const { userId } = useContext(UserContext)
    const { showMessage } = useMessage()

    const { chat, isFetching, refetch } = useChat.useDetails(id)

    const createMessageHandler = useChat.useCreateMessage()
    const editMessageHandler = useChat.useEditMessage()

    const isEditMode = false
    const messageToEdit = null

    const validationSchema = Yup.object({
        message: Yup
            .string()
            .min(1)
            .max(5000)
            .required('Message is required!')
    })

    const formik = useFormik({
        initialValues: {
            chatId: id,
            message: isEditMode ? messageToEdit?.message || '' : ''
        },
        validationSchema,
        onSubmit: async (values) => {
            const messageData = { ...values, chatId: id }
            
            try {
                if (isEditMode) {
                    await editMessageHandler(chat.id, { ...messageData })
                } else {
                    await createMessageHandler(messageData)
                }
                
                navigate(routes.chat +  `/${chat.id}`)
            } catch {
                showMessage("Something went wrong while processing your message, please try again", false)
            } finally {
                formik.resetForm()
                refetch()
            }
        }
    })

    if (isFetching) {
        return <DefaultSpinner />
    }

    return (
        <MDBContainer className="py-5">
            <MDBRow className="d-flex justify-content-center">
                <MDBCol md="8" lg="6" xl="4">
                    <MDBCard id="chat1" style={{ borderRadius: "15px" }}>
                        <MDBCardHeader
                            className="d-flex justify-content-between align-items-center p-3 bg-info text-white border-bottom-0"
                            style={{
                                borderTopLeftRadius: "15px",
                                borderTopRightRadius: "15px"
                            }}
                        >
                            <MDBIcon fas icon="angle-left" />
                            <p className="mb-0 fw-bold">{chat?.name}</p>
                            <MDBIcon fas icon="times" />
                        </MDBCardHeader>
                        <MDBCardBody>
                            {chat?.messages.map(m => {
                                const isSentByUser = m.senderId === userId
    
                                const sender = chat.participants.find(p => p.id === m.senderId)
    
                                return (
                                    <div
                                        key={m.id}
                                        className={`d-flex flex-row justify-content-${isSentByUser ? 'end' : 'start'} mb-4`}
                                    >
                                        <div
                                            className={`p-3 ${isSentByUser ? 'me-3 border' : 'ms-3'}`}
                                            style={{
                                                borderRadius: "15px",
                                                backgroundColor: isSentByUser ? "#fbfbfb" : "rgba(57, 192, 237,.2)",
                                            }}
                                        >
                                            <p className="small mb-0">{m.message}</p>
                                            <small className="text-muted">
                                                {
                                                    utcToLocal(m.createdOn)
                                                }
                                            </small>
                                        </div>
                                        <img
                                            src={sender?.imageUrl || "https://mdbcdn.b-cdn.net/img/Photos/new-templates/bootstrap-chat/ava1-bg.webp"}
                                            alt="avatar"
                                            style={{ width: "45px", height: "100%" }}
                                        />
                                        <div className="ms-2">
                                            <strong>{sender?.firstName} {sender?.lastName}</strong>
                                        </div>
                                    </div>
                                )
                            })}
    
                            <form onSubmit={formik.handleSubmit}>
                                <MDBTextArea
                                    className="form-outline"
                                    label="Type your message"
                                    id="textAreaExample"
                                    name="message"
                                    value={formik.values.message}
                                    onChange={formik.handleChange}
                                    rows={4}
                                    isInvalid={formik.touched.message && formik.errors.message}
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
                        </MDBCardBody>
                    </MDBCard>
                </MDBCol>
            </MDBRow>
        </MDBContainer>
    )
}
