import { useSearchAuthors } from '../../../../hooks/useAuthor'

import './AuthorSearch.css'

export default function AuthorSearch({ authors, loading, formik }) {
    const {
        searchTerm,
        filteredAuthors,
        showDropdown,
        updateSearchTerm,
        selectAuthor,
        showDropdownOnFocus,
    } = useSearchAuthors(authors)

    return (
        <div className="mb-4">
            <h6 className="fw-bold mb-2">{"Author Name: (Select known authors, or leave blank if unknown)"}</h6>
            {formik.touched.authorName && formik.errors.authorName && (
                <div className="text-danger">{formik.errors.authorName}</div>
            )}
            <input
                type="text"
                id="authorName"
                className="form-control"
                placeholder="Search for an author..."
                value={searchTerm}
                onChange={(e) => {
                    const value = e.target.value
                    updateSearchTerm(value)
                    
                    if (!value) {
                        formik.setFieldValue('authorId', '')
                        formik.setFieldValue('authorName', '')
                    }
                }}
                onFocus={showDropdownOnFocus}
            />
            {loading ? (
                <div className="mt-2">Loading...</div>
            ) : (
                showDropdown && searchTerm && (
                    <ul className="list-group mt-2 dropdown-list">
                        {filteredAuthors.length === 0 ? (
                            <li className="list-group-item">No matches found</li>
                        ) : (
                            filteredAuthors.map(a => (
                                <li
                                    key={a.id}
                                    className="list-group-item dropdown-item"
                                    onClick={() => {
                                        formik.setFieldValue('authorId', a.id);
                                        formik.setFieldValue('authorName', a.name) 

                                        selectAuthor(a.name)
                                    }}
                                >
                                    {a.name}
                                </li>
                            ))
                        )}
                    </ul>
                )
            )}
        </div>
    )
}
