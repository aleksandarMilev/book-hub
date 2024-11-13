import { useContext, useState } from 'react'
import { Link } from 'react-router-dom'
import { Navbar, Nav, Container } from 'react-bootstrap'

import { routes } from '../../../common/constants/api'
import { UserContext } from '../../../contexts/userContext'

import './Header.css'  

export default function Header() {
    const [expanded, setExpanded] = useState(false)
    const { isAuthenticated, username } = useContext(UserContext)

    const handleToggle = () => {
        setExpanded(prev => !prev)
    }

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
                            <Nav.Link as={Link} to={routes.books}>
                                Books
                            </Nav.Link>
                            <Nav.Link as={Link} to={routes.createBook}>
                                Create Book
                            </Nav.Link>
                            <Nav.Link as={Link} to={routes.createAuthor}>
                                Create Author
                            </Nav.Link>
                        </Nav>
                        <Nav className="ms-auto">
                            {isAuthenticated ? (
                                <>
                                    <Nav.Item>
                                        <span className="nav-link fw-bold navbar-user">
                                            Hello, {username}!
                                        </span>
                                    </Nav.Item>
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
    )
}
