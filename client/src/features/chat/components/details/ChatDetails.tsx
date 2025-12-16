import './ChatDetails.css';

import {
  MDBBtn,
  MDBCard,
  MDBCardBody,
  MDBCardHeader,
  MDBCardImage,
  MDBCol,
  MDBContainer,
  MDBRow,
} from 'mdb-react-ui-kit';
import type { FC } from 'react';

import ChatButtons from '@/features/chat/components/details/buttons/ChatButtons.js';
import Message from '@/features/chat/components/details/message/Message.js';
import ParticipantListItem from '@/features/chat/components/details/participant-list-item/ParticipantListItem.js';
import SendForm from '@/features/chat/components/details/send-form/SendForm.js';
import { useChatDetails } from '@/features/chat/hooks/useCrud.js';
import DefaultSpinner from '@/shared/components/default-spinner/DefaultSpinner.js';

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
    loadMoreMessages,
  } = useChatDetails();

  if (isFetching || !chat) {
    return <DefaultSpinner />;
  }

  return (
    <>
      <ChatButtons
        chatName={chat.name}
        chatCreatorId={chat.creatorId}
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
                      src={chat.imagePath || ''}
                      alt="Chat"
                      className="chat-card-header-img"
                    />
                    <p className="mb-0 fw-bold">{chat.name}</p>
                  </MDBCardHeader>
                  <MDBCardBody className="chat-card-body">
                    <div className="d-flex justify-content-center mb-3">
                      <MDBBtn
                        size="sm"
                        outline
                        onClick={() => loadMoreMessages(50)}
                        disabled={!messages || messages.length === 0}
                      >
                        Load older messages
                      </MDBBtn>
                    </div>
                    {messages?.map((m) => {
                      const isSentByUser = m.senderId === userId;
                      const sender = participants.find((p) => p.id === m.senderId);

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
                        .slice()
                        .sort((a, b) =>
                          a.id === chat.creatorId ? -1 : b.id === chat.creatorId ? 1 : 0,
                        )
                        .map((p, i) => (
                          <ParticipantListItem
                            key={p.id}
                            participant={p}
                            index={i}
                            onProfileClickHandler={onProfileClickHandler}
                            onDeleteHandler={removeUserClickHandler}
                            currentUserIsChatCreator={userId === chat.creatorId}
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
