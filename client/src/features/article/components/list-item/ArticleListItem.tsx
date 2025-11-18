import './ArticleListItem.css';
import type { FC } from 'react';
import { Link } from 'react-router-dom';

import type { ArticlesSearchResult } from '@/features/search/types/search.js';
import { routes } from '@/shared/lib/constants/api.js';
import { formatIsoDate } from '@/shared/lib/utils.js';

const ArticleListItem: FC<ArticlesSearchResult> = ({
  id,
  title,
  introduction,
  imageUrl,
  createdOn,
}) => {
  return (
    <div className="article-card fade-in">
      <div className="article-card-image-wrapper">
        {imageUrl ? (
          <img src={imageUrl} alt={title} className="article-card-image" />
        ) : (
          <div className="article-card-image-placeholder">No Image</div>
        )}
      </div>
      <div className="article-card-content">
        <Link to={`${routes.article}/${id}`} className="article-card-title">
          {title}
        </Link>
        <p className="article-card-intro">{introduction}</p>
        <p className="article-card-date">
          Published on {formatIsoDate(createdOn, 'Published date not available.')}
        </p>
      </div>
    </div>
  );
};

export default ArticleListItem;
