import { useStatistics } from '@/features/statistics/hooks/useCrud.js';
import { useCountUp } from '@/shared/hooks/countup/useCountup.js';
import { useAuth } from '@/shared/stores/auth/auth.js';
import { useTranslation } from 'react-i18next';

export const useStatisticsPage = () => {
  const { t } = useTranslation('home');
  const { isAuthenticated } = useAuth();
  const { statistics, isFetching, error } = useStatistics();

  const users = useCountUp(statistics?.users ?? 0, 2_000);
  const books = useCountUp(statistics?.books ?? 0, 2_000);
  const authors = useCountUp(statistics?.authors ?? 0, 2_000);
  const reviews = useCountUp(statistics?.reviews ?? 0, 2_000);
  const genres = useCountUp(statistics?.genres ?? 0, 2_000);
  const articles = useCountUp(statistics?.articles ?? 0, 2_000);

  return {
    t,
    isAuthenticated,
    isFetching,
    error,
    counts: {
      users,
      books,
      authors,
      reviews,
      genres,
      articles,
    },
  };
};
