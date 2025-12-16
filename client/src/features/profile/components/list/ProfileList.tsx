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

  return (
    <div className="profile-list-page container">
      <div className="row mb-4">
        <div className="col-md-10 mx-auto d-flex">
          <div className="search-bar-container d-flex w-100">
            <input
              type="text"
              className="form-control search-input"
              placeholder={t('list.searchPlaceholder')}
              value={searchTerm}
              onChange={handleSearchChange}
              disabled={isFetching}
            />
            <button
              className="btn btn-light search-btn"
              disabled={isFetching}
              aria-label={t('list.searchAria')}
            >
              <FaSearch size={20} />
            </button>
          </div>
        </div>
      </div>
      <div className="d-flex justify-content-center row">
        <div className="col-md-10">
          {isFetching ? (
            <DefaultSpinner />
          ) : profiles.length > 0 ? (
            <>
              {profiles.map((p) => {
                const mapped = {
                  id: p.id.toString(),
                  imageUrl: p.imageUrl ?? '',
                  firstName: p.firstName,
                  lastName: p.lastName,
                  isPrivate: p.isPrivate,
                };
                return <ProfileListItem key={mapped.id} {...mapped} />;
              })}
              <Pagination
                page={page}
                totalPages={totalPages}
                disabled={isFetching}
                onPageChange={handlePageChange}
              />
            </>
          ) : (
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
    </div>
  );
};

export default ProfileList;
