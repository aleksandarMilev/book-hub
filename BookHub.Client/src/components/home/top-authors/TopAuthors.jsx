import { Link } from 'react-router-dom'
import { MDBBtn, MDBTable, MDBTableHead, MDBTableBody } from 'mdb-react-ui-kit'

import * as useAuthors from '../../../hooks/useAuthor'
import renderStars from '../../../common/functions/renderStars'
import { routes } from '../../../common/constants/api'

import DefaultSpinner from '../../common/default-spinner/DefaultSpinner'

import './TopAuthors.css'  

export default function TopAuthors() {
    const { authors, isFetching } = useAuthors.useGetTopThree()

    if(isFetching){
        return <DefaultSpinner/ >
    }

    return (
        <>
            <h2 className="text-center my-5 top-authors-title">
                Top Authors
            </h2>
            <MDBTable align="middle" className="table-striped">
                <MDBTableHead>
                    <tr className="table-header">
                        <th scope="col">Author</th>
                        <th scope="col">Total Books</th>
                        <th scope="col">Rating</th>
                        <th scope="col"></th>
                    </tr>
                </MDBTableHead>
                <MDBTableBody>
                    {authors.map(a => (
                        <tr key={a.id} className="author-row">
                            <td>
                                <div className="d-flex align-items-center">
                                    <img
                                        src={a.imageUrl}
                                        alt={a.name}
                                        className="author-image"
                                    />
                                    <div>
                                        <p className="author-name mb-1">
                                            {a.name}
                                        </p>
                                    </div>
                                </div>
                            </td>
                            <td>{a.booksCount}</td>
                            <td>{renderStars(a.rating)}</td>
                            <td>
                                <MDBBtn 
                                    tag={Link} 
                                    to={routes.author + `/${a.id}`} 
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
    )
}
