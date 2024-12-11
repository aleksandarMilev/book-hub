import { useContext, useState } from 'react'
import { Link, useNavigate } from 'react-router-dom'
import { MDBIcon } from 'mdb-react-ui-kit'

import * as api from '../../../api/chatApi'
import { routes } from '../../../common/constants/api'
import { UserContext } from '../../../contexts/userContext'
import { useMessage } from '../../../contexts/messageContext'

import DeleteModal from '../../common/delete-modal/DeleteModal'

import './ChatListItem.css'

export default function ChatListItem({ id, name, imageUrl, creatorId }) {
    const navigate = useNavigate()

    const { userId, token, isAdmin } = useContext(UserContext)
    const { showMessage } = useMessage() 

    const onEditClick = () => {
        const chatData = {
            id,
            name,
            imageUrl
        }

        navigate(routes.editChat, { state: { chat: chatData } })
    }

    const [showModal, setShowModal] = useState(false)
    const toggleModal = () => setShowModal(prev => !prev)

    const onChatDelete = async () => {
        if(showModal){
            try {
                await api.deleteChatAsync(id, token)
                showMessage(`You have successfuly deleted ${name}!`, true)
                navigate(routes.home)
            } catch(error) {
                showMessage(error.message, false)
            }
        } else {
            toggleModal()
        }
    }

    return (
        <>
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
                    <div className="d-flex mt-2">
                        {userId === creatorId && (
                                <MDBIcon
                                    icon="pen"
                                    className="cursor-pointer"
                                    title="Edit Chat"
                                    onClick={onEditClick}
                                />
                        )}
                        {(userId === creatorId) || isAdmin ? (
                            <MDBIcon
                                    icon="trash"
                                    className="cursor-pointer ms-2"
                                    onClick={onChatDelete}
                                    title="Delete Chat"
                                />
                        ): null}
                    </div>
                <Link to={`/chat/${id}`} className="chat-list-item-btn">
                    Details
                </Link>
            </div>
        </div>
        <DeleteModal
            showModal={showModal}
            toggleModal={toggleModal}
            deleteHandler={onChatDelete}
        />
     </>
    )
}
