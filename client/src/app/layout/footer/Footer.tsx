import './Footer.css';

import { MDBCol, MDBContainer, MDBFooter, MDBIcon, MDBRow } from 'mdb-react-ui-kit';
import type { FC } from 'react';

const Footer: FC = () => {
  return (
    <MDBFooter bgColor="light" className="text-center text-lg-start text-muted">
      <section className="d-flex justify-content-center justify-content-lg-between p-4 border-bottom">
        <div className="me-5 d-none d-lg-block">
          <span>Get connected with us on social networks:</span>
        </div>
        <div className="social-links">
          {[
            { icon: 'facebook-f', label: 'Facebook' },
            { icon: 'twitter', label: 'Twitter' },
            { icon: 'google', label: 'Google' },
            { icon: 'instagram', label: 'Instagram' },
            { icon: 'linkedin', label: 'LinkedIn' },
            { icon: 'github', label: 'GitHub' },
          ].map(({ icon, label }) => (
            <a key={icon} href="#" className="me-4 text-reset" aria-label={`BookHub on ${label}`}>
              <MDBIcon fab icon={icon} />
            </a>
          ))}
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
                Your one-stop destination for the best books, reviews, and recommendations. Join the
                BookHub community to discover new reads and share your favorites!
              </p>
            </MDBCol>
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
                <MDBIcon icon="phone" className="me-3" /> +800-KHUB
              </p>
              <p>
                <MDBIcon icon="print" className="me-3" /> +B00K-HU8
              </p>
            </MDBCol>
          </MDBRow>
        </MDBContainer>
      </section>
      <div className="text-center p-4 footer-section">
        © {new Date().getFullYear()} BookHub,{' '}
        <a
          href="https://github.com/aleksandarMilev/book-hub"
          target="_blank"
          rel="noopener noreferrer"
        >
          an open source project
        </a>
      </div>
    </MDBFooter>
  );
};

export default Footer;
