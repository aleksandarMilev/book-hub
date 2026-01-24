import './ChatList.css';

import { type ChangeEvent, type FC, useEffect, useState } from 'react';
import { FaSearch } from 'react-icons/fa';
import { useTranslation } from 'react-i18next';

import image from '@/features/chat/components/form/assets/chat.avif';
import ChatListItem from '@/features/chat/components/list-item/ChatListItem.js';
import { useSearchChats } from '@/features/search/hooks/useCrud.js';
import DefaultSpinner from '@/shared/components/default-spinner/DefaultSpinner.js';
import Pagination from '@/shared/components/pagination/Pagination.js';
import { useDebounce } from '@/shared/hooks/debounce/useDebounce.js';
import { pagination } from '@/shared/lib/constants/defaultValues.js';

const ChatList: FC = () => {
  const { t } = useTranslation('chats');

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
    <div className="chat-list-page container">
      <div className="search-wrapper">
        <div className="search-bar">
          <FaSearch className="search-icon" />
          <input
            type="text"
            placeholder={t('list.search.placeholder')}
            value={searchTerm}
            onChange={handleSearchChange}
            aria-label={t('list.search.ariaLabel')}
          />
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
                alt={t('list.empty.imageAlt')}
                className="mb-4 clickable"
                style={{ maxWidth: '200px', opacity: 0.7, cursor: 'pointer' }}
                onClick={clearSearch}
              />
              <h5 className="text-muted">{t('list.empty.title')}</h5>
              <p className="text-muted text-center" style={{ maxWidth: '400px' }}>
                {t('list.empty.message')}
              </p>
            </div>
          )}
        </div>
      </div>
    </div>
  );
};

export default ChatList;
