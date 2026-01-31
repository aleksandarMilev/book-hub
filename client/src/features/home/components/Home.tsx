import { type FC } from 'react';

import Hero from '@/features/home/components/hero/Hero';
import Statistics from '@/features/home/components/statistics/Statistics';
import TopAuthors from '@/features/home/components/top-authors/TopAuthors';
import TopBooks from '@/features/home/components/top-books/TopBooks';
import TopUsers from '@/features/home/components/top-users/TopUsers';

const Home: FC = () => {
  return (
    <>
      <Statistics />
      <Hero />
      <TopAuthors />
      <TopBooks />
      <TopUsers />
    </>
  );
};

export default Home;


