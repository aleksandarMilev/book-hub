import { useNavigate } from 'react-router-dom';
import { useFormik } from 'formik';
import { useContext } from 'react';
import { useMessage } from '../../../../contexts/message/messageContext';
import { UserContext } from '../../../../contexts/user/userContext';
import * as authorHooks from '../../../../hooks/useAuthor';
import { authorSchema } from '../validation/authorSchema';
import { routes } from '../../../../common/constants/api';
import type { AuthorFormProps } from '../types/authorFormProps';
import type { AuthorInput } from '../../../../api/author/types/author.type';
import type { AuthorFormValues } from './types/authorFormValues';

export const useAuthorFormik = ({ authorData = null, isEditMode = false }: AuthorFormProps) => {
  const navigate = useNavigate();

  const { isAdmin } = useContext(UserContext);
  const { showMessage } = useMessage();

  const createHandler = authorHooks.useCreate();
  const editHandler = authorHooks.useEdit();

  const initialNationality =
    isEditMode && authorData?.nationality ? authorData.nationality.id : null;

  const initialNationalityName =
    isEditMode && authorData?.nationality ? authorData.nationality.name : '';

  const formik = useFormik<AuthorFormValues>({
    initialValues: {
      name: authorData?.name ?? '',
      penName: authorData?.penName ?? '',
      imageUrl: authorData?.imageUrl ?? '',
      bornAt: authorData?.bornAt ?? '',
      diedAt: authorData?.diedAt ?? '',
      gender: authorData?.gender ?? '',
      nationality: initialNationality,
      nationalityName: initialNationalityName,
      biography: authorData?.biography ?? '',
    },
    validationSchema: authorSchema,
    onSubmit: async (values, { resetForm }) => {
      try {
        const payload: AuthorInput = {
          name: values.name,
          penName: values.penName || null,
          imageUrl: values.imageUrl || null,
          bornAt: values.bornAt || null,
          diedAt: values.diedAt || null,
          gender: values.gender,
          nationality: (values.nationality ?? initialNationality) || null,
          biography: values.biography,
        };

        if (authorData && isEditMode) {
          await editHandler(authorData.id, payload);

          showMessage(`${authorData.name || 'This author'} was successfully edited!`, true);
          navigate(`${routes.author}/${authorData.id}`, { replace: true });
        } else {
          const id = await createHandler(payload);
          if (id) {
            showMessage(
              isAdmin
                ? 'Author successfully created!'
                : 'Thank you for being part of our community! Our admin team will process your author soon.',
              true,
            );

            navigate(isAdmin ? `${routes.author}/${id}` : routes.home, { replace: true });
            resetForm();
          }
        }
      } catch (error: any) {
        showMessage(error?.message || 'Something went wrong, please try again!', false);
      }
    },
  });

  return formik;
};
