import { useContext, useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { format } from "date-fns";

import * as articleApi from "../api/articleApi";
import { routes } from "../common/constants/api";
import { UserContext } from "../contexts/userContext";

export function useDetails(id) {
  const { token } = useContext(UserContext);

  const navigate = useNavigate();
  const [article, setArticle] = useState(null);
  const [isFetching, setIsFetching] = useState(false);

  useEffect(() => {
    async function fetchData() {
      try {
        setIsFetching(true);

        const articleData = await articleApi.detailsAsync(id, token);
        const article = {
          ...articleData,
          createdOn: format(new Date(articleData.createdOn), "yyyy-MM-dd"),
        };

        setArticle(article);
      } catch (error) {
        navigate(routes.notFound, { state: { message: error.message } });
      } finally {
        setIsFetching(false);
      }
    }

    fetchData();
  }, [token, navigate]);

  return { article, isFetching };
}

export function useCreate() {
  const { token } = useContext(UserContext);

  const navigate = useNavigate();

  const createHandler = async (articleData) => {
    const article = {
      ...articleData,
      imageUrl: articleData.imageUrl || null,
    };

    try {
      const id = await articleApi.createAsync(article, token);
      return id;
    } catch (error) {
      navigate(routes.badRequest, { state: { message: error.message } });
    }
  };

  return createHandler;
}

export function useEdit() {
  const { token } = useContext(UserContext);

  const navigate = useNavigate();

  const editHandler = async (id, articleData) => {
    const article = {
      ...articleData,
      imageUrl: articleData.imageUrl || null,
    };

    try {
      await articleApi.editAsync(id, article, token);
    } catch (error) {
      navigate(routes.badRequest, { state: { message: error.message } });
    }
  };

  return editHandler;
}
