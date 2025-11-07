import { Outlet } from 'react-router-dom';

import Footer from '@/app/layout/footer/Footer';
import Header from '@/app/layout/header/Header';

export default function App() {
  return (
    <>
      <Header />
      <Outlet />
      <Footer />
    </>
  );
}
