import { type ChangeEvent, type FC, useEffect, useState } from 'react';
import { FaSearch } from 'react-icons/fa';

import image from '@/features/chat/components/form/assets/chat.avif';
import ChatListItem from '@/features/chat/components/list-item/ChatListItem.js';
import { useSearchChats } from '@/features/search/hooks/useCrud.js';
import DefaultSpinner from '@/shared/components/default-spinner/DefaultSpinner.js';
import Pagination from '@/shared/components/pagination/Pagination.js';
import { useDebounce } from '@/shared/hooks/debounce/useDebounce.js';
import { pagination } from '@/shared/lib/constants/defaultValues.js';

const ChatList: FC = () => {
  const [searchTerm, setSearchTerm] = useState('');
  const debouncedSearch = useDebounce(searchTerm);

  const [page, setPage] = useState(pagination.defaultPageIndex);
  const pageSize = pagination.defaultPageSize;

  useEffect(() => {
    setPage(pagination.defaultPageIndex);
  }, [debouncedSearch]);

  const { items: chats, totalItems, isFetching } = useSearchChats(debouncedSearch, page, pageSize);

  const totalPages = Math.max(1, Math.ceil(totalItems / pageSize));

  const handleSearchChange = (e: ChangeEvent<HTMLInputElement>) => {
    setSearchTerm(e.target.value);
    setPage(pagination.defaultPageIndex);
  };

  const handlePageChange = (newPage: number) => {
    if (newPage < 1 || newPage > totalPages) {
      return;
    }

    setPage(newPage);
  };

  const clearSearch = () => {
    setSearchTerm('');
    setPage(pagination.defaultPageIndex);
  };

  return (
    <div className="container mt-5 mb-5">
      <div className="row mb-4">
        <div className="col-md-10 mx-auto d-flex">
          <div className="search-bar-container d-flex w-100">
            <input
              type="text"
              className="form-control search-input"
              placeholder="Search chats..."
              value={searchTerm}
              onChange={handleSearchChange}
              disabled={isFetching}
            />
            <button
              type="button"
              className="btn btn-light search-btn"
              disabled={isFetching}
              aria-label="Search chats"
              onClick={() => {
                if (searchTerm.trim().length > 0) {
                  clearSearch();
                }
              }}
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
          ) : chats.length > 0 ? (
            <>
              {chats.map((c) => (
                <ChatListItem key={c.id} {...c} />
              ))}
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
                alt="No chats found"
                className="mb-4 clickable"
                style={{ maxWidth: '200px', opacity: 0.7, cursor: 'pointer' }}
                onClick={clearSearch}
              />
              <h5 className="text-muted">We couldn&apos;t find any chats</h5>
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

export default ChatList;
