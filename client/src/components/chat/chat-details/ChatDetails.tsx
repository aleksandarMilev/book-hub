
import './ChatDetails.css';

import {
  MDBCard,
  MDBCardBody,
  MDBCardHeader,
  MDBCardImage,
  MDBCol,
  MDBContainer,
  MDBRow,
} from 'mdb-react-ui-kit';
import type { FC } from 'react';

import * as hooks from '../../../hooks/useChat';
import DefaultSpinner from '../../common/default-spinner/DefaultSpinner';
import ChatButtons from './chat-buttons/ChatButtons';
import Message from './message/Message';
import ParticipantListItem from './participant-list-item/ParticipantListItem';
import SendForm from './send-form/SendForm';

const ChatDetails: FC = () => {
  const {
    chat,
    isFetching,
    userId,
    isEditMode,
    messages,
    participants,
    formik,
    handleEditMessage,
    handleCancelEdit,
    deleteMessage,
    removeUserClickHandler,
    refreshParticipantsList,
    onProfileClickHandler,
  } = hooks.useChatDetails();

  if (isFetching || !chat) {
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
                      src={chat?.imageUrl || ''}
                      alt="Chat"
                      className="chat-card-header-img"
                    />
                    <p className="mb-0 fw-bold">{chat?.name}</p>
                  </MDBCardHeader>
                  <MDBCardBody className="chat-card-body">
                    {messages?.map((m) => {
                      const isSentByUser = m.senderId === userId;
                      const sender = chat.participants.find((p) => p.id === m.senderId);

                      return (
                        <Message
                          key={m.id}
                          message={m}
                          isSentByUser={isSentByUser}
                          sender={sender}
                          onEdit={handleEditMessage}
                          onDelete={deleteMessage}
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
                          a.id === chat?.creatorId ? -1 : b.id === chat?.creatorId ? 1 : 0,
                        )
                        .map((p, i) => (
                          <ParticipantListItem
                            key={p.id}
                            participant={p}
                            index={i}
                            onProfileClickHandler={onProfileClickHandler}
                            onDeleteHandler={removeUserClickHandler}
                            currentUserIsChatCreator={userId === chat?.creatorId}
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
};

export default ChatDetails;
