import './TopAuthors.css';

import { MDBBtn, MDBTable, MDBTableBody, MDBTableHead } from 'mdb-react-ui-kit';
import { type FC } from 'react';
import { FaBookReader } from 'react-icons/fa';
import { Link } from 'react-router-dom';

import { useTopThree } from '@/features/author/hooks/useCrud.js';
import DefaultSpinner from '@/shared/components/default-spinner/DefaultSpinner.js';
import { RenderStars } from '@/shared/components/render-stars/RenderStars.js';
import { routes } from '@/shared/lib/constants/api.js';

const TopAuthors: FC = () => {
  const { authors, isFetching, error } = useTopThree();

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
      <div className="authors-wrapper">
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
                <td className="rating-cell">
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
      </div>
    </>
  );
};

export default TopAuthors;
