import { useFormik } from 'formik';
import { useNavigate } from 'react-router-dom';

import { authorSchema } from '@/features/author/components/form/validation/authorSchema.js';
import { useCreate, useEdit } from '@/features/author/hooks/useCrud.js';
import type { AuthorDetails, CreateAuthor } from '@/features/author/types/author.js';
import { routes } from '@/shared/lib/constants/api.js';
import { IsError } from '@/shared/lib/utils/utils.js';
import { useAuth } from '@/shared/stores/auth/auth.js';
import { useMessage } from '@/shared/stores/message/message.js';

export type AuthorFormValues = {
  name: string;
  penName: string;
  imageUrl: string;
  bornAt: string;
  diedAt: string;
  gender: '' | 'male' | 'female' | 'other';
  nationality: number | null;
  nationalityName: string;
  biography: string;
};

export const useAuthorFormik = ({
  authorData = null,
  isEditMode = false,
}: {
  authorData?: AuthorDetails | null;
  isEditMode?: boolean;
}) => {
  const navigate = useNavigate();

  const { isAdmin } = useAuth();
  const { showMessage } = useMessage();

  const createHandler = useCreate();
  const editHandler = useEdit();

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
      gender: (authorData?.gender as AuthorFormValues['gender']) ?? '',
      nationality: initialNationality,
      nationalityName: initialNationalityName,
      biography: authorData?.biography ?? '',
    },
    validationSchema: authorSchema,
    onSubmit: async (values, { resetForm }) => {
      try {
        const nationalityId = values.nationality ?? initialNationality ?? null;

        const payload: CreateAuthor = {
          name: values.name,
          penName: values.penName || '',
          imageUrl: values.imageUrl || '',
          bornAt: values.bornAt || '',
          diedAt: values.diedAt || '',
          gender: values.gender,
          nationalityId: nationalityId !== null ? String(nationalityId) : '',
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
      } catch (error) {
        const message = IsError(error) ? error.message : 'Something went wrong, please try again!';
        showMessage(message, false);
      }
    },
  });

  return formik;
};
