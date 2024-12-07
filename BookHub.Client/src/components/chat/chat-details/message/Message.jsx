import React, { memo, useContext } from 'react'
import { MDBIcon } from 'mdb-react-ui-kit'
import { utcToLocal } from '../../../../common/functions/utcToLocal'
import * as api from '../../../../api/chatApi'
import { useMessage } from '../../../../contexts/messageContext'
import { UserContext } from '../../../../contexts/userContext'


const Message = memo(({ m, isSentByUser, sender, onEdit, onProfileClick }) => {
    const { token } = useContext(UserContext)
    const { showMessage } = useMessage()

    const onDelete = async (id) => {
        try {
            await api.deleteMessageAsync(id, token)
            showMessage("Your message was successfuly deleted", true)
        } catch (error) {
            showMessage(error.message, false)
        }
        // } finally {
        //     refetch()
        // }
    }

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
                    {m.modifiedOn
                        ? utcToLocal(m.modifiedOn) + " (Modified)"
                        : utcToLocal(m.createdOn)}
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
    );
});

export default Message;
