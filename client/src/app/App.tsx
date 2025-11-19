import { Outlet, ScrollRestoration } from 'react-router-dom';

import Footer from '@/app/layout/footer/Footer.js';
import Header from '@/app/layout/header/Header.js';
import MessageDisplay from '@/shared/components/message/Message.js';
import { useMessage } from '@/shared/stores/message/message.js';

export default function App() {
  const { message, isShowing, isSuccess } = useMessage();

  return (
    <>
      {isShowing && <MessageDisplay message={message!} isSuccess={isSuccess} />}
      <Header />
      <ScrollRestoration />
      <Outlet />
      <Footer />
    </>
  );
}
