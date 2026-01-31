import './App.css';

import { Outlet, ScrollRestoration } from 'react-router-dom';

import Footer from '@/app/layout/footer/Footer';
import Header from '@/app/layout/header/Header';
import ErrorBoundary from '@/shared/components/errors/error-boundary/ErrorBoundary';
import MessageDisplay from '@/shared/components/message/Message';
import { useMessage } from '@/shared/stores/message/message';

export default function App() {
  const { message, isShowing, isSuccess } = useMessage();

  return (
    <>
      {isShowing && <MessageDisplay message={message!} isSuccess={isSuccess} />}
      <Header />
      <ErrorBoundary onReset={() => window.location.reload()}>
        <ScrollRestoration />
        <main className="app-shell">
          <Outlet />
        </main>
      </ErrorBoundary>
      <Footer />
    </>
  );
}


