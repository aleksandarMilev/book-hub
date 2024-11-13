import * as useBook from '../../../hooks/useBook'

import BookListItem from '../book-list-item/BooksListItem'
import DefaultSpinner from '../../common/default-spinner/DefaultSpinner'

export default function BookList() {
    const { books, isFetching } = useBook.useGetAll()
    
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
