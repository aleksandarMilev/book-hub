import React, { useEffect, useState } from 'react';
import { MDBContainer, MDBListGroup } from 'mdb-react-ui-kit';

import { getAllAsync } from '../../api/bookAPI';

import BookListItem from './BooksListItem';


export default function BookList() {
    const [books, setBooks] = useState([])

    useEffect(() => {
        (async () => {
            setBooks(await getAllAsync())
        })()
    }, [])

    return (
        <MDBContainer className="mt-5">
            <h2 className="text-center mb-4">📚 Book List</h2>
            <MDBListGroup className="shadow-2 rounded">
                {books.length > 0 
                    ? books.map(b => (<BookListItem key={b.id} {...b}/>))
                    : <p className="text-center mt-4 text-muted">No books found!</p>
                }
            </MDBListGroup>
        </MDBContainer>
    )
}
