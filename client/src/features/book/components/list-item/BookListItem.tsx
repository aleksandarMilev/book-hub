import './BookListItem.css';

import { type FC } from 'react';
import { useTranslation } from 'react-i18next';
import { FaBook, FaTag, FaUser } from 'react-icons/fa';
import { Link } from 'react-router-dom';

import type { GenreName } from '@/features/genre/types/genre';
import { RenderStars } from '@/shared/components/render-stars/RenderStars';
import { routes } from '@/shared/lib/constants/api';
import { getImageUrl, slugify } from '@/shared/lib/utils/utils';

type Props = {
  id: string;
  imagePath: string;
  title: string;
  authorName?: string | null;
  shortDescription: string;
  averageRating: number;
  genres: GenreName[];
};

const BookListItem: FC<Props> = ({
  id,
  imagePath,
  title,
  authorName,
  shortDescription,
  averageRating = 0,
  genres,
}) => {
  const { t } = useTranslation('books');

  const displayAuthorName = authorName || t('list.unknownAuthor');

  return (
    <div className="row p-3 bg-light border rounded mb-3 shadow-sm book-list-item">
      <div className="col-md-3 col-4 mt-1 d-flex justify-content-center align-items-center">
        <img src={getImageUrl(imagePath, 'books')} alt={title} className="book-list-item-image" />
      </div>
      <div className="col-md-6 col-8 mt-1 book-list-item-content">
        <h5 className="mb-2 book-list-item-title">
          <FaBook className="me-2" />
          {title}
        </h5>
        <h6 className="text-muted mb-2 book-list-item-author">
          <FaUser className="me-2" />
          {t('list.byAuthor', { author: displayAuthorName })}
        </h6>
        <div className="d-flex flex-row mb-2 book-list-item-rating">
          <RenderStars rating={averageRating} />
        </div>
        <div className="mt-1 mb-2 book-list-item-genres">
          <FaTag className="me-2" />
          {genres.length > 0 ? (
            genres.map((g) => (
              <Link key={g.id} to={`${routes.genres}/${g.id}/${slugify(g.name)}`}>
                <span className="badge bg-secondary me-1">{g.name}</span>
              </Link>
            ))
          ) : (
            <span className="text-muted">{t('list.noGenres')}</span>
          )}
        </div>
        {shortDescription && (
          <p className="text-justify para mb-0 book-list-item-description">{shortDescription}</p>
        )}
      </div>
      <div className="col-md-3 d-flex align-items-center justify-content-center mt-1">
        <div className="d-flex flex-column align-items-center">
          <Link
            to={`${routes.book}/${id}/${slugify(title)}`}
            className="btn btn-sm btn-primary book-list-item-btn"
          >
            {t('list.view')}
          </Link>
        </div>
      </div>
    </div>
  );
};

export default BookListItem;


