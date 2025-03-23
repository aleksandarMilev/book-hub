import { useContext, useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";

import * as genreApi from "../api/genreApi";
import { routes } from "../common/constants/api";
import { UserContext } from "../contexts/userContext";

export function useGenres() {
  const navigate = useNavigate();
  const [genres, setGenres] = useState([]);
  const [isFetching, setIsFetching] = useState(false);

  const { token } = useContext(UserContext);

  useEffect(() => {
    async function fetchData() {
      try {
        setIsFetching(true);
        setGenres(await genreApi.getGenresAsync(token));
      } catch (error) {
        navigate(routes.badRequest, { state: { message: error.message } });
      } finally {
        setIsFetching(false);
      }
    }

    fetchData();
  }, [token, navigate]);

  return { genres, isFetching };
}

export function useSearchGenres(genres, selectedGenres) {
  const [searchTerm, setSearchTerm] = useState("");
  const [filteredGenres, setFilteredGenres] = useState([]);

  useEffect(() => {
    if (searchTerm === "") {
      setFilteredGenres([]);
    } else {
      const filtered = genres.filter(
        (g) =>
          g.name.toLowerCase().includes(searchTerm.toLowerCase()) &&
          !selectedGenres.some((selectedGenre) => selectedGenre.id === g.id)
      );

      setFilteredGenres(filtered);
    }
  }, [searchTerm, genres, selectedGenres]);

  const updateSearchTerm = (term) => setSearchTerm(term);

  return { searchTerm, filteredGenres, updateSearchTerm };
}

export function useDetails(id) {
  const navigate = useNavigate();
  const [genre, setGenre] = useState(null);
  const [isFetching, setIsFetching] = useState(false);

  const { token } = useContext(UserContext);

  useEffect(() => {
    async function fetchData() {
      try {
        setIsFetching(true);
        setGenre(await genreApi.detailsAsync(id, token));
      } catch (error) {
        navigate(routes.badRequest, { state: { message: error.message } });
      } finally {
        setIsFetching(false);
      }
    }

    fetchData();
  }, [token, navigate]);

  return { genre, isFetching };
}
