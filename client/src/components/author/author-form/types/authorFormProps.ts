import type { Author } from '../../../../api/author/types/author.type';

export interface AuthorFormProps {
  authorData?: Author | null;
  isEditMode?: boolean;
}
