import './CookiesPage.css';

import { MDBCard, MDBCardBody, MDBCol, MDBContainer, MDBRow } from 'mdb-react-ui-kit';
import type { FC } from 'react';
import { useTranslation } from 'react-i18next';

import { CONTACT_EMAIL, LAST_UPDATED } from '@/features/legal/utils/constants';

const CookiesPage: FC = () => {
  const { t } = useTranslation('legal');

  const renderList = (key: string) =>
    (t(key, { returnObjects: true }) as string[]).map((item) => (
      <li key={item} className="legal-cookies-li">
        {item}
      </li>
    ));

  return (
    <MDBContainer className="legal-cookies-page my-5">
      <MDBRow className="justify-content-center">
        <MDBCol lg="9" className="legal-cookies-col">
          <MDBCard className="legal-cookies-card">
            <MDBCardBody className="legal-cookies-body">
              <div className="legal-cookies-header">
                <h1 className="legal-cookies-title">{t('cookies.title')}</h1>
                <div className="legal-cookies-updated">
                  {t('common.lastUpdated', { date: LAST_UPDATED })}
                </div>
              </div>

              <p className="legal-cookies-intro">{t('cookies.intro')}</p>

              <section className="legal-cookies-section">
                <h2 className="legal-cookies-h2">{t('cookies.sections.what.title')}</h2>
                <p className="legal-cookies-text">{t('cookies.sections.what.text')}</p>
              </section>

              <section className="legal-cookies-section">
                <h2 className="legal-cookies-h2">{t('cookies.sections.today.title')}</h2>
                <p className="legal-cookies-text">{t('cookies.sections.today.text')}</p>
                <ul className="legal-cookies-list">{renderList('cookies.sections.today.items')}</ul>
              </section>

              <section className="legal-cookies-section">
                <h2 className="legal-cookies-h2">{t('cookies.sections.manage.title')}</h2>
                <ul className="legal-cookies-list">
                  {renderList('cookies.sections.manage.items')}
                </ul>
              </section>

              <section className="legal-cookies-section">
                <h2 className="legal-cookies-h2">{t('cookies.sections.future.title')}</h2>
                <p className="legal-cookies-text">{t('cookies.sections.future.text')}</p>
              </section>

              <section className="legal-cookies-section legal-cookies-section-last">
                <h2 className="legal-cookies-h2">{t('cookies.sections.contact.title')}</h2>
                <p className="legal-cookies-text">
                  {t('cookies.sections.contact.text', { email: CONTACT_EMAIL })}
                </p>
              </section>
            </MDBCardBody>
          </MDBCard>
        </MDBCol>
      </MDBRow>
    </MDBContainer>
  );
};

export default CookiesPage;
