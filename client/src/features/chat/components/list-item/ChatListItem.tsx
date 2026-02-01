import './ChatListItem.css';

import { MDBIcon } from 'mdb-react-ui-kit';
import { type FC } from 'react';
import { Link, useNavigate } from 'react-router-dom';

import { useDeleteChat } from '@/features/chat/hooks/useCrud';
import DeleteModal from '@/shared/components/delete-modal/DeleteModal';
import { routes } from '@/shared/lib/constants/api';
import { useAuth } from '@/shared/stores/auth/auth';
import { getImageUrl } from '@/shared/lib/utils/utils';

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
    navigate(`${routes.editChat}/${id}`, {
      state: { chat: { id, name, imagePath, creatorId } },
    });
  };

  const { showModal, toggleModal, deleteHandler } = useDeleteChat(id, name);

  return (
    <>
      <div className="row p-3 bg-light border rounded mb-0 shadow-sm chat-list-item">
        <div className="col-md-3 col-4 mt-1 d-flex justify-content-center align-items-center">
          <img className="chat-list-item-image" src={getImageUrl(imagePath!, 'chats')} alt={name} />
        </div>
        <div className="col-md-6 col-8 mt-1 chat-list-item-content">
          <h5 className="mb-2 chat-list-item-name">{name}</h5>
          <div className="chat-list-item-icons">
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
        </div>
        <div className="col-md-3 d-flex align-items-center justify-content-center mt-1">
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


