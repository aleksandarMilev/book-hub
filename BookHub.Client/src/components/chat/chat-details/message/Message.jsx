import { useContext } from 'react'
import { MDBIcon } from 'mdb-react-ui-kit'

import { useMessage } from '../../../../contexts/messageContext'
import { UserContext } from '../../../../contexts/userContext'

const convertToLocalTime = (utcDate) => {
    const localDate = new Date(utcDate)
    const offset = localDate.getTimezoneOffset() * 60000
    const localTime = new Date(localDate.getTime() - offset)
    return localTime.toLocaleString('en-GB', {
        weekday: 'short',
        day: '2-digit',
        month: 'short',
        year: 'numeric',
        hour: '2-digit',
        minute: '2-digit',
        hour12: false
    })
}

export default function Message ({ m, isSentByUser, sender, onEdit, onDelete, onProfileClick }) {
    const { token } = useContext(UserContext)
    const { showMessage } = useMessage()

   
    return (
        <div
            className={`d-flex flex-row justify-content-${isSentByUser ? "end" : "start"} mb-4`}
        >
            <div
                className={`p-3 ${isSentByUser ? "me-3 border" : "ms-3"}`}
                style={{
                    borderRadius: "15px",
                    backgroundColor: isSentByUser ? "#fbfbfb" : "rgba(57, 192, 237,.2)",
                }}
            >
                <p className="small mb-0">{m.message}</p>
                <small className="text-muted">
                {
                    m.modifiedOn
                        ? convertToLocalTime(m.modifiedOn) + " (Modified)"
                        : convertToLocalTime(m.createdOn)
                }               
                </small>
                {isSentByUser && (
                    <>
                        <MDBIcon
                            fas
                            icon="pencil-alt"
                            className="ms-2 cursor-pointer"
                            onClick={() => onEdit(m)}
                        />
                        <MDBIcon
                            fas
                            icon="trash"
                            className="ms-2 cursor-pointer"
                            onClick={() => onDelete(m.id)}
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
                onClick={() => onProfileClick(sender.id)}
            >
                <strong>
                    {sender?.firstName} {sender?.lastName}
                </strong>
            </div>
        </div>
    )
}
