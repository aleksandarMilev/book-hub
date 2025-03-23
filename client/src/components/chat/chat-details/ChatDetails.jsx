import { useContext, useState, useEffect } from "react";
import { useParams, useNavigate } from "react-router-dom";
import * as Yup from "yup";
import { useFormik } from "formik";
import {
  MDBCardImage,
  MDBContainer,
  MDBRow,
  MDBCol,
  MDBCard,
  MDBCardHeader,
  MDBCardBody,
} from "mdb-react-ui-kit";

import * as api from "../../../api/chatApi";
import * as useChat from "../../../hooks/useChat";
import { useMessage } from "../../../contexts/messageContext";
import { routes } from "../../../common/constants/api";
import { UserContext } from "../../../contexts/userContext";

import Message from "./message/Message";
import SendForm from "./send-form/SendForm";
import ParticipantListItem from "./participant-list-item/ParticipantListItem";
import ChatButtons from "./chat-buttons/ChatButtons";
import DefaultSpinner from "../../common/default-spinner/DefaultSpinner";

import "./ChatDetails.css";

export default function ChatDetails() {
  const { id } = useParams();
  const navigate = useNavigate();

  const { userId, token } = useContext(UserContext);
  const { showMessage } = useMessage();

  const { chat, isFetching, refetch } = useChat.useDetails(id);

  const [isEditMode, setIsEditMode] = useState(false);
  const [messageToEdit, setMessageToEdit] = useState(null);

  const [messages, setMessages] = useState(chat?.messages || []);

  useEffect(() => {
    setMessages(chat?.messages || []);
  }, [chat]);

  const createMessageHandler = useChat.useCreateMessage();
  const editMessageHandler = useChat.useEditMessage();

  const deleteMessageHandler = async (id) => {
    try {
      await api.deleteMessageAsync(id, token);
      setMessages((prevMessages) =>
        prevMessages.filter((msg) => msg.id !== id)
      );
      showMessage("Your message was successfuly deleted", true);
    } catch (error) {
      showMessage(error.message, false);
    }
  };

  const [participants, setParticipants] = useState(chat?.participants || []);

  useEffect(() => {
    setParticipants(chat?.participants || []);
  }, [chat]);

  const refreshParticipantsList = (newParticipant) => {
    setParticipants((prev) => [...prev, newParticipant]);
  };

  const removeUserClickhandler = async (profileId, firstName) => {
    try {
      await api.removeUserAsync(id, profileId, token);
      setParticipants((prev) => prev.filter((p) => p.id !== profileId));
      showMessage(`You have successfully removed ${firstName}!`, true);
    } catch (error) {
      showMessage(error.message, false);
    }
  };

  const validationSchema = Yup.object({
    message: Yup.string().min(1).max(5000).required("Message is required!"),
  });

  const formik = useFormik({
    initialValues: {
      chatId: id,
      message: "",
    },
    validationSchema,
    onSubmit: async (values) => {
      const messageData = { ...values, chatId: id };

      try {
        let response;
        if (isEditMode && messageToEdit) {
          response = await editMessageHandler(messageToEdit.id, {
            ...messageData,
          });
          setMessages((prevMessages) =>
            prevMessages.map((msg) =>
              msg.id === response.id
                ? {
                    ...msg,
                    message: response.message,
                    createdOn: convertToLocalTime(response.createdOn),
                    modifiedOn: convertToLocalTime(response.modifiedOn),
                  }
                : msg
            )
          );
        } else {
          response = await createMessageHandler(messageData);
          response.createdOn = convertToLocalTime(response.createdOn);

          setMessages((prevMessages) => [...prevMessages, response]);
        }

        formik.resetForm();
      } catch {
        showMessage(
          "Something went wrong while processing your message, please try again",
          false
        );
      } finally {
        setIsEditMode(false);
        setMessageToEdit(null);
      }
    },
  });

  const handleEditMessage = (message) => {
    setIsEditMode(true);
    setMessageToEdit(message);

    formik.setValues({
      chatId: id,
      message: message.message,
    });
  };

  const handleCancelEdit = () => {
    setIsEditMode(false);
    setMessageToEdit(null);
    formik.resetForm();
  };

  const onProfileClickHandler = (profileId) => {
    navigate(routes.profile, {
      state: { id: profileId === userId ? null : profileId },
    });
  };

  if (isFetching) {
    return <DefaultSpinner />;
  }

  return (
    <>
      <ChatButtons
        chatName={chat?.name}
        chatCreatorId={chat?.creatorId}
        refreshParticipantsList={refreshParticipantsList}
      />
      <MDBContainer className="py-5 vh-100">
        <MDBRow className="d-flex justify-content-center h-100">
          <MDBCol md="10" lg="8" className="h-100">
            <MDBRow className="h-100">
              <MDBCol md="8" lg="8" className="h-100">
                <MDBCard id="chat1" className="chat-card">
                  <MDBCardHeader className="chat-card-header">
                    <MDBCardImage
                      src={chat?.imageUrl}
                      alt="Chat"
                      className="chat-card-header-img"
                    />
                    <p className="mb-0 fw-bold">{chat?.name}</p>
                  </MDBCardHeader>
                  <MDBCardBody className="chat-card-body">
                    {messages?.map((m) => {
                      const isSentByUser = m.senderId === userId;
                      const sender = chat.participants.find(
                        (p) => p.id === m.senderId
                      );

                      return (
                        <Message
                          key={m.id}
                          m={m}
                          isSentByUser={isSentByUser}
                          sender={sender}
                          onEdit={handleEditMessage}
                          onDelete={deleteMessageHandler}
                          onProfileClick={onProfileClickHandler}
                        />
                      );
                    })}
                    <SendForm
                      formik={formik}
                      isEditMode={isEditMode}
                      handleCancelEdit={handleCancelEdit}
                    />
                  </MDBCardBody>
                </MDBCard>
              </MDBCol>
              <MDBCol md="4" lg="4" className="h-100">
                <MDBCard className="participants-card">
                  <MDBCardHeader className="participants-card-header">
                    <h6 className="mb-0">Participants</h6>
                  </MDBCardHeader>
                  <MDBCardBody className="participants-card-body">
                    <ul className="participants-list">
                      {participants
                        .sort((a, b) =>
                          a.id === chat?.creatorId
                            ? -1
                            : b.id === chat?.creatorId
                            ? 1
                            : 0
                        )
                        .map((p, i) => (
                          <ParticipantListItem
                            key={p.id}
                            participant={p}
                            index={i}
                            onProfileClickHandler={onProfileClickHandler}
                            onDeleteHandler={removeUserClickhandler}
                            currentUserIsChatCreator={
                              userId === chat?.creatorId
                            }
                          />
                        ))}
                    </ul>
                  </MDBCardBody>
                </MDBCard>
              </MDBCol>
            </MDBRow>
          </MDBCol>
        </MDBRow>
      </MDBContainer>
    </>
  );
}
const convertToLocalTime = (serverDate) => {
  try {
    const trimmedDate = serverDate.split(".")[0];
    const utcDateObj = new Date(trimmedDate.replace(" ", "T") + "Z");

    if (isNaN(utcDateObj)) {
      throw new Error("Invalid server date format");
    }

    const correctedDate = new Date(utcDateObj.getTime() - 2 * 60 * 60 * 1000);

    return correctedDate.toLocaleString("en-GB", {
      weekday: "short",
      day: "2-digit",
      month: "short",
      year: "numeric",
      hour: "2-digit",
      minute: "2-digit",
      hour12: false,
    });
  } catch (error) {
    return "Invalid Date";
  }
};
