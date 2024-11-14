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
            <input
                type="text"
                id="authorName"
                className="form-control"
                placeholder="Search for an author..."
                value={searchTerm}
                onChange={(e) => updateSearchTerm(e.target.value)}
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
                            filteredAuthors.map((author, index) => (
                                <li
                                    key={index}
                                    className="list-group-item dropdown-item"
                                    onClick={() => {
                                        formik.setFieldValue('authorName', author)
                                        selectAuthor(author)
                                    }}
                                >
                                    {author}
                                </li>
                            ))
                        )}
                    </ul>
                )
            )}
            {formik.touched.authorName && formik.errors.authorName && (
                <div className="text-danger">{formik.errors.authorName}</div>
            )}
        </div>
    )
}
