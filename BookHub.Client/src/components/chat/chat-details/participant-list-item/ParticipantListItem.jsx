import {MDBCardImage, MDBIcon} from "mdb-react-ui-kit"
import * as api from '../../../../api/chatApi'
import { useParams } from "react-router-dom"
import { useMessage } from "../../../../contexts/messageContext"
import { useContext } from "react"
import { UserContext } from "../../../../contexts/userContext"

export default function ParticipantListItem({
    participants,
    index,
    onProfileClickHandler,
    currentUserIsChatCreator
}){

    const {id} = useParams()
    const {token} = useContext(UserContext)
    const { showMessage } = useMessage()

    const onRemoveUserClick = async (profileId, firstName) => {
        try {
            await api.removeUserAsync(id, profileId, token)
            showMessage(`You have successfully removed ${firstName}!`, true)
            //refetch()
        } catch (error) {
            showMessage(error.message, false)
        }  
    }

    return(
        <li
            key={participants.id}
            className="d-flex align-items-center mb-3 profile-item"
            onClick={() => onProfileClickHandler(participants.id)}
        >
            <MDBCardImage
                src={participants.imageUrl}
                alt={participants.firstName}
                style={{
                    width: "40px",
                    height: "40px",
                    borderRadius: "50%",
                    objectFit: "cover",
                    marginRight: "10px"
                }}
            />
            <span>
                {index === 0 ? (
                    <>
                        <strong>{participants.firstName} {participants.lastName}</strong> <span className="text-muted">(Chat Creator)</span>
                    </>
                ) : (
                    <>
                        {participants.firstName} {participants.lastName}
                        {currentUserIsChatCreator && 
                            <MDBIcon
                            fas
                            icon="times"
                            className="ms-2 cursor-pointer"
                            onClick={(e) => {
                                e.stopPropagation()
                                onRemoveUserClick(participants.id, participants.firstName)
                            }}
                        />}
                    </>
            
                )}
            </span>
        </li>
    )
}