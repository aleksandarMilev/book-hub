import { Link } from 'react-router-dom'

import {
    MDBFooter, 
    MDBContainer, 
    MDBRow, 
    MDBCol, 
    MDBIcon 
} from 'mdb-react-ui-kit'

import './Footer.css'

export default function Footer() {
    const year = new Date().getFullYear()

    return (
        <MDBFooter bgColor="light" className="text-center text-lg-start text-muted">
            <section className="d-flex justify-content-center justify-content-lg-between p-4 border-bottom">
                <div className="me-5 d-none d-lg-block">
                    <span>Get connected with us on social networks:</span>
                </div>
                <div className="social-links">
                    <a href="" className="me-4 text-reset">
                        <MDBIcon fab icon="facebook-f" />
                    </a>
                    <a href="" className="me-4 text-reset">
                        <MDBIcon fab icon="twitter" />
                    </a>
                    <a href="" className="me-4 text-reset">
                        <MDBIcon fab icon="google" />
                    </a>
                    <a href="" className="me-4 text-reset">
                        <MDBIcon fab icon="instagram" />
                    </a>
                    <a href="" className="me-4 text-reset">
                        <MDBIcon fab icon="linkedin" />
                    </a>
                    <a href="" className="me-4 text-reset">
                        <MDBIcon fab icon="github" />
                    </a>
                </div>
            </section>
            <section>
                <MDBContainer className="text-center text-md-start mt-5">
                    <MDBRow className="mt-3">
                        <MDBCol md="3" lg="4" xl="3" className="mx-auto mb-4">
                            <h6 className="text-uppercase fw-bold mb-4">
                                <MDBIcon icon="book" className="me-3" />
                                BookHub
                            </h6>
                            <p>
                                Your one-stop destination for the best books, reviews, and recommendations.
                                Join the BookHub community to discover new reads and share your favorites!
                            </p>
                        </MDBCol>
                        {/* <MDBCol md="2" lg="2" xl="2" className="mx-auto mb-4">
                            <h6 className="text-uppercase fw-bold mb-4">Explore</h6>
                            <p>
                                <a href="#!" className="text-reset">Books</a>
                            </p>
                            <p>
                                <a href="#!" className="text-reset">Articles</a>
                            </p>
                            <p>
                                <a href="#!" className="text-reset">Author Interviews</a>
                            </p>
                            <p>
                                <a href="#!" className="text-reset">Reading Challenges</a>
                            </p>
                        </MDBCol>
                        <MDBCol md="3" lg="2" xl="2" className="mx-auto mb-4">
                            <h6 className="text-uppercase fw-bold mb-4">Resources</h6>
                            <p>
                                <a href="#!" className="text-reset">Book Reviews</a>
                            </p>
                            <p>
                                <a href="#!" className="text-reset">Book Recommendations</a>
                            </p>
                            <p>
                                <a href="#!" className="text-reset">Author Features</a>
                            </p>
                            <p>
                                <a href="#!" className="text-reset">Literary Events</a>
                            </p>
                        </MDBCol> */}
                        <MDBCol md="4" lg="3" xl="3" className="mx-auto mb-md-0 mb-4">
                            <h6 className="text-uppercase fw-bold mb-4">Contact</h6>
                            <p>
                                <MDBIcon icon="home" className="me-2" />
                                123 Book Street, NY 10001, US
                            </p>
                            <p>
                                <MDBIcon icon="envelope" className="me-3" />
                                info@bookhub.com
                            </p>
                            <p>
                                <MDBIcon icon="phone" className="me-3" /> + 800KHUB
                            </p>
                            <p>
                                <MDBIcon icon="print" className="me-3" /> + B00KHU8
                            </p>
                        </MDBCol>
                    </MDBRow>
                </MDBContainer>
            </section>
            <div className="text-center p-4 footer-section">
                © {year} BookHub, <a target="_blank" rel="noopener noreferrer" href="https://github.com/aleksandarMilev/BookHub">an open source project</a>
            </div>
        </MDBFooter>
    )
}
