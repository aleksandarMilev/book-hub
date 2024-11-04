import React, { useState } from 'react'
import { Link } from 'react-router-dom'
import { MDBNavbar, MDBNavbarNav, MDBNavbarItem, MDBNavbarToggler, MDBContainer, MDBIcon, MDBCollapse } from 'mdb-react-ui-kit'

import { routes } from '../../common/constants/api'

export default function Header() {
    const [showBasic, setShowBasic] = useState(false)

    return (
        <header>
            <MDBNavbar expand='lg' light bgColor='white'>
                <MDBContainer fluid>
                    <MDBNavbarToggler
                        onClick={() => setShowBasic(prev => !prev)}
                        aria-controls='navbarExample01'
                        aria-expanded={showBasic}
                        aria-label='Toggle navigation'
                    >
                        <MDBIcon fas icon='bars' />
                    </MDBNavbarToggler>
                    <MDBCollapse navbar show={showBasic ? true : undefined}>
                        <MDBNavbarNav right className='mb-2 mb-lg-0'>
                            <MDBNavbarItem active>
                                <Link className="nav-link" to={routes.home}>
                                    Home
                                </Link>
                            </MDBNavbarItem>
                            <MDBNavbarItem>
                                <Link className="nav-link" to={routes.register}>
                                    Register
                                </Link>
                            </MDBNavbarItem>
                            <MDBNavbarItem>
                                <Link className="nav-link" to={routes.login}>
                                    Login
                                </Link>
                            </MDBNavbarItem>
                            <MDBNavbarItem>
                                <Link className="nav-link" to={routes.books}>
                                    Books
                                </Link>
                            </MDBNavbarItem>
                            <MDBNavbarItem>
                                <Link className="nav-link" to={routes.createBook}>
                                    Create Book
                                </Link>
                            </MDBNavbarItem>
                        </MDBNavbarNav>
                    </MDBCollapse>
                </MDBContainer>
            </MDBNavbar>
        </header>
    )
}
