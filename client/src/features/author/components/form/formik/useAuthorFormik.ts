import { useFormik } from 'formik';
import { useTranslation } from 'react-i18next';
import { useNavigate } from 'react-router-dom';

import {
  type AuthorFormValues,
  authorSchema,
} from '@/features/author/components/form/validation/authorSchema.js';
import { useCreate, useEdit } from '@/features/author/hooks/useCrud.js';
import type { AuthorDetails, CreateAuthor } from '@/features/author/types/author.js';
import { routes } from '@/shared/lib/constants/api.js';
import { IsError, slugify } from '@/shared/lib/utils/utils.js';
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
  const { t } = useTranslation('authors');

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

          const nameForMessage = authorData.name || t('form.fallbackName');
          showMessage(t('form.messages.updateSuccess', { name: nameForMessage }), true);

          navigate(`${routes.author}/${authorData.id}/${slugify(payload.name)}`, { replace: true });
        } else {
          const created = await createHandler(payload);

          if (created) {
            const nameForMessage = payload.name || t('form.fallbackName');
            const messageKey = isAdmin
              ? 'form.messages.createSuccessAdmin'
              : 'form.messages.createSuccessUser';

            showMessage(t(messageKey, { name: nameForMessage }), true);

            navigate(
              isAdmin ? `${routes.author}/${created.id}/${slugify(payload.name)}` : routes.home,
              { replace: true },
            );
            resetForm();
          }
        }
      } catch (error) {
        const message = IsError(error) ? error.message : t('form.messages.operationFailed');
        showMessage(message, false);
      }
    },
  });

  return formik;
};
