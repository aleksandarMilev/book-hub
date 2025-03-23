import { useParams } from "react-router-dom";

import * as useArticle from "../../../hooks/useArticle";

import ArticleForm from "../article-form/ArticleForm";
import DefaultSpinner from "../../common/default-spinner/DefaultSpinner";

export default function EditArticle() {
  const { id } = useParams();
  const { article } = useArticle.useDetails(id);

  return article ? (
    <ArticleForm article={article} isEditMode={true} />
  ) : (
    <DefaultSpinner />
  );
}
