import './Header.css';

import { type FC, useState } from 'react';
import { Container, Nav, Navbar } from 'react-bootstrap';
import { useTranslation } from 'react-i18next';
import { Link } from 'react-router-dom';

import LastNotifications from '@/features/notification/components/last-list/LastNotifications';
import { routes } from '@/shared/lib/constants/api';
import { useAuth } from '@/shared/stores/auth/auth';

const Header: FC = () => {
  const [expanded, setExpanded] = useState<boolean>(false);
  const { isAuthenticated, isAdmin, username } = useAuth();
  const { t, i18n } = useTranslation('layout');

  const handleToggle = () => setExpanded((prev) => !prev);
  const closeMenu = () => setExpanded(false);

  const currentLanguage = i18n.language;
  const isBg = currentLanguage.startsWith('bg');
  const isEn = currentLanguage.startsWith('en');

  const changeLanguage = (lang: 'bg-BG' | 'en-US') => {
    i18n.changeLanguage(lang);
    localStorage.setItem('language', lang);
  };

  return (
    <header className="navbar-custom">
      <Navbar
        expand="lg"
        bg="light"
        variant="light"
        expanded={expanded}
        className="shadow-sm fancy-navbar"
      >
        <Container fluid>
          <Navbar.Brand as={Link} to={routes.home} className="brand-title">
            📚 {t('brand')}
          </Navbar.Brand>
          <Navbar.Toggle
            aria-controls="responsive-navbar-nav"
            onClick={handleToggle}
            className="custom-toggler"
          />
          <Navbar.Collapse id="responsive-navbar-nav">
            <Nav className="me-auto main-links">
              <Nav.Link as={Link} to={routes.home} onClick={closeMenu}>
                {t('header.nav.home')}
              </Nav.Link>
              <Nav.Link as={Link} to={routes.book} onClick={closeMenu}>
                {t('header.nav.books')}
              </Nav.Link>
              <Nav.Link as={Link} to={routes.genres} onClick={closeMenu}>
                {t('header.nav.genres')}
              </Nav.Link>
              <Nav.Link as={Link} to={routes.author} onClick={closeMenu}>
                {t('header.nav.authors')}
              </Nav.Link>
              <Nav.Link as={Link} to={routes.articles} onClick={closeMenu}>
                {t('header.nav.articles')}
              </Nav.Link>
              <Nav.Link as={Link} to={routes.chat} onClick={closeMenu}>
                {t('header.nav.chats')}
              </Nav.Link>
              <Nav.Link as={Link} to={routes.profiles} onClick={closeMenu}>
                {t('header.nav.users')}
              </Nav.Link>
              <Nav.Link as={Link} to={routes.createBook} onClick={closeMenu}>
                {t('header.nav.createBook')}
              </Nav.Link>
              <Nav.Link as={Link} to={routes.createAuthor} onClick={closeMenu}>
                {t('header.nav.createAuthor')}
              </Nav.Link>
              {!isAdmin && (
                <Nav.Link as={Link} to={routes.createChat} onClick={closeMenu}>
                  {t('header.nav.createChat')}
                </Nav.Link>
              )}
              {isAdmin && (
                <Nav.Link as={Link} to={routes.admin.createArticle} onClick={closeMenu}>
                  {t('header.nav.createArticle')}
                </Nav.Link>
              )}
            </Nav>
            <Nav className="ms-auto auth-section align-items-center">
              <div className="language-switcher me-3">
                <button
                  type="button"
                  className={`lang-btn ${isBg ? 'active' : ''}`}
                  onClick={() => changeLanguage('bg-BG')}
                >
                  BG
                </button>
                <span className="lang-separator">|</span>
                <button
                  type="button"
                  className={`lang-btn ${isEn ? 'active' : ''}`}
                  onClick={() => changeLanguage('en-US')}
                >
                  EN
                </button>
              </div>
              {isAuthenticated && <LastNotifications />}
              {isAuthenticated ? (
                <>
                  <span className="nav-link fw-bold navbar-user">
                    {t('header.auth.hello', { username })}
                  </span>
                  {!isAdmin && (
                    <Nav.Link
                      as={Link}
                      to={routes.profile}
                      onClick={closeMenu}
                      className="btn-pill"
                    >
                      {t('header.auth.myProfile')}
                    </Nav.Link>
                  )}
                  <Nav.Link
                    as={Link}
                    to={routes.logout}
                    onClick={closeMenu}
                    className="btn-pill danger"
                  >
                    {t('header.auth.logout')}
                  </Nav.Link>
                </>
              ) : (
                <>
                  <Nav.Link as={Link} to={routes.register} onClick={closeMenu} className="btn-pill">
                    {t('header.auth.register')}
                  </Nav.Link>
                  <Nav.Link
                    as={Link}
                    to={routes.login}
                    onClick={closeMenu}
                    className="btn-pill highlight"
                  >
                    {t('header.auth.login')}
                  </Nav.Link>
                </>
              )}
            </Nav>
          </Navbar.Collapse>
        </Container>
      </Navbar>
    </header>
  );
};

export default Header;
