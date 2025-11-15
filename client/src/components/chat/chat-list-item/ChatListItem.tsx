import './ChatListItem.css';

import { MDBIcon } from 'mdb-react-ui-kit';
import { type FC,useContext } from 'react';
import { Link, useNavigate } from 'react-router-dom';

import { routes } from '../../../common/constants/api';
import { UserContext } from '../../../contexts/user/userContext';
import * as hooks from '../../../hooks/useChat';
import DeleteModal from '../../common/delete-modal/DeleteModal';

const ChatListItem: FC<{
  id: number;
  name: string;
  imageUrl: string | null;
  creatorId: string;
}> = ({ id, name, imageUrl, creatorId }) => {
  const navigate = useNavigate();
  const { userId, isAdmin } = useContext(UserContext);

  const onEditClick = () => {
    navigate(routes.editChat, { state: { chat: { id, name, imageUrl } } });
  };

  const { showModal, toggleModal, deleteHandler } = hooks.useDeleteChat(id, name);

  return (
    <>
      <div className="row chat-list-item p-2 bg-light border rounded mb-3 shadow-sm">
        <div className="col-3 d-flex justify-content-center align-items-center">
          <img className="img-fluid chat-list-item-image" src={imageUrl || ''} alt={name} />
        </div>
        <div className="col-7 d-flex flex-column justify-content-between chat-list-item-content">
          <h5 className="mb-1 chat-list-item-name">{name}</h5>
          <div className="d-flex mt-2">
            {userId === creatorId && (
              <MDBIcon
                icon="pen"
                className="cursor-pointer"
                title="Edit Chat"
                onClick={onEditClick}
              />
            )}
            {userId === creatorId || isAdmin ? (
              <MDBIcon
                icon="trash"
                className="cursor-pointer ms-2"
                onClick={deleteHandler}
                title="Delete Chat"
              />
            ) : null}
          </div>
          <Link to={`${routes.chat}/${id}`} className="chat-list-item-btn">
            Details
          </Link>
        </div>
      </div>
      <DeleteModal showModal={showModal} toggleModal={toggleModal} deleteHandler={deleteHandler} />
    </>
  );
};

export default ChatListItem;
