import { useSearchNationalities } from "../../../../hooks/useNationality";

export default function NationalitySearch({ nationalities, loading, formik }) {
  const {
    searchTerm,
    filteredNationalities,
    showDropdown,
    updateSearchTerm,
    selectNationality,
    showDropdownOnFocus,
  } = useSearchNationalities(nationalities);

  return (
    <div className="mb-4">
      <h6 className="fw-bold mb-2">
        {"Nationality: (select 'unknown' if you are not sure)"}
      </h6>
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
        showDropdown &&
        searchTerm && (
          <ul
            className="list-group mt-2"
            style={{ maxHeight: "200px", overflowY: "auto" }}
          >
            {filteredNationalities.length === 0 ? (
              <li className="list-group-item">No matches found</li>
            ) : (
              filteredNationalities.map((n) => (
                <li
                  key={n.id}
                  className="list-group-item"
                  onClick={() => {
                    formik.setFieldValue("nationality", n.id);
                    selectNationality(n.name);
                  }}
                  style={{
                    cursor: "pointer",
                    padding: "10px",
                    borderBottom: "1px solid #ddd",
                  }}
                >
                  {n.name}
                </li>
              ))
            )}
          </ul>
        )
      )}
    </div>
  );
}
