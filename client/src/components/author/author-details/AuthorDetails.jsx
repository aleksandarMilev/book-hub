import { useContext, useState } from "react";
import { Link, useParams, useNavigate } from "react-router-dom";
import { format } from "date-fns";
import { FaEdit, FaTrashAlt, FaTrash } from "react-icons/fa";
import {
  MDBCol,
  MDBContainer,
  MDBRow,
  MDBCard,
  MDBCardText,
  MDBCardBody,
  MDBCardImage,
  MDBTypography,
  MDBBtn,
} from "mdb-react-ui-kit";

import * as authorApi from "../../../api/authorApi";
import * as useAuthor from "../../../hooks/useAuthor";
import renderStars from "../../../common/functions/renderStars";
import { errors } from "../../../common/constants/messages";
import { routes } from "../../../common/constants/api";
import { UserContext } from "../../../contexts/userContext";
import { useMessage } from "../../../contexts/messageContext";

import BookListItem from "../../book/book-list-item/BooksListItem";
import DeleteModal from "../../common/delete-modal/DeleteModal";
import DefaultSpinner from "../../common/default-spinner/DefaultSpinner";

import "./AuthorDetails.css";

export default function AuthorDetails() {
  const { id } = useParams();
  const navigate = useNavigate();
  const [showModal, setShowModal] = useState(false);

  const { userId, token, isAdmin } = useContext(UserContext);
  const { showMessage } = useMessage();

  const { author, isFetching } = useAuthor.useGetDetails(id);

  const toggleModal = () => setShowModal((prev) => !prev);

  const onAllBooksClick = (e) => {
    e.preventDefault();
    navigate(routes.book, {
      state: { authorId: id, authorName: author?.name },
    });
  };

  const deleteHandler = async () => {
    if (showModal) {
      const success = await authorApi.deleteAsync(id, token);

      if (success) {
        navigate(routes.author);
        showMessage(
          `${author?.name || "This author"} was successfully deleted!`,
          true
        );
      } else {
        showMessage(
          `Something went wrong while deleting ${
            author?.name || "this author"
          }, please, try again!`,
          false
        );
      }
    } else {
      toggleModal();
    }
  };

  if (isFetching || !author) {
    return <DefaultSpinner />;
  }

  const isCreator = author?.creatorId === userId;

  return (
    <div className="author-details-wrapper">
      <MDBContainer className="py-5 h-100">
        <MDBRow className="justify-content-center align-items-center h-100">
          <MDBCol lg="10">
            <MDBCard className="author-card">
              <MDBCardBody>
                <div className="author-header">
                  <MDBCardImage
                    src={author.imageUrl}
                    alt={`${author.name}'s image`}
                    className="author-image"
                    fluid
                  />
                  <div>
                    <MDBTypography tag="h2" className="author-name">
                      {author.name}
                    </MDBTypography>
                    <MDBCardText className="author-nationality">
                      {author.nationality.name}
                    </MDBCardText>
                    <MDBCardText className="author-penname">
                      {author.penName
                        ? `Pen Name: ${author.penName}`
                        : "No Pen Name"}
                    </MDBCardText>
                  </div>
                </div>
                <div className="author-actions">
                  {isCreator && (
                    <Link
                      to={`${routes.editAuthor}/${author.id}`}
                      className="me-2"
                    >
                      <MDBBtn outline color="warning" size="sm">
                        <FaEdit className="me-1" /> Edit
                      </MDBBtn>
                    </Link>
                  )}
                  {(isCreator || isAdmin) && (
                    <MDBBtn
                      outline
                      color="danger"
                      size="sm"
                      onClick={toggleModal}
                    >
                      <FaTrashAlt className="me-1" /> Delete
                    </MDBBtn>
                  )}
                </div>
                {isAdmin && !author.isApproved && (
                  <ApproveRejectButtons
                    authorId={author.id}
                    authorName={author.name}
                    initialIsApproved={author.isApproved}
                    token={token}
                    onSuccess={(message, success = true) =>
                      showMessage(message, success)
                    }
                  />
                )}
                <section className="author-about">
                  <MDBTypography tag="h4" className="section-title">
                    About
                  </MDBTypography>
                  <MDBCardText className="author-biography">
                    {author.biography}
                  </MDBCardText>
                  <MDBCardText className="author-birthdate">
                    <strong>Born:</strong>{" "}
                    {author.bornAt
                      ? format(new Date(author.bornAt), "MMM dd, yyyy")
                      : "Unknown"}
                    {author.bornAt && !author.diedAt
                      ? ` (${
                          new Date().getFullYear() -
                          new Date(author.bornAt).getFullYear()
                        } years old)`
                      : ""}
                  </MDBCardText>
                  <MDBCardText className="author-deathdate">
                    {author.diedAt && (
                      <>
                        <strong>Died: </strong>
                        {format(new Date(author.diedAt), "MMM dd, yyyy")}
                        {author.bornAt &&
                          ` (${
                            new Date(author.diedAt).getFullYear() -
                            new Date(author.bornAt).getFullYear()
                          } years old)`}
                      </>
                    )}
                  </MDBCardText>
                </section>
                <section className="author-statistics">
                  <MDBTypography tag="h4" className="section-title">
                    Statistics
                  </MDBTypography>
                  <MDBRow className="text-center mt-3">
                    <MDBCol className="d-flex flex-column align-items-center">
                      <MDBCardText className="author-rating">
                        {renderStars(author.averageRating)}
                      </MDBCardText>
                      <MDBCardText className="author-rating-text">
                        Average Rating
                      </MDBCardText>
                    </MDBCol>
                    <MDBCol className="d-flex flex-column align-items-center">
                      <MDBCardText className="author-books-count">
                        {author.booksCount}
                      </MDBCardText>
                      <MDBCardText className="author-books-text">
                        Books Published
                      </MDBCardText>
                    </MDBCol>
                  </MDBRow>
                </section>
                <section className="author-top-books">
                  <MDBTypography tag="h4" className="section-title">
                    Top Books
                  </MDBTypography>
                  {author.topBooks && author.topBooks.length > 0 ? (
                    <>
                      {author.topBooks.map((b) => (
                        <BookListItem key={b.id} {...b} />
                      ))}
                      <div className="d-flex justify-content-center mt-3">
                        <MDBBtn onClick={(e) => onAllBooksClick(e)}>
                          View all {author?.name} books
                        </MDBBtn>
                      </div>
                    </>
                  ) : (
                    <MDBCardText>
                      No books available for this author.
                    </MDBCardText>
                  )}
                </section>
              </MDBCardBody>
            </MDBCard>
          </MDBCol>
        </MDBRow>
      </MDBContainer>
      <DeleteModal
        showModal={showModal}
        toggleModal={toggleModal}
        deleteHandler={deleteHandler}
      />
    </div>
  );
}

function ApproveRejectButtons({
  authorId,
  authorName,
  initialIsApproved,
  token,
  onSuccess,
}) {
  const navigate = useNavigate();
  const [isApproved, setIsApproved] = useState(initialIsApproved);

  const approveHandler = async () => {
    try {
      await authorApi.approveAsync(authorId, token);
      setIsApproved(true);
      onSuccess(`${authorName} was successfully approved!`);
    } catch (error) {
      onSuccess(error.message, false);
    }
  };

  const rejectHandler = async () => {
    try {
      await authorApi.rejectAsync(authorId, token);
      onSuccess(`${authorName} was successfully rejected!`);
      navigate(routes.home);
    } catch (error) {
      onSuccess(error.message, false);
    }
  };

  if (isApproved) {
    return <p className="text-success">This author has been approved.</p>;
  }

  return (
    <div className="author-actions">
      <button className="btn btn-success me-2" onClick={approveHandler}>
        Approve
      </button>
      <button className="btn btn-danger" onClick={rejectHandler}>
        Reject
      </button>
    </div>
  );
}
