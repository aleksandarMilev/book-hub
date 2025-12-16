import './Footer.css';

import { MDBCol, MDBContainer, MDBFooter, MDBIcon, MDBRow } from 'mdb-react-ui-kit';
import type { FC } from 'react';
import { useTranslation } from 'react-i18next';
import { Link } from 'react-router-dom';

import { routes } from '@/shared/lib/constants/api.js';

const Footer: FC = () => {
  const { t } = useTranslation('layout');

  const socialItems = [
    { icon: 'facebook-f', label: 'Facebook' },
    { icon: 'twitter', label: 'Twitter' },
    { icon: 'google', label: 'Google' },
    { icon: 'instagram', label: 'Instagram' },
    { icon: 'linkedin', label: 'LinkedIn' },
    { icon: 'github', label: 'GitHub' },
  ] as const;

  const currentYear = new Date().getFullYear();

  return (
    <MDBFooter className="footer-wrapper text-center text-lg-start text-muted">
      <section className="footer-social border-bottom">
        <div className="social-container">
          <span className="social-text d-none d-lg-block">{t('footer.social.text')}</span>
          <div className="social-links">
            {socialItems.map(({ icon, label }) => (
              <a
                key={icon}
                href="#"
                className="social-link"
                aria-label={`${t('brand')} on ${label}`}
              >
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
                {t('brand')}
              </h6>
              <p className="footer-description">{t('footer.about.description1')}</p>
              <p className="footer-description">{t('footer.about.description2')}</p>
            </MDBCol>
            <MDBCol md="2" lg="2" className="footer-column mb-4">
              <h6 className="footer-title">{t('footer.columns.quickLinks')}</h6>
              <p>
                <Link to={routes.home} className="footer-link">
                  {t('footer.links.home')}
                </Link>
              </p>
              <p>
                <Link to={routes.book} className="footer-link">
                  {t('footer.links.books')}
                </Link>
              </p>
              <p>
                <Link to={routes.author} className="footer-link">
                  {t('footer.links.authors')}
                </Link>
              </p>
              <p>
                <Link to={routes.articles} className="footer-link">
                  {t('footer.links.articles')}
                </Link>
              </p>
              <p>
                <Link to={routes.chat} className="footer-link">
                  {t('footer.links.chats')}
                </Link>
              </p>
            </MDBCol>
            <MDBCol md="2" lg="2" className="footer-column mb-4">
              <h6 className="footer-title">{t('footer.columns.forUsers')}</h6>
              <p>
                <Link to={routes.profile} className="footer-link">
                  {t('footer.links.myProfile')}
                </Link>
              </p>
              <p>
                <Link to={routes.profiles} className="footer-link">
                  {t('footer.links.topUsers')}
                </Link>
              </p>
              <p>
                <Link to={routes.register} className="footer-link">
                  {t('footer.links.register')}
                </Link>
              </p>
              <p>
                <Link to={routes.login} className="footer-link">
                  {t('footer.links.login')}
                </Link>
              </p>
            </MDBCol>
          </MDBRow>
        </MDBContainer>
      </section>
      <div className="footer-bottom">
        © {currentYear} {t('brand')} ·{' '}
        <a
          className="footer-link"
          href="https://github.com/aleksandarMilev/book-hub"
          target="_blank"
          rel="noopener noreferrer"
        >
          {t('footer.bottom.openSource')}
        </a>
      </div>
    </MDBFooter>
  );
};

export default Footer;
