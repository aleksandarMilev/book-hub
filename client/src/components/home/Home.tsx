import { type FC } from 'react';

import Hero from './hero/Hero';
import Statistics from './statistics/Statistics';
import TopAuthors from './top-authors/TopAuthors';
import TopBooks from './top-books/TopBooks';
import TopUsers from './top-users/TopUsers';

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
