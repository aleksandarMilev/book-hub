import './AuthorListItem.css';

import { type FC } from 'react';
import { useTranslation } from 'react-i18next';
import { FaPenFancy, FaUser } from 'react-icons/fa';
import { Link } from 'react-router-dom';

import type { AuthorsSearchResult } from '@/features/search/types/search';
import { RenderStars } from '@/shared/components/render-stars/RenderStars';
import { routes } from '@/shared/lib/constants/api';
import { getImageUrl, slugify } from '@/shared/lib/utils/utils';

const AuthorListItem: FC<AuthorsSearchResult> = ({
  id,
  name,
  imagePath,
  averageRating,
  penName,
}) => {
  const { t } = useTranslation('authors');
  const displayName = name || penName || t('list.unknownAuthor');

  return (
    <article className="author-list-item">
      <div className="author-list-item__media">
        <img
          src={getImageUrl(imagePath, 'authors')}
          alt={displayName}
          className="author-card-image"
        />
      </div>
      <div className="author-list-item__content">
        <h5 className="author-list-item-name">
          <FaUser className="me-2" />
          {displayName}
        </h5>
        {penName && (
          <div className="author-list-item-pen-name">
            <FaPenFancy className="me-2" />
            {t('list.penNameLabel', { penName })}
          </div>
        )}
        <div className="author-list-item-rating">
          <RenderStars rating={averageRating ?? 0} />
        </div>
      </div>
      <div className="author-list-item__actions">
        <Link
          to={`${routes.author}/${id}/${slugify(displayName)}`}
          className="author-list-item-btn"
        >
          {t('list.view')}
        </Link>
      </div>
    </article>
  );
};

export default AuthorListItem;


