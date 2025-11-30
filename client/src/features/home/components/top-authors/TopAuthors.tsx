import './TopAuthors.css';

import { MDBTable, MDBTableBody, MDBTableHead } from 'mdb-react-ui-kit';
import { type FC } from 'react';
import { useTranslation } from 'react-i18next';
import { FaBookReader } from 'react-icons/fa';
import { Link } from 'react-router-dom';

import { useTopThree } from '@/features/author/hooks/useCrud.js';
import DefaultSpinner from '@/shared/components/default-spinner/DefaultSpinner.js';
import EmptyState from '@/shared/components/empty-state/EmptyState.js';
import HomePageError from '@/shared/components/errors/home-page/HomePageError.js';
import { RenderStars } from '@/shared/components/render-stars/RenderStars.js';
import { routes } from '@/shared/lib/constants/api.js';
import { getImageUrl } from '@/shared/lib/utils/utils.js';

const TopAuthors: FC = () => {
  const { t } = useTranslation('home');
  const { authors, isFetching, error } = useTopThree();

  if (error) {
    return <HomePageError message={error} />;
  }

  if (isFetching) {
    return <DefaultSpinner />;
  }

  if (!authors?.length) {
    return (
      <EmptyState
        icon={<FaBookReader />}
        title={t('topAuthors.emptyTitle')}
        message={t('topAuthors.emptyMessage')}
      />
    );
  }

  return (
    <>
      <h2 className="text-center my-5 top-authors-title">{t('topAuthors.title')}</h2>
      <div className="authors-wrapper">
        <MDBTable align="middle" className="table-striped">
          <MDBTableHead>
            <tr className="table-header">
              <th scope="col">{t('topAuthors.table.author')}</th>
              <th scope="col">{t('topAuthors.table.totalBooks')}</th>
              <th scope="col">{t('topAuthors.table.rating')}</th>
              <th scope="col" />
            </tr>
          </MDBTableHead>
          <MDBTableBody>
            {authors.map((a) => (
              <tr key={a.id} className="author-row">
                <td>
                  <div className="d-flex align-items-center">
                    <img
                      src={getImageUrl(a.imagePath, 'authors')}
                      alt={a.name}
                      className="author-image me-3"
                    />
                    <div>
                      <p className="author-name mb-1">{a.name}</p>
                    </div>
                  </div>
                </td>
                <td>{a.booksCount}</td>
                <td className="rating-cell">
                  <RenderStars rating={a.averageRating ?? 0} />
                </td>
                <td>
                  <Link to={`${routes.author}/${a.id}`} className="view-button">
                    {t('topAuthors.table.view')}
                  </Link>
                </td>
              </tr>
            ))}
          </MDBTableBody>
        </MDBTable>
      </div>
    </>
  );
};

export default TopAuthors;
