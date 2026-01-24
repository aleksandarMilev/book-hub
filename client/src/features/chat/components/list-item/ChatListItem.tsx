import './ChatListItem.css';

import { MDBIcon } from 'mdb-react-ui-kit';
import { type FC } from 'react';
import { Link, useNavigate } from 'react-router-dom';

import { useDeleteChat } from '@/features/chat/hooks/useCrud.js';
import DeleteModal from '@/shared/components/delete-modal/DeleteModal.js';
import { routes } from '@/shared/lib/constants/api.js';
import { useAuth } from '@/shared/stores/auth/auth.js';

type Props = {
  id: string;
  name: string;
  imagePath: string | null;
  creatorId: string;
};

const ChatListItem: FC<Props> = ({ id, name, imagePath, creatorId }) => {
  const navigate = useNavigate();
  const { userId, isAdmin } = useAuth();

  const onEditClick = () => {
    navigate(routes.editChat, { state: { chat: { id, name, imagePath, creatorId } } });
  };

  const { showModal, toggleModal, deleteHandler } = useDeleteChat(id, name);

  return (
    <>
      <div className="row chat-list-item p-2 bg-light border rounded mb-3 shadow-sm">
        <div className="col-3 d-flex justify-content-center align-items-center">
          <img className="img-fluid chat-list-item-image" src={imagePath || ''} alt={name} />
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
