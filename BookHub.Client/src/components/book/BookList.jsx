import * as useBook from '../../hooks/useBook'

import BookListItem from './BooksListItem'
import DefaultSpinner from '../common/DefaultSpinner'

export default function BookList() {
    const { books, isFetching } = useBook.useGetAll()
    console.log(books);
    
    return (
            <div className="container mt-5 mb-5">
                <div className="d-flex justify-content-center row">
                    <div className="col-md-10">
                        {isFetching
                            ?  <DefaultSpinner/> 
                            :  books.length > 0 
                                    ? books.map(b => (<BookListItem key={b.id} {...b}/>))
                                    : <p className="text-center mt-4 text-muted">No books found!</p>
                        }
                    </div>
                </div>
            </div>
    )
}
