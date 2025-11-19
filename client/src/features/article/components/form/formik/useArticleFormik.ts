import { useFormik } from 'formik';
import { useNavigate } from 'react-router-dom';

import {
  articleSchema,
  type ArticleFormValues,
} from '@/features/article/components/form/validation/articleSchema.js';
import { useCreate, useEdit } from '@/features/article/hooks/useCrud.js';
import type { ArticleDetails, CreateArticle } from '@/features/article/types/article.js';
import { routes } from '@/shared/lib/constants/api.js';
import { IsError } from '@/shared/lib/utils.js';
import { useMessage } from '@/shared/stores/message/message.js';

type Props = {
  article?: ArticleDetails | null;
  isEditMode?: boolean;
};

export const useArticleFormik = ({ article = null, isEditMode = false }: Props) => {
  const navigate = useNavigate();
  const { showMessage } = useMessage();

  const createHandler = useCreate();
  const editHandler = useEdit();

  const formik = useFormik<ArticleFormValues>({
    initialValues: {
      title: article?.title ?? '',
      introduction: article?.introduction ?? '',
      imageUrl: article?.imageUrl ?? '',
      content: article?.content ?? '',
    },
    validationSchema: articleSchema,
    onSubmit: async (values, { resetForm }) => {
      try {
        const castValues = articleSchema.cast(values) as ArticleFormValues;
        const payload: CreateArticle = {
          title: castValues.title,
          introduction: castValues.introduction,
          content: castValues.content,
          imageUrl: castValues.imageUrl ?? null,
        };

        if (isEditMode && article?.id) {
          await editHandler(article.id, payload);

          showMessage(`${article.title || 'This article'} was successfully updated!`, true);
          navigate(`${routes.article}/${article.id}`, { replace: true });
        } else {
          const id = await createHandler(payload);

          showMessage(`${payload.title || 'This article'} was successfully created!`, true);
          navigate(`${routes.article}/${id}`, { replace: true });
          resetForm();
        }
      } catch (error) {
        const message = IsError(error) ? error.message : 'Operation failed!';
        showMessage(message, false);
      }
    },
  });

  return formik;
};
