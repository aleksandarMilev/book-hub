import './AuthorListItem.css';

import { type FC } from 'react';
import { useTranslation } from 'react-i18next';
import { FaPenFancy, FaUser } from 'react-icons/fa';
import { Link } from 'react-router-dom';

import type { AuthorsSearchResult } from '@/features/search/types/search.js';
import { RenderStars } from '@/shared/components/render-stars/RenderStars.js';
import { routes } from '@/shared/lib/constants/api.js';
import { getImageUrl, slugify } from '@/shared/lib/utils/utils.js';

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
    <div className="row p-3 bg-light border rounded mb-3 shadow-sm author-list-item">
      <div className="col-md-3 col-4 mt-1 d-flex justify-content-center align-items-center">
        <img
          src={getImageUrl(imagePath, 'authors')}
          alt={displayName}
          className="author-card-image"
        />
      </div>
      <div className="col-md-6 col-8 mt-1 author-list-item-content">
        <h5 className="mb-2 author-list-item-name">
          <FaUser className="me-2" />
          {displayName}
        </h5>
        {penName && (
          <h6 className="text-muted mb-2 author-list-item-pen-name">
            <FaPenFancy className="me-2" />
            {t('list.penNameLabel', { penName })}
          </h6>
        )}
        <div className="d-flex flex-row mb-2 author-list-item-rating">
          <RenderStars rating={averageRating ?? 0} />
        </div>
      </div>
      <div className="col-md-3 d-flex align-items-center justify-content-center mt-1">
        <div className="d-flex flex-column align-items-center">
          <Link
            to={`${routes.author}/${id}/${slugify(displayName)}`}
            className="btn btn-sm btn-primary author-list-item-btn"
          >
            {t('list.view')}
          </Link>
        </div>
      </div>
    </div>
  );
};

export default AuthorListItem;
