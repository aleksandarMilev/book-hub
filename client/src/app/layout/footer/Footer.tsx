import './Footer.css';

import { MDBCol, MDBContainer, MDBFooter, MDBIcon, MDBRow } from 'mdb-react-ui-kit';
import type { FC } from 'react';
import { Link } from 'react-router-dom';

import { routes } from '@/shared/lib/constants/api.js';

const Footer: FC = () => {
  return (
    <MDBFooter className="footer-wrapper text-center text-lg-start text-muted">
      <section className="footer-social border-bottom">
        <div className="social-container">
          <span className="social-text d-none d-lg-block">Get connected with us:</span>
          <div className="social-links">
            {[
              { icon: 'facebook-f', label: 'Facebook' },
              { icon: 'twitter', label: 'Twitter' },
              { icon: 'google', label: 'Google' },
              { icon: 'instagram', label: 'Instagram' },
              { icon: 'linkedin', label: 'LinkedIn' },
              { icon: 'github', label: 'GitHub' },
            ].map(({ icon, label }) => (
              <a key={icon} href="#" className="social-link" aria-label={`BookHub on ${label}`}>
                <MDBIcon fab icon={icon} />
              </a>
            ))}
          </div>
        </div>
      </section>
      <section className="footer-content">
        <MDBContainer className="mt-5">
          <MDBRow className="mt-3 justify-content-center">
            <MDBCol md="3" lg="3" className="footer-column mb-4">
              <h6 className="footer-title">
                <MDBIcon icon="book" className="me-2" />
                BookHub
              </h6>
              <p className="footer-description">
                Discover books, authors, articles, and join a vibrant community of passionate
                readers.
              </p>
              <p className="footer-description">Your next great story awaits.</p>
            </MDBCol>
            <MDBCol md="2" lg="2" className="footer-column mb-4">
              <h6 className="footer-title">Quick Links</h6>
              <p>
                <Link to={routes.home} className="footer-link">
                  Home
                </Link>
              </p>
              <p>
                <Link to={routes.book} className="footer-link">
                  Books
                </Link>
              </p>
              <p>
                <Link to={routes.author} className="footer-link">
                  Authors
                </Link>
              </p>
              <p>
                <Link to={routes.articles} className="footer-link">
                  Articles
                </Link>
              </p>
              <p>
                <Link to={routes.chats} className="footer-link">
                  Chats
                </Link>
              </p>
            </MDBCol>
            <MDBCol md="2" lg="2" className="footer-column mb-4">
              <h6 className="footer-title">For Users</h6>
              <p>
                <Link to={routes.profile} className="footer-link">
                  My Profile
                </Link>
              </p>
              <p>
                <Link to={routes.profiles} className="footer-link">
                  Top Users
                </Link>
              </p>
              <p>
                <Link to={routes.register} className="footer-link">
                  Register
                </Link>
              </p>
              <p>
                <Link to={routes.login} className="footer-link">
                  Login
                </Link>
              </p>
            </MDBCol>
            <MDBCol md="4" lg="4" className="footer-column mb-md-0 mb-4">
              <h6 className="footer-title">Newsletter</h6>
              <p className="footer-description">Stay updated with new books and articles.</p>
              <form className="newsletter-form">
                <input type="email" placeholder="Your email" className="newsletter-input" />
                <button type="submit" className="newsletter-button">
                  Subscribe
                </button>
              </form>
            </MDBCol>
          </MDBRow>
        </MDBContainer>
      </section>
      <div className="footer-bottom">
        © {new Date().getFullYear()} BookHub ·{' '}
        <a
          className="footer-link"
          href="https://github.com/aleksandarMilev/book-hub"
          target="_blank"
          rel="noopener noreferrer"
        >
          Open Source Project
        </a>
      </div>
    </MDBFooter>
  );
};

export default Footer;
