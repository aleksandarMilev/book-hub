import { useFormik } from 'formik';
import { useNavigate } from 'react-router-dom';

import type { Article, ArticleInput } from '../../../../api/article/types/article';
import { routes } from '../../../../common/constants/api';
import { useMessage } from '../../../../contexts/message/messageContext';
import * as hooks from '../../../../features/article/hooks/useCrud';
import { articleSchema } from '../validation/articleSchema';

export const useArticleFormik = ({
  article = null,
  isEditMode = false,
}: {
  article?: Article | null;
  isEditMode?: boolean;
}) => {
  const navigate = useNavigate();
  const { showMessage } = useMessage();

  const createHandler = hooks.useCreate();
  const editHandler = hooks.useEdit();

  const formik = useFormik<ArticleInput & { imageUrl: string }>({
    initialValues: {
      title: article?.title || '',
      introduction: article?.introduction || '',
      imageUrl: article?.imageUrl || '',
      content: article?.content || '',
    },
    validationSchema: articleSchema,
    onSubmit: async (values, { resetForm }) => {
      try {
        if (isEditMode && article?.id) {
          await editHandler(article.id, values);

          showMessage(`${article.title || 'This article'} was successfully edited!`, true);
          navigate(`${routes.article}/${article.id}`, { replace: true });
        } else {
          const id = await createHandler(values);

          showMessage(`${values.title || 'This article'} was successfully created!`, true);
          navigate(`${routes.article}/${id}`, { replace: true });
          resetForm();
        }
      } catch (error: unknown) {
        const message = error instanceof Error ? error.message : 'Operation failed!';
        showMessage(message, false);
      }
    },
  });

  return formik;
};
