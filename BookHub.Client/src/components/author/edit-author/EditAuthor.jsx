import { useParams } from 'react-router-dom'

import AuthorForm from '../author-form/AuthorForm'
import * as useAuthor from '../../../hooks/useAuthor'
import DefaultSpinner from '../../common/default-spinner/DefaultSpinner'

export default function EditAuthor() {
    const { id } = useParams()
    const { author } = useAuthor.useGetDetails(id)

    return author ? <AuthorForm authorData={author} isEditMode={true} /> : <DefaultSpinner/ >
}
