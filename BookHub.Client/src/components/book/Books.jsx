import React, { useEffect, useState } from 'react';
import { MDBContainer, MDBListGroup, MDBListGroupItem, MDBCardTitle, MDBCardText, MDBIcon } from 'mdb-react-ui-kit';

import { getAllAsync } from '../../api/bookAPI';

const books = [
    {
        id: 1,
        title: "To Kill a Mockingbird",
        author: "Harper Lee",
        description: "A novel about the serious issues of rape and racial inequality.",
    },
    {
        id: 2,
        title: "1984",
        author: "George Orwell",
        description: "A dystopian social science fiction novel and cautionary tale about the dangers of totalitarianism.",
    },
    {
        id: 3,
        title: "Pride and Prejudice",
        author: "Jane Austen",
        description: "A romantic novel that charts the emotional development of the protagonist, Elizabeth Bennet.",
    }
];

export default function Books() {
    // [books, setBooks] = useState([])

    // useEffect(() => {
    //     (async () => {
    //         let books = await getAllAsync()
    //         setBooks(books)
    //     })()
    // }, [])

    return (
        <MDBContainer className="mt-5">
            <h2 className="text-center mb-4">📚 Book List</h2>
            <MDBListGroup className="shadow-2 rounded">
                {books.map((book) => (
                    <MDBListGroupItem key={book.id} className="d-flex flex-column bg-light p-4 mb-2">
                        <div className="d-flex align-items-center mb-2">
                            <MDBIcon fas icon="book-open" className="me-2 text-primary" />
                            <MDBCardTitle className="h5 mb-0">{book.title}</MDBCardTitle>
                        </div>
                        <h6 className="text-muted mb-3">by {book.author}</h6>
                        <MDBCardText>{book.description}</MDBCardText>
                    </MDBListGroupItem>
                ))}
            </MDBListGroup>
        </MDBContainer>
    );
}
