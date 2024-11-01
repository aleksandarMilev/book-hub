import { MDBContainer, MDBListGroup } from 'mdb-react-ui-kit';

import { useGetAllBooks } from '../../hooks/useBooks';

import BookListItem from './BooksListItem';
import DefaultSpinner from '../common/DefaultSpinner'

export default function BookList() {
    const { books, isFetching } = useGetAllBooks()

    return (
        <MDBContainer className="mt-5">
            <h2 className="text-center mb-4">📚 Book List</h2>
            <MDBListGroup className="shadow-2 rounded">
                {isFetching 
                    ?  <DefaultSpinner/> 
                    :  books.length > 0 
                            ? books.map(b => (<BookListItem key={b.id} {...b}/>))
                            : <p className="text-center mt-4 text-muted">No books found!</p>
                }
            </MDBListGroup>
        </MDBContainer>
    )
}
