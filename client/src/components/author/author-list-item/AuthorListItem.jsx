import { Link } from "react-router-dom";
import { FaUser, FaPenFancy } from "react-icons/fa";

import renderStars from "../../../common/functions/renderStars";
import { routes } from "../../../common/constants/api";

import "./AuthorListItem.css";

export default function AuthorListItem({
  id,
  imageUrl,
  name,
  penName,
  averageRating,
}) {
  return (
    <div className="row p-3 bg-light border rounded mb-3 shadow-sm author-list-item">
      <div className="col-md-3 col-4 mt-1 d-flex justify-content-center align-items-center">
        {imageUrl ? (
          <img
            className="img-fluid img-responsive rounded-circle author-list-item-image"
            src={imageUrl}
            alt={name || penName || "Author"}
          />
        ) : (
          <div className="author-list-item-placeholder rounded-circle">
            No Image
          </div>
        )}
      </div>
      <div className="col-md-6 col-8 mt-1 author-list-item-content">
        <h5 className="mb-2 author-list-item-name">
          <FaUser className="me-2" />
          {name || penName || "Unknown Author"}
        </h5>
        {penName && (
          <h6 className="text-muted mb-2 author-list-item-pen-name">
            <FaPenFancy className="me-2" />
            Pen Name: {penName}
          </h6>
        )}
        <div className="d-flex flex-row mb-2 author-list-item-rating">
          {renderStars(averageRating)}
        </div>
      </div>

      <div className="col-md-3 d-flex align-items-center justify-content-center mt-1">
        <div className="d-flex flex-column align-items-center">
          <Link
            to={routes.author + `/${id}`}
            className="btn btn-sm btn-primary author-list-item-btn"
          >
            View
          </Link>
        </div>
      </div>
    </div>
  );
}
