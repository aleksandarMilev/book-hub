import React, { useEffect, useState } from 'react';
import { MDBContainer, MDBListGroup, MDBListGroupItem, MDBCardTitle, MDBCardText, MDBIcon } from 'mdb-react-ui-kit';
import { getAllAsync } from '../../api/bookAPI';

export default function Books() {
    const [books, setBooks] = useState([]); 
    const [loading, setLoading] = useState(true); 

    useEffect(() => {
        (async () => {
            try {
                const data = await getAllAsync();
                setBooks(data); 
            } catch (error) {
                console.error("Failed to fetch books:", error);
            } finally {
                setLoading(false); 
            }
        })();
    }, []);

    if (loading) {
        return <MDBContainer className="mt-5"><h2 className="text-center mb-4">Loading...</h2></MDBContainer>; // Loading state
    }

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
