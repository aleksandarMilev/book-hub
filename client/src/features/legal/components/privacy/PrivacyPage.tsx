import './PrivacyPage.css';

import { MDBCard, MDBCardBody, MDBCol, MDBContainer, MDBRow } from 'mdb-react-ui-kit';
import type { FC } from 'react';
import { useTranslation } from 'react-i18next';

import { CONTACT_EMAIL, LAST_UPDATED, MIN_AGE } from '@/features/legal/utils/constants';

const PrivacyPage: FC = () => {
  const { t } = useTranslation('legal');

  const renderList = (key: string) =>
    (t(key, { returnObjects: true }) as string[]).map((item) => (
      <li key={item} className="legal-privacy-li">
        {item}
      </li>
    ));

  return (
    <MDBContainer className="legal-privacy-page my-5">
      <MDBRow className="justify-content-center">
        <MDBCol lg="9" className="legal-privacy-col">
          <MDBCard className="legal-privacy-card">
            <MDBCardBody className="legal-privacy-body">
              <div className="legal-privacy-header">
                <h1 className="legal-privacy-title">{t('privacy.title')}</h1>
                <div className="legal-privacy-updated">
                  {t('common.lastUpdated', { date: LAST_UPDATED })}
                </div>
              </div>

              <p className="legal-privacy-intro">{t('privacy.intro')}</p>

              <section className="legal-privacy-section">
                <h2 className="legal-privacy-h2">{t('privacy.sections.controller.title')}</h2>
                <p className="legal-privacy-text">
                  {t('privacy.sections.controller.text', { email: CONTACT_EMAIL })}
                </p>
              </section>

              <section className="legal-privacy-section">
                <h2 className="legal-privacy-h2">{t('privacy.sections.data.title')}</h2>
                <ul className="legal-privacy-list">{renderList('privacy.sections.data.items')}</ul>
              </section>

              <section className="legal-privacy-section">
                <h2 className="legal-privacy-h2">{t('privacy.sections.use.title')}</h2>
                <ul className="legal-privacy-list">{renderList('privacy.sections.use.items')}</ul>
              </section>

              <section className="legal-privacy-section">
                <h2 className="legal-privacy-h2">{t('privacy.sections.basis.title')}</h2>
                <ul className="legal-privacy-list">{renderList('privacy.sections.basis.items')}</ul>
              </section>

              <section className="legal-privacy-section">
                <h2 className="legal-privacy-h2">{t('privacy.sections.sharing.title')}</h2>
                <p className="legal-privacy-text">{t('privacy.sections.sharing.lead')}</p>
                <ul className="legal-privacy-list">
                  {renderList('privacy.sections.sharing.items')}
                </ul>
                <div className="legal-privacy-note">{t('privacy.sections.sharing.note')}</div>
              </section>

              <section className="legal-privacy-section">
                <h2 className="legal-privacy-h2">{t('privacy.sections.retention.title')}</h2>
                <ul className="legal-privacy-list">
                  {renderList('privacy.sections.retention.items')}
                </ul>
              </section>

              <section className="legal-privacy-section">
                <h2 className="legal-privacy-h2">{t('privacy.sections.rights.title')}</h2>
                <p className="legal-privacy-text">
                  {t('privacy.sections.rights.text', { email: CONTACT_EMAIL })}
                </p>
              </section>

              <section className="legal-privacy-section">
                <h2 className="legal-privacy-h2">{t('privacy.sections.profiles.title')}</h2>
                <p className="legal-privacy-text">{t('privacy.sections.profiles.text')}</p>
              </section>

              <section className="legal-privacy-section legal-privacy-section-last">
                <h2 className="legal-privacy-h2">{t('privacy.sections.children.title')}</h2>
                <p className="legal-privacy-text">
                  {t('privacy.sections.children.text', { age: MIN_AGE })}
                </p>
              </section>

              <section className="legal-privacy-section legal-privacy-section-last">
                <h2 className="legal-privacy-h2">{t('privacy.sections.changes.title')}</h2>
                <p className="legal-privacy-text">{t('privacy.sections.changes.text')}</p>
              </section>
            </MDBCardBody>
          </MDBCard>
        </MDBCol>
      </MDBRow>
    </MDBContainer>
  );
};

export default PrivacyPage;
