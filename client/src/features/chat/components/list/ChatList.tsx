import './ChatList.css';

import { type ChangeEvent, type FC, useEffect, useState } from 'react';
import { FaSearch } from 'react-icons/fa';
import { useTranslation } from 'react-i18next';

import image from '@/features/chat/components/form/assets/chat.avif';
import ChatListItem from '@/features/chat/components/list-item/ChatListItem';
import { useSearchChats } from '@/features/search/hooks/useCrud';
import DefaultSpinner from '@/shared/components/default-spinner/DefaultSpinner';
import Pagination from '@/shared/components/pagination/Pagination';
import { useDebounce } from '@/shared/hooks/debounce/useDebounce';
import { pagination } from '@/shared/lib/constants/defaultValues';

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

  const handleSearchChange = (event: ChangeEvent<HTMLInputElement>) => {
    setSearchTerm(event.target.value);
  };

  const handlePageChange = (newPage: number) => {
    if (newPage < 1 || newPage > totalPages) return;
    setPage(newPage);
  };

  const clearSearch = () => {
    setSearchTerm('');
    setPage(pagination.defaultPageIndex);
  };

  const showEmpty = !isFetching && chats.length === 0;

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

      <div className="chat-container">
        {isFetching && <DefaultSpinner />}
        {!isFetching && !showEmpty && (
          <>
            <div className="chat-list">
              {chats.map((c) => (
                <ChatListItem key={c.id} {...c} />
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
            <img src={image} alt={t('list.empty.imageAlt')} onClick={clearSearch} />
            <h4>{t('list.empty.title')}</h4>
            <p>{t('list.empty.message')}</p>
          </div>
        )}
      </div>
    </div>
  );
};

export default ChatList;


