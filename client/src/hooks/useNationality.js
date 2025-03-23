import { useContext, useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";

import * as nationalityApi from "../api/nationalityApi";
import { routes } from "../common/constants/api";
import { UserContext } from "../contexts/userContext";

export function useNationalities() {
  const { token } = useContext(UserContext);

  const navigate = useNavigate();
  const [nationalities, setNationalities] = useState([]);
  const [isFetching, setIsFetching] = useState(false);

  useEffect(() => {
    async function fetchData() {
      try {
        setIsFetching((old) => !old);
        setNationalities(await nationalityApi.getNationalitiesAsync(token));
        setIsFetching((old) => !old);
      } catch (error) {
        navigate(routes.badRequest, { state: { message: error.message } });
      }
    }

    fetchData();
  }, [token, navigate]);

  return { nationalities, isFetching };
}

export function useSearchNationalities(nationalities) {
  const [searchTerm, setSearchTerm] = useState("");
  const [filteredNationalities, setFilteredNationalities] = useState([]);
  const [showDropdown, setShowDropdown] = useState(false);

  useEffect(() => {
    if (searchTerm === "") {
      setFilteredNationalities([]);
    } else {
      const filtered = nationalities.filter((n) =>
        n.name.toLowerCase().includes(searchTerm.toLowerCase())
      );

      setFilteredNationalities(filtered);
    }
  }, [searchTerm, nationalities]);

  const updateSearchTerm = (term) => {
    setSearchTerm(term);
    setShowDropdown(true);
  };

  const selectNationality = (nationality) => {
    setSearchTerm(nationality);
    setShowDropdown(false);
  };

  const showDropdownOnFocus = () => setShowDropdown(true);

  return {
    searchTerm,
    filteredNationalities,
    showDropdown,
    updateSearchTerm,
    selectNationality,
    showDropdownOnFocus,
  };
}
