import { useContext } from 'react'
import { Link, useNavigate } from 'react-router-dom'
import { MDBIcon } from 'mdb-react-ui-kit'

import { routes } from '../../../common/constants/api'
import { UserContext } from '../../../contexts/userContext'

import './ChatListItem.css'

export default function ChatListItem({ id, name, imageUrl, creatorId }) {
    const navigate = useNavigate()
    const { userId } = useContext(UserContext)

    const onEditClick = () => {
        const chatData = {
            id,
            name,
            imageUrl
        }

        navigate(routes.editChat, { state: { chat: chatData } })
    }

    return (
        <div className="row chat-list-item p-2 bg-light border rounded mb-3 shadow-sm">
            <div className="col-3 d-flex justify-content-center align-items-center">
                <img
                    className="img-fluid chat-list-item-image"
                    src={imageUrl}
                    alt={name}
                />
            </div>
            <div className="col-7 d-flex flex-column justify-content-between chat-list-item-content">
                <h5 className="mb-1 chat-list-item-name">{name}</h5>
                {userId === creatorId && (
                    <div className="d-flex mt-2">
                        <MDBIcon
                            icon="pen"
                            className="cursor-pointer"
                            title="Edit Chat"
                            onClick={onEditClick}
                        />
                        <MDBIcon
                            icon="trash"
                            className="cursor-pointer ms-2"
                            onClick={() => {}}
                            title="Delete Chat"
                        />
                    </div>
                )}
                <Link to={`/chat/${id}`} className="chat-list-item-btn">
                    Details
                </Link>
            </div>
        </div>
    )
}
