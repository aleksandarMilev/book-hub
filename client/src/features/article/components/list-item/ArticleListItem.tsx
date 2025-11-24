import './ArticleListItem.css';

import type { FC } from 'react';
import { useTranslation } from 'react-i18next';
import { Link } from 'react-router-dom';

import type { ArticlesSearchResult } from '@/features/search/types/search.js';
import { routes } from '@/shared/lib/constants/api.js';
import { formatIsoDate, getImageUrl, slugify } from '@/shared/lib/utils/utils.js';

const ArticleListItem: FC<ArticlesSearchResult> = ({
  id,
  title,
  introduction,
  imagePath,
  createdOn,
}) => {
  const { t } = useTranslation('articles');

  const dateText = formatIsoDate(createdOn, t('list.dateUnavailable'));

  return (
    <div className="article-card fade-in">
      <div className="article-card-image-wrapper">
        <img src={getImageUrl(imagePath, 'articles')} alt={title} className="article-card-image" />
      </div>
      <div className="article-card-content">
        <Link to={`${routes.articles}/${id}/${slugify(title)}`} className="article-card-title">
          {title}
        </Link>
        <p className="article-card-intro">{introduction}</p>
        <p className="article-card-date">{t('list.publishedOn', { date: dateText })}</p>
      </div>
    </div>
  );
};

export default ArticleListItem;
