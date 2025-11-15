import { type FC } from 'react';

import Hero from '@/features/home/components/hero/Hero.js';
import Statistics from '@/features/home/components/statistics/Statistics.js';
import TopAuthors from '@/features/home/components/top-authors/TopAuthors.js';
import TopBooks from '@/features/home/components/top-books/TopBooks.js';
import TopUsers from '@/features/home/components/top-users/TopUsers.js';

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
