import { type FC, useState, type ChangeEvent } from 'react';
import { FaSearch } from 'react-icons/fa';

import type { ProfileListItemProps } from '../../../api/profile/types/profile';
import image from '../../../assets/images/no-books-found.png';
import { pagination } from '../../../common/constants/defaultValues';
import { useDebounce } from '../../../hooks/common/useDebounce';
import * as hooks from '../../../hooks/useSearch';
import DefaultSpinner from '../../common/default-spinner/DefaultSpinner';
import Pagination from '../../common/pagination/Pagination';

import ProfileListItem from '../profile-list-item/ProfileListItem';


const ProfileList: FC = () => {
  const [searchTerm, setSearchTerm] = useState('');
  const debouncedSearchTerm = useDebounce(searchTerm);

  const [page, setPage] = useState(pagination.defaultPageIndex);
  const pageSize = pagination.defaultPageSize;

  const {
    items: profiles,
    totalItems,
    isFetching,
  } = hooks.useSearchProfiles(debouncedSearchTerm, page, pageSize);

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
    <div className="container mt-5 mb-5">
      <div className="row mb-4">
        <div className="col-md-10 mx-auto d-flex">
          <div className="search-bar-container d-flex w-100">
            <input
              type="text"
              className="form-control search-input"
              placeholder="Search users..."
              value={searchTerm}
              onChange={handleSearchChange}
              disabled={isFetching}
            />
            <button
              className="btn btn-light search-btn"
              disabled={isFetching}
              aria-label="Search users"
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
                const mapped: ProfileListItemProps = {
                  id: p.id.toString(),
                  imageUrl: p.imageUrl ?? null,
                  firstName: p.username,
                  lastName: '',
                  isPrivate: false,
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
                src={image}
                alt="No users found"
                className="mb-4 clickable"
                style={{ maxWidth: '200px', opacity: 0.7, cursor: 'pointer' }}
                onClick={() => setSearchTerm('')}
              />
              <h5 className="text-muted">We couldn't find any users</h5>
              <p className="text-muted text-center" style={{ maxWidth: '400px' }}>
                Try adjusting your search terms or exploring our collection for more options.
              </p>
            </div>
          )}
        </div>
      </div>
    </div>
  );
};

export default ProfileList;
