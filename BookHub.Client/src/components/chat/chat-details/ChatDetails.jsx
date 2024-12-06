import { useContext, useState } from "react"
import { useParams, useNavigate, Link } from "react-router-dom"
import * as Yup from 'yup'
import { useFormik } from 'formik'
import {
    MDBCardImage,
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
import * as api from "../../../api/chatApi"
import { utcToLocal } from '../../../common/functions/utcToLocal'
import { useMessage } from '../../../contexts/messageContext'
import { routes } from "../../../common/constants/api"
import { UserContext } from "../../../contexts/userContext"

import DefaultSpinner from "../../common/default-spinner/DefaultSpinner"

import './ChatDetails.css'

export default function ChatDetails() {
    const { id } = useParams()
    const navigate = useNavigate()

    const { userId, token } = useContext(UserContext)
    const { showMessage } = useMessage()

    const { chat, isFetching, refetch } = useChat.useDetails(id)

    const createMessageHandler = useChat.useCreateMessage()
    const editMessageHandler = useChat.useEditMessage()

    const [isEditMode, setIsEditMode] = useState(false)
    const [messageToEdit, setMessageToEdit] = useState(null)

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
            message: ''
        },
        validationSchema,
        onSubmit: async (values) => {
            const messageData = { ...values, chatId: id }

            try {
                if (isEditMode && messageToEdit) {
                    await editMessageHandler(messageToEdit.id, { ...messageData })
                } else {
                    await createMessageHandler(messageData)
                }

                formik.resetForm()
                refetch()
                navigate(routes.chat + `/${chat.id}`)
            } catch {
                showMessage("Something went wrong while processing your message, please try again", false)
            } finally {
                setIsEditMode(false)
                setMessageToEdit(null)
            }
        }
    })

    const handleEditMessage = (message) => {
        setIsEditMode(true)
        setMessageToEdit(message)

        formik.setValues({
            chatId: id,
            message: message.message
        })
    }

    const handleCancelEdit = () => {
        setIsEditMode(false)
        setMessageToEdit(null)
        formik.resetForm() 
    }

    const handleDeleteMessage = async (id) => {
        try {
            await api.deleteMessageAsync(id, token)
            showMessage("Your message was successfuly deleted", true)
        } catch (error) {
            showMessage(error.message, false)
        } finally {
            refetch()
        }
    }

    const onProfileClickHandler = (profileId) => {
        navigate(routes.profile, { state: { id: profileId === userId ? null : profileId } })
    }

    if (isFetching) {
        return <DefaultSpinner />
    }

    return (
        <MDBContainer className="py-5 vh-100">
            <MDBRow className="d-flex justify-content-center h-100">
                <MDBCol md="10" lg="8" className="h-100">
                    <MDBRow className="h-100">
                        <MDBCol md="8" lg="8" className="h-100">
                            <MDBCard id="chat1" style={{ borderRadius: "15px", height: "100%", display: "flex", flexDirection: "column" }}>
                                <MDBCardHeader
                                    className="d-flex align-items-center bg-info text-white"
                                    style={{
                                        borderTopLeftRadius: "15px",
                                        borderTopRightRadius: "15px",
                                        flexDirection: "column",
                                        textAlign: "center",
                                    }}
                                >
                                    <MDBCardImage
                                        src={chat?.imageUrl}
                                        alt="Chat"
                                        style={{
                                            width: "80px",
                                            height: "80px",
                                            borderRadius: "50%",
                                            objectFit: "cover"
                                        }}
                                        className="mb-2"
                                    />
                                    <p className="mb-0 fw-bold">{chat?.name}</p>
                                </MDBCardHeader>
                                <MDBCardBody style={{ flex: 1, overflowY: "auto" }}>
                                    {chat?.messages.map(m => {
                                        const isSentByUser = m.senderId === userId
                                        const sender = chat.participants.find(p => p.id === m.senderId)

                                        return (
                                            <div
                                                key={m.id}
                                                className={`d-flex flex-row justify-content-${
                                                    isSentByUser ? "end" : "start"
                                                } mb-4`}
                                            >
                                                <div
                                                    className={`p-3 ${
                                                        isSentByUser ? "me-3 border" : "ms-3"
                                                    }`}
                                                    style={{
                                                        borderRadius: "15px",
                                                        backgroundColor: isSentByUser
                                                            ? "#fbfbfb"
                                                            : "rgba(57, 192, 237,.2)",
                                                    }}
                                                >
                                                    <p className="small mb-0">{m.message}</p>
                                                    <small className="text-muted">
                                                        {m.modifiedOn
                                                            ? utcToLocal(m.modifiedOn) + " (Modified)"
                                                            : utcToLocal(m.createdOn)}
                                                    </small>
                                                    {isSentByUser && (
                                                        <>
                                                            <MDBIcon
                                                                fas
                                                                icon="pencil-alt"
                                                                className="ms-2 cursor-pointer"
                                                                onClick={() => handleEditMessage(m)}
                                                            />
                                                            <MDBIcon
                                                                fas
                                                                icon="trash"
                                                                className="ms-2 cursor-pointer"
                                                                onClick={() =>
                                                                    handleDeleteMessage(m.id)
                                                                }
                                                            />
                                                        </>
                                                    )}
                                                </div>
                                                <img
                                                    src={
                                                        sender?.imageUrl ||
                                                        "https://mdbcdn.b-cdn.net/img/Photos/new-templates/bootstrap-chat/ava1-bg.webp"
                                                    }
                                                    alt="avatar"
                                                    style={{ width: "45px", height: "100%" }}
                                                />
                                                <div 
                                                    className="ms-2 profile-item"
                                                    onClick={() => onProfileClickHandler(sender.id)}
                                                >
                                                    <strong>
                                                        {sender?.firstName} {sender?.lastName}
                                                    </strong>
                                                </div>
                                            </div>
                                        )
                                    })}
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
                                </MDBCardBody>
                            </MDBCard>
                        </MDBCol>
                        <MDBCol md="4" lg="4" className="h-100">
                            <MDBCard
                                style={{
                                    borderRadius: "15px",
                                    height: "100%",
                                    display: "flex",
                                    flexDirection: "column",
                                }}
                            >
                                <MDBCardHeader
                                    className="bg-light text-center"
                                    style={{
                                        borderTopLeftRadius: "15px",
                                        borderTopRightRadius: "15px"
                                    }}
                                >
                                    <h6 className="mb-0">Participants</h6>
                                </MDBCardHeader>
                                <MDBCardBody style={{ flex: 1, overflowY: "auto" }}>
                                    <ul className="list-unstyled mb-0">
                                        {chat?.participants.map(p => (
                                            <li
                                                key={p.id}
                                                className="d-flex align-items-center mb-3 profile-item"
                                                onClick={() => onProfileClickHandler(p.id)}
                                            >
                                                <MDBCardImage
                                                    src={p.imageUrl}
                                                    alt={p.firstName}
                                                    style={{
                                                        width: "40px",
                                                        height: "40px",
                                                        borderRadius: "50%",
                                                        objectFit: "cover",
                                                        marginRight: "10px"
                                                    }}
                                                />
                                                <span>
                                                    {p.firstName} {p.lastName}
                                                </span>
                                            </li>
                                        ))}
                                    </ul>
                                </MDBCardBody>
                            </MDBCard>
                        </MDBCol>
                    </MDBRow>
                </MDBCol>
            </MDBRow>
        </MDBContainer>
    )
}
