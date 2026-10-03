import { searchBooks } from '@/features/search/api/api';
import { http } from '@/shared/api/http';
import { routes } from '@/shared/lib/constants/api';

describe('search api', () => {
  afterEach(() => {
    vi.restoreAllMocks();
  });

  // F-16: search used the global axios, bypassing the 401 interceptor on `http`.
  it('sends the request through the shared http instance', async () => {
    const page = { items: [], totalItems: 0, pageIndex: 1, pageSize: 10 };
    const get = vi.spyOn(http, 'get').mockResolvedValue({ data: page });

    const result = await searchBooks('dune', 1, 10, 'token-value');

    expect(result).toEqual(page);
    expect(get).toHaveBeenCalledWith(
      routes.searchBooks,
      expect.objectContaining({
        headers: { Authorization: 'Bearer token-value' },
        params: { searchTerm: 'dune', page: 1, pageSize: 10 },
      }),
    );
  });
});
