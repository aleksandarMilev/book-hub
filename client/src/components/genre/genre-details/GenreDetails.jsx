import { useParams, useNavigate } from "react-router-dom";
import { MDBTypography, MDBCardText, MDBBtn } from "mdb-react-ui-kit";

import * as useGenre from "../../../hooks/useGenre";
import { routes } from "../../../common/constants/api";

import BookListItem from "../../book/book-list-item/BooksListItem";
import DefaultSpinner from "../../common/default-spinner/DefaultSpinner";

import "./GenreDetails.css";

export default function GenreDetails() {
  const { id } = useParams();
  const navigate = useNavigate();

  const { genre, isFetching } = useGenre.useDetails(id);

  if (isFetching || !genre) {
    return <DefaultSpinner />;
  }

  const onAllBooksClick = (e) => {
    e.preventDefault();
    navigate(routes.book, { state: { genreId: id, genreName: genre.name } });
  };

  return (
    <div className="container genre-details mt-5">
      <div className="card shadow-lg p-3 mb-5 bg-white rounded">
        <div className="row g-0">
          <div className="col-md-4 genre-image">
            <img
              src={genre.imageUrl}
              alt={`${genre.name} genre`}
              className="img-fluid rounded-start"
            />
          </div>
          <div className="col-md-8">
            <div className="card-body">
              <h1 className="card-title text-primary">{genre.name}</h1>
              <p className="card-text text-muted">{genre.description}</p>
            </div>
          </div>
        </div>
      </div>
      <div className="top-books-section mt-5">
        <MDBTypography tag="h4" className="section-title">
          Top Books
        </MDBTypography>
        {genre.topBooks && genre.topBooks.length > 0 ? (
          <>
            {genre.topBooks.map((b) => (
              <BookListItem key={b.id} {...b} />
            ))}
            <div className="d-flex justify-content-center mt-3">
              <MDBBtn onClick={(e) => onAllBooksClick(e)}>
                View all {genre.name} books
              </MDBBtn>
            </div>
          </>
        ) : (
          <MDBCardText>No books available for this genre.</MDBCardText>
        )}
      </div>
    </div>
  );
}
