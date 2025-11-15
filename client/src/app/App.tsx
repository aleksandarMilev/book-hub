import { Outlet } from 'react-router-dom';

import Footer from '@/app/layout/footer/Footer.js';
import Header from '@/app/layout/header/Header.js';

export default function App() {
  return (
    <>
      <Header />
      <Outlet />
      <Footer />
    </>
  );
}
