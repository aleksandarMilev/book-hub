import './Header.css';

import { type FC, useState } from 'react';
import { Container, Nav, Navbar } from 'react-bootstrap';
import { Link } from 'react-router-dom';

import LastNotifications from '@/features/notification/components/last-list/LastNotifications.js';
import { routes } from '@/shared/lib/constants/api.js';
import { useAuth } from '@/shared/stores/auth/auth.js';

const Header: FC = () => {
  const [expanded, setExpanded] = useState<boolean>(false);
  const { isAuthenticated, isAdmin, username } = useAuth();

  const handleToggle = () => setExpanded((prev) => !prev);
  const closeMenu = () => setExpanded(false);

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
            📚 BookHub
          </Navbar.Brand>
          <Navbar.Toggle
            aria-controls="responsive-navbar-nav"
            onClick={handleToggle}
            className="custom-toggler"
          />
          <Navbar.Collapse id="responsive-navbar-nav">
            <Nav className="me-auto main-links">
              <Nav.Link as={Link} to={routes.home} onClick={closeMenu}>
                Home
              </Nav.Link>
              <Nav.Link as={Link} to={routes.book} onClick={closeMenu}>
                Books
              </Nav.Link>
              <Nav.Link as={Link} to={routes.author} onClick={closeMenu}>
                Authors
              </Nav.Link>
              <Nav.Link as={Link} to={routes.articles} onClick={closeMenu}>
                Articles
              </Nav.Link>
              <Nav.Link as={Link} to={routes.chats} onClick={closeMenu}>
                Chats
              </Nav.Link>
              <Nav.Link as={Link} to={routes.profiles} onClick={closeMenu}>
                Users
              </Nav.Link>
              <Nav.Link as={Link} to={routes.createBook} onClick={closeMenu}>
                Create Book
              </Nav.Link>
              <Nav.Link as={Link} to={routes.createAuthor} onClick={closeMenu}>
                Create Author
              </Nav.Link>
              {!isAdmin && (
                <Nav.Link as={Link} to={routes.createChat} onClick={closeMenu}>
                  Create Chat
                </Nav.Link>
              )}
              {isAdmin && (
                <Nav.Link as={Link} to={routes.admin.createArticle} onClick={closeMenu}>
                  Create Article
                </Nav.Link>
              )}
            </Nav>
            <Nav className="ms-auto auth-section">
              {isAuthenticated && <LastNotifications />}
              {isAuthenticated ? (
                <>
                  <span className="nav-link fw-bold navbar-user">Hello, {username}!</span>
                  {!isAdmin && (
                    <Nav.Link
                      as={Link}
                      to={routes.profile}
                      onClick={closeMenu}
                      className="btn-pill"
                    >
                      My Profile
                    </Nav.Link>
                  )}
                  <Nav.Link
                    as={Link}
                    to={routes.logout}
                    onClick={closeMenu}
                    className="btn-pill danger"
                  >
                    Logout
                  </Nav.Link>
                </>
              ) : (
                <>
                  <Nav.Link as={Link} to={routes.register} onClick={closeMenu} className="btn-pill">
                    Register
                  </Nav.Link>
                  <Nav.Link
                    as={Link}
                    to={routes.login}
                    onClick={closeMenu}
                    className="btn-pill highlight"
                  >
                    Login
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
