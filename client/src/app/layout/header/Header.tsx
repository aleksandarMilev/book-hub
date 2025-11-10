import './Header.css';

import { type FC, useState } from 'react';
import { Container, Nav, Navbar } from 'react-bootstrap';
import { Link } from 'react-router-dom';

import LastNotifications from '@/features/notifications/components/last-notifications/LastNotifications';
import { routes } from '@/shared/lib/constants/api';
import { useAuth } from '@/shared/stores/auth/auth';

const Header: FC = () => {
  const [expanded, setExpanded] = useState<boolean>(false);
  const { isAuthenticated, isAdmin, username } = useAuth();

  const handleToggle = () => setExpanded((prev) => !prev);

  return (
    <header>
      <Navbar expand="lg" bg="light" variant="light" expanded={expanded}>
        <Container fluid>
          <Navbar.Toggle aria-controls="responsive-navbar-nav" onClick={handleToggle} />
          <Navbar.Collapse id="responsive-navbar-nav">
            <Nav className="me-auto">
              <Nav.Link as={Link} to={routes.home}>
                Home
              </Nav.Link>
              <Nav.Link as={Link} to={routes.book}>
                Books
              </Nav.Link>
              <Nav.Link as={Link} to={routes.author}>
                Authors
              </Nav.Link>
              <Nav.Link as={Link} to={routes.articles}>
                Articles
              </Nav.Link>
              <Nav.Link as={Link} to={routes.chats}>
                Chats
              </Nav.Link>
              <Nav.Link as={Link} to={routes.profiles}>
                Users
              </Nav.Link>
              <Nav.Link as={Link} to={routes.createBook}>
                Create Book
              </Nav.Link>
              <Nav.Link as={Link} to={routes.createAuthor}>
                Create Author
              </Nav.Link>
              {!isAdmin && (
                <Nav.Link as={Link} to={routes.createChat}>
                  Create Chat
                </Nav.Link>
              )}
              {isAdmin && (
                <Nav.Link as={Link} to={routes.admin.createArticle}>
                  Create Article
                </Nav.Link>
              )}
            </Nav>
            <Nav className="ms-auto">
              {isAuthenticated && <LastNotifications />}
              {isAuthenticated ? (
                <>
                  <Nav.Item>
                    <span className="nav-link fw-bold navbar-user">Hello, {username}!</span>
                  </Nav.Item>
                  {!isAdmin && (
                    <Nav.Link as={Link} to={routes.profile}>
                      My Profile
                    </Nav.Link>
                  )}
                  <Nav.Link as={Link} to={routes.logout}>
                    Logout
                  </Nav.Link>
                </>
              ) : (
                <>
                  <Nav.Link as={Link} to={routes.register}>
                    Register
                  </Nav.Link>
                  <Nav.Link as={Link} to={routes.login}>
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
