import { Link } from 'react-router-dom'

import { routes } from '../../../common/constants/api'

import './ChatListItem.css'

export default function ChatListItem({ id, name, imageUrl }) {
    return (
        <Link to={routes.chat + `/${id}`} className="chat-list-item">
            <div className="row p-2 bg-light border rounded mb-3 shadow-sm">
                <div className="col-3 d-flex justify-content-center align-items-center">
                    <img
                        className="img-fluid img-responsive rounded chat-list-item-image"
                        src={imageUrl}
                        alt={name}
                    />
                </div>
                <div className="col-7 d-flex flex-column justify-content-center chat-list-item-content">
                    <h5 className="mb-1 chat-list-item-name">{name}</h5>
                </div>
            </div>
        </Link>
    )
}
