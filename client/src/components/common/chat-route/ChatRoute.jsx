import { useContext, useState, useEffect } from "react";
import { Navigate, useParams } from "react-router-dom";

import * as api from "../../../api/chatApi";
import { UserContext } from "../../../contexts/userContext";
import { routes } from "../../../common/constants/api";

export default function ChatRoute({ element }) {
  const { id } = useParams();
  const [isLoading, setIsLoading] = useState(true);
  const [hasAccess, setHasAccess] = useState(false);

  const { token, isAuthenticated, userId } = useContext(UserContext);

  if (!isAuthenticated) {
    return <Navigate to={routes.login} replace />;
  }

  useEffect(() => {
    api
      .hasAccessAsync(id, userId, token)
      .then((access) => setHasAccess(access))
      .finally(() => setIsLoading(false));
  }, [id, userId, token]);

  if (!hasAccess && !isLoading) {
    return <Navigate to={routes.home} replace />;
  }

  return element;
}
