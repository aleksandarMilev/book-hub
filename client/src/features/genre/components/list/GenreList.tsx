import './GenreList.css';

import { type ChangeEvent, type FC, useState } from 'react';
import { useTranslation } from 'react-i18next';

import emptyImg from '@/assets/images/no-books-found.png';
import GenreListItem from '@/features/genre/components/list-item/GenreListItem';
import { useSearchGenres } from '@/features/search/hooks/useCrud';
import DefaultSpinner from '@/shared/components/default-spinner/DefaultSpinner';
import Pagination from '@/shared/components/pagination/Pagination';
import { useDebounce } from '@/shared/hooks/debounce/useDebounce';
import { pagination } from '@/shared/lib/constants/defaultValues';

const GenreList: FC = () => {
  const { t } = useTranslation('genres');
  const [searchTerm, setSearchTerm] = useState('');
  const debouncedSearchTerm = useDebounce(searchTerm);

  const [page, setPage] = useState(pagination.defaultPageIndex);
  const pageSize = pagination.defaultPageSize;

  const {
    items: genres,
    totalItems,
    isFetching,
  } = useSearchGenres(debouncedSearchTerm, page, pageSize);

  const totalPages = Math.ceil(totalItems / pageSize) || 1;

  const handleSearchChange = (e: ChangeEvent<HTMLInputElement>) => {
    setSearchTerm(e.target.value);
    setPage(pagination.defaultPageIndex);
  };

  const handlePageChange = (newPage: number) => {
    if (newPage < 1 || newPage > totalPages) return;
    setPage(newPage);
  };

  const showEmpty = !isFetching && genres.length === 0;

  return (
    <div className="genre-list-page container">
      <div className="search-wrapper">
        <div className="search-bar">
          <input
            type="text"
            placeholder={t('list.searchPlaceholder')}
            value={searchTerm}
            onChange={handleSearchChange}
          />
        </div>
      </div>
      <div className="genre-container">
        {isFetching && <DefaultSpinner />}
        {!isFetching && !showEmpty && (
          <>
            <div className="genre-list">
              {genres.map((genre) => (
                <GenreListItem key={genre.id} {...genre} />
              ))}
            </div>
            <Pagination
              page={page}
              totalPages={totalPages}
              disabled={isFetching}
              onPageChange={handlePageChange}
            />
          </>
        )}
        {showEmpty && (
          <div className="empty-state">
            <img src={emptyImg} alt={t('list.emptyAlt')} onClick={() => setSearchTerm('')} />
            <h4>{t('list.emptyTitle')}</h4>
            <p>{t('list.emptyMessage')}</p>
          </div>
        )}
      </div>
    </div>
  );
};

export default GenreList;


