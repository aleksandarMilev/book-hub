import { type FC } from 'react';
import { Link } from 'react-router-dom';
import { MDBBtn, MDBTable, MDBTableHead, MDBTableBody } from 'mdb-react-ui-kit';
import { FaBookReader } from 'react-icons/fa';

import * as hooks from '../../../hooks/useAuthor';
import { routes } from '../../../common/constants/api';

import DefaultSpinner from '../../common/default-spinner/DefaultSpinner';

import './TopAuthors.css';
import { RenderStars } from '../../common/render-stars/renderStars';

const TopAuthors: FC = () => {
  const { authors, isFetching, error } = hooks.useTopThree();

  if (isFetching) {
    return <DefaultSpinner />;
  }

  if (error) {
    return (
      <div className="d-flex flex-column align-items-center justify-content-center vh-50">
        <div className="text-center">
          <FaBookReader size={100} color="red" className="mb-3" />
          <p className="lead">{error}</p>
        </div>
      </div>
    );
  }

  if (!authors?.length) {
    return (
      <div className="d-flex flex-column align-items-center justify-content-center vh-50">
        <div className="text-center">
          <FaBookReader size={80} className="mb-3 text-muted" />
          <p className="lead text-muted">No authors found.</p>
        </div>
      </div>
    );
  }

  return (
    <>
      <h2 className="text-center my-5 top-authors-title">Top Authors</h2>
      <MDBTable align="middle" className="table-striped">
        <MDBTableHead>
          <tr className="table-header">
            <th scope="col">Author</th>
            <th scope="col">Total Books</th>
            <th scope="col">Rating</th>
            <th scope="col" />
          </tr>
        </MDBTableHead>
        <MDBTableBody>
          {authors.map((a) => (
            <tr key={a.id} className="author-row">
              <td>
                <div className="d-flex align-items-center">
                  <img src={a.imageUrl ?? undefined} alt={a.name} className="author-image me-3" />
                  <div>
                    <p className="author-name mb-1">{a.name}</p>
                  </div>
                </div>
              </td>
              <td>{a.booksCount}</td>
              <td>
                <RenderStars rating={a.averageRating ?? 0} />
              </td>
              <td>
                <MDBBtn
                  tag={Link}
                  to={`${routes.author}/${a.id}`}
                  color="dark"
                  rounded
                  size="lg"
                  className="view-button"
                >
                  View
                </MDBBtn>
              </td>
            </tr>
          ))}
        </MDBTableBody>
      </MDBTable>
    </>
  );
};

export default TopAuthors;
