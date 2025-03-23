import { useParams } from "react-router-dom";

import * as useAuthor from "../../../hooks/useAuthor";

import AuthorForm from "../author-form/AuthorForm";
import DefaultSpinner from "../../common/default-spinner/DefaultSpinner";

export default function EditAuthor() {
  const { id } = useParams();
  const { author, isFetching } = useAuthor.useGetDetails(id);

  return isFetching ? (
    <DefaultSpinner />
  ) : (
    <AuthorForm authorData={author} isEditMode={true} />
  );
}
