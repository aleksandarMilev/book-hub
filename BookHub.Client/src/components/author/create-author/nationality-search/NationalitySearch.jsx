import React from 'react'

import useSearchNationalities from '../../../../hooks/useAuthor'

export default function NationalitySearch({ nationalities, loading, formik }) {
    const {
        searchTerm,
        filteredNationalities,
        showDropdown,
        updateSearchTerm,
        selectNationality,
        showDropdownOnFocus,
    } = useSearchNationalities(nationalities)

    return (
        <div className="mb-4">
            <h6 className="fw-bold mb-2">{"Nationality: * (select the 'unknown' option if you are not sure)"}</h6>
            <input
                type="text"
                id="nationality"
                className="form-control"
                placeholder="Search for a nationality..."
                value={searchTerm}
                onChange={(e) => updateSearchTerm(e.target.value)} 
                onFocus={showDropdownOnFocus} 
            />
            {loading ? (
                <div className="mt-2">Loading...</div>
            ) : (
                showDropdown && searchTerm && (
                    <ul className="list-group mt-2" style={{ maxHeight: '200px', overflowY: 'auto' }}>
                        {filteredNationalities.length === 0 ? (
                            <li className="list-group-item">No matches found</li>
                        ) : (
                            filteredNationalities.map((nationality, index) => (
                                <li
                                    key={index}
                                    className="list-group-item"
                                    onClick={() => {
                                        formik.setFieldValue('nationality', nationality)
                                        selectNationality(nationality)
                                    }}
                                    style={{ cursor: 'pointer', padding: '10px', borderBottom: '1px solid #ddd' }}
                                >
                                    {nationality}
                                </li>
                            ))
                        )}
                    </ul>
                )
            )}
            {formik.touched.nationality && formik.errors.nationality && (
                <div className="text-danger">{formik.errors.nationality}</div>
            )}
        </div>
    )
}
