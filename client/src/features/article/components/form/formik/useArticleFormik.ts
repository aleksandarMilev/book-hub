import { useFormik } from 'formik';
import { useNavigate } from 'react-router-dom';

import { articleSchema } from '@/features/article/components/form/validation/articleSchema.js';
import { useCreate, useEdit } from '@/features/article/hooks/useCrud.js';
import type { ArticleDetails, CreateArticle } from '@/features/article/types/article.js';
import { routes } from '@/shared/lib/constants/api.js';
import { IsError } from '@/shared/lib/utils.js';
import { useMessage } from '@/shared/stores/message/message.js';

export const useArticleFormik = ({
  article = null,
  isEditMode = false,
}: {
  article?: ArticleDetails | null;
  isEditMode?: boolean;
}) => {
  const navigate = useNavigate();
  const { showMessage } = useMessage();

  const createHandler = useCreate();
  const editHandler = useEdit();

  const formik = useFormik<CreateArticle & { imageUrl: string }>({
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

          showMessage(`${article.title || 'This article'} was successfully updated!`, true);
          navigate(`${routes.article}/${article.id}`, { replace: true });
        } else {
          const id = await createHandler(values);

          showMessage(`${values.title || 'This article'} was successfully created!`, true);
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
