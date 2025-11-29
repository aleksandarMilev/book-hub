import './AuthorIntroduction.css';

import { t } from 'i18next';
import type { FC } from 'react';
import { FaBook } from 'react-icons/fa';
import { Link } from 'react-router-dom';

import type { Author } from '@/features/author/types/author.js';
import { routes } from '@/shared/lib/constants/api.js';
import { getImageUrl, slugify } from '@/shared/lib/utils/utils.js';

type Props = { author?: Author | null };

const AuthorIntroduction: FC<Props> = ({ author }) => {
  if (!author) {
    return (
      <div className="author-intro-card">
        <h3 className="author-intro-title">About the Author</h3>
        <div className="author-intro-placeholder">
          <p className="text-center">The book author is unknown.</p>
        </div>
      </div>
    );
  }

  const previewLength = 200;
  const bio = author.biography ?? '';
  const previewBio = bio.slice(0, previewLength);
  const booksCount = author.booksCount ?? 0;

  return (
    <div className="author-intro-card">
      <h3 className="author-intro-title">About the Author</h3>
      <div className="author-intro-header">
        <div className="author-intro-image-container">
          {author.imagePath ? (
            <img
              src={getImageUrl(author.imagePath, 'authors')}
              alt={author.name}
              className="author-details-image"
            />
          ) : (
            <div className="author-details-image-placeholder">
              {t('author:details.image.noImage')}
            </div>
          )}
          <div className="author-intro-info">
            <h4 className="author-name">{author.name}</h4>
            <div className="author-books-count">
              <FaBook className="author-book-icon" />
              <p>
                {booksCount} {booksCount === 1 ? 'Book' : 'Books'}
              </p>
            </div>
          </div>
        </div>
      </div>
      <div className="author-intro-bio">
        <p className="author-bio-text">
          {previewBio}
          {bio.length > previewLength && <span className="see-more">...</span>}
          <Link
            to={`${routes.author}/${author.id}/${slugify(author.name)}`}
            className="see-more-link"
          >
            See More
          </Link>
        </p>
      </div>
    </div>
  );
};

export default AuthorIntroduction;
