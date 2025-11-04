import type { Article } from '../../../../api/article/types/article.type';

export interface ArticleFormProps {
  article?: Article | null;
  isEditMode?: boolean;
}
