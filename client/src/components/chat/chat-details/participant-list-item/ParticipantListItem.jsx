import { MDBCardImage, MDBIcon } from "mdb-react-ui-kit";

export default function ParticipantListItem({
  participant,
  index,
  onProfileClickHandler,
  onDeleteHandler,
  currentUserIsChatCreator,
}) {
  return (
    <li
      key={participant.id}
      className="d-flex align-items-center mb-3 profile-item"
      onClick={() => onProfileClickHandler(participant.id)}
    >
      <MDBCardImage
        src={participant.imageUrl}
        alt={participant.firstName}
        style={{
          width: "40px",
          height: "40px",
          borderRadius: "50%",
          objectFit: "cover",
          marginRight: "10px",
        }}
      />
      <span>
        {index === 0 ? (
          <>
            <strong>
              {participant.firstName} {participant.lastName}
            </strong>{" "}
            <span className="text-muted">(Chat Creator)</span>
          </>
        ) : (
          <>
            {participant.firstName} {participant.lastName}
            {currentUserIsChatCreator && (
              <MDBIcon
                fas
                icon="times"
                className="ms-2 cursor-pointer"
                onClick={(e) => {
                  e.stopPropagation();
                  onDeleteHandler(participant.id, participant.firstName);
                }}
              />
            )}
          </>
        )}
      </span>
    </li>
  );
}
