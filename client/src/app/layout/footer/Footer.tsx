import './Footer.css';

import { MDBCol, MDBContainer, MDBFooter, MDBIcon, MDBRow } from 'mdb-react-ui-kit';
import type { FC } from 'react';

const Footer: FC = () => {
  return (
    <MDBFooter className="footer-wrapper text-center text-lg-start text-muted">
      {/* Social Section */}
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
            <MDBCol md="4" lg="4" className="footer-column mb-4">
              <h6 className="footer-title">
                <MDBIcon icon="book" className="me-2" />
                BookHub
              </h6>
              <p className="footer-description">
                Discover books, authors, articles, and join a vibrant community of passionate
                readers. Your next great story awaits.
              </p>
            </MDBCol>
            <MDBCol md="4" lg="3" className="footer-column mb-md-0 mb-4">
              <h6 className="footer-title">Contact</h6>
              <p>
                <MDBIcon icon="home" className="me-2" /> 123 Book Street, NY 10001, US
              </p>
              <p>
                <MDBIcon icon="envelope" className="me-2" /> info@bookhub.com
              </p>
              <p>
                <MDBIcon icon="phone" className="me-2" /> +800-KHUB
              </p>
              <p>
                <MDBIcon icon="print" className="me-2" /> +B00K-HU8
              </p>
            </MDBCol>
          </MDBRow>
        </MDBContainer>
      </section>
      <div className="footer-bottom">
        © {new Date().getFullYear()} BookHub -{' '}
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
