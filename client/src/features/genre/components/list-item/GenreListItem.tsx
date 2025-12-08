import './GenreListItem.css';

import { type FC } from 'react';
import { useTranslation } from 'react-i18next';
import { Link } from 'react-router-dom';

import type { GenresSearchResult } from '@/features/search/types/search.js';
import { routes } from '@/shared/lib/constants/api.js';
import { getImageUrl, slugify } from '@/shared/lib/utils/utils.js';

const GenreListItem: FC<GenresSearchResult> = ({ id, name, imagePath }) => {
  const { t } = useTranslation('genres');

  return (
    <div className="row p-3 bg-light border rounded mb-3 shadow-sm genre-list-item">
      <div className="col-md-3 col-4 mt-1 d-flex justify-content-center align-items-center">
        <img src={getImageUrl(imagePath, 'genres')} alt={name} className="genre-card-image" />
      </div>
      <div className="col-md-6 col-8 mt-1 genre-list-item-content">
        <h5 className="mb-2 genre-list-item-name">{name}</h5>
      </div>
      <div className="col-md-3 d-flex align-items-center justify-content-center mt-1">
        <div className="d-flex flex-column align-items-center">
          <Link
            to={`${routes.genres}/${id}/${slugify(name)}`}
            className="btn btn-sm btn-primary genre-list-item-btn"
          >
            {t('list.view')}
          </Link>
        </div>
      </div>
    </div>
  );
};

export default GenreListItem;
