import { useFormik } from 'formik';
import { useNavigate } from 'react-router-dom';

import {
  type AuthorFormValues,
  authorSchema,
} from '@/features/author/components/form/validation/authorSchema.js';
import { useCreate, useEdit } from '@/features/author/hooks/useCrud.js';
import type { AuthorDetails, CreateAuthor } from '@/features/author/types/author.js';
import { routes } from '@/shared/lib/constants/api.js';
import { IsError } from '@/shared/lib/utils/utils.js';
import { useAuth } from '@/shared/stores/auth/auth.js';
import { useMessage } from '@/shared/stores/message/message.js';

type Props = {
  authorData?: AuthorDetails | null;
  isEditMode?: boolean;
};

export const useAuthorFormik = ({ authorData = null, isEditMode = false }: Props) => {
  const { isAdmin } = useAuth();
  const navigate = useNavigate();
  const { showMessage } = useMessage();

  const createHandler = useCreate();
  const editHandler = useEdit();

  const initialNationality = isEditMode && authorData ? authorData.nationality : null;

  const formik = useFormik<AuthorFormValues>({
    initialValues: {
      name: authorData?.name ?? '',
      penName: authorData?.penName ?? '',
      image: null,
      bornAt: null,
      diedAt: null,
      gender: (authorData?.gender as AuthorFormValues['gender']) ?? '',
      nationality: initialNationality,
      biography: authorData?.biography ?? '',
    },
    validationSchema: authorSchema,
    onSubmit: async (values, { resetForm }) => {
      try {
        const castValues = authorSchema.cast(values) as AuthorFormValues;
        const normalizeDate = (value: unknown): string | null | undefined => {
          if (!value) {
            return null;
          }

          if (value instanceof Date && !isNaN(value.getTime())) {
            return value.toISOString().split('T')[0];
          }

          if (typeof value === 'string' && value.trim() !== '') {
            return value;
          }

          return null;
        };

        const payload: CreateAuthor = {
          name: castValues.name,
          penName: castValues.penName ?? null,
          image: castValues.image ?? null,
          bornAt: normalizeDate(castValues.bornAt),
          diedAt: normalizeDate(castValues.diedAt),
          gender: castValues.gender as CreateAuthor['gender'],
          nationality: castValues.nationality ?? null,
          biography: castValues.biography,
        };

        if (authorData && isEditMode) {
          await editHandler(authorData.id, payload);

          showMessage(`${authorData.name || 'This author'} was successfully edited!`, true);
          navigate(`${routes.author}/${authorData.id}`, { replace: true });
        } else {
          const created = await createHandler(payload);

          if (created) {
            showMessage(
              isAdmin
                ? 'Author successfully created!'
                : 'Thank you for being part of our community! Our admin team will process your author soon.',
              true,
            );

            navigate(isAdmin ? `${routes.author}/${created.id}` : routes.home, { replace: true });
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
