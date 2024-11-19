import { useParams } from 'react-router-dom'

import BookForm from '../book-form/BookForm'
import * as useBook from '../../../hooks/useBook'
import DefaultSpinner from '../../common/default-spinner/DefaultSpinner'

export default function EditAuthor() {
    const { id } = useParams()
    const { book } = useBook.useGetFullInfo(id)

    if(book){
        console.log(book);
        
    }

    return book ? <BookForm bookData={book} isEditMode={true} /> : <DefaultSpinner/ >
}
