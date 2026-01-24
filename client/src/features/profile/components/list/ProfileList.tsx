import './ProfileList.css';

import { type ChangeEvent, type FC, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { FaSearch } from 'react-icons/fa';

import noUsersImage from '@/features/profile/components/list/assets/no-users-found.avif';
import ProfileListItem from '@/features/profile/components/list-item/ProfileListItem.js';
import { useSearchProfiles } from '@/features/search/hooks/useCrud.js';
import DefaultSpinner from '@/shared/components/default-spinner/DefaultSpinner.js';
import Pagination from '@/shared/components/pagination/Pagination.js';
import { useDebounce } from '@/shared/hooks/debounce/useDebounce.js';
import { pagination } from '@/shared/lib/constants/defaultValues.js';

const ProfileList: FC = () => {
  const [searchTerm, setSearchTerm] = useState('');
  const debouncedSearchTerm = useDebounce(searchTerm);
  const { t } = useTranslation('profiles');

  const [page, setPage] = useState(pagination.defaultPageIndex);
  const pageSize = pagination.defaultPageSize;

  const {
    items: profiles,
    totalItems,
    isFetching,
  } = useSearchProfiles(debouncedSearchTerm, page, pageSize);

  const totalPages = Math.ceil(totalItems / pageSize) || 1;

  const handleSearchChange = (e: ChangeEvent<HTMLInputElement>) => {
    setSearchTerm(e.target.value);
    setPage(pagination.defaultPageIndex);
  };

  const handlePageChange = (newPage: number) => {
    if (newPage < 1 || newPage > totalPages) return;
    setPage(newPage);
  };

  const showEmpty = !isFetching && profiles.length === 0;

  return (
    <div className="profile-list-page container">
      <div className="search-wrapper">
        <div className="search-bar">
          <FaSearch className="search-icon" />
          <input
            type="text"
            placeholder={t('list.searchPlaceholder')}
            value={searchTerm}
            onChange={handleSearchChange}
            disabled={isFetching}
            aria-label={t('list.searchAria')}
          />
        </div>
      </div>

      <div className="profile-container">
        {isFetching && <DefaultSpinner />}

        {!isFetching && !showEmpty && (
          <>
            <div className="profile-list">
              {profiles.map((p) => (
                <ProfileListItem
                  key={p.id}
                  id={p.id.toString()}
                  imagePath={p.imagePath ?? ''}
                  firstName={p.firstName}
                  lastName={p.lastName}
                  isPrivate={p.isPrivate}
                />
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
          <div className="d-flex flex-column align-items-center justify-content-center mt-5">
            <img
              src={noUsersImage}
              alt="No users found"
              className="empty-state-image"
              onClick={() => setSearchTerm('')}
            />
            <h5 className="empty-state-title text-center">{t('list.emptyTitle')}</h5>
            <p className="empty-state-text text-center">{t('list.emptyText')}</p>
          </div>
        )}
      </div>
    </div>
  );
};

export default ProfileList;
