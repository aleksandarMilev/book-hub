import { useNavigate } from 'react-router-dom';
import { useFormik } from 'formik';
import { useMessage } from '../../../../contexts/message/messageContext';
import * as hooks from '../../../../hooks/useArticle';
import { articleSchema } from '../validation/articleSchema';
import { routes } from '../../../../common/constants/api';
import type { ArticleInput } from '../../../../api/article/types/article.type';
import type { ArticleFormProps } from '../types/articleFormProps';

export const useArticleFormik = ({ article = null, isEditMode = false }: ArticleFormProps) => {
  const navigate = useNavigate();
  const { showMessage } = useMessage();

  const createHandler = hooks.useCreate();
  const editHandler = hooks.useEdit();

  const formik = useFormik<ArticleInput>({
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
      } catch (error: any) {
        showMessage(error.message, false);
      }
    },
  });

  return formik;
};
