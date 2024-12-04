import { useParams } from 'react-router-dom'

import * as useBook from '../../../hooks/useBook'
import BookForm from '../book-form/BookForm'
import DefaultSpinner from '../../common/default-spinner/DefaultSpinner'

export default function EditBook() {
    const { id } = useParams()
    const { book, isFetching } = useBook.useGetFullInfo(id)

    return isFetching 
        ? <BookForm bookData={book} isEditMode={true} /> 
        : <DefaultSpinner/ >
}
