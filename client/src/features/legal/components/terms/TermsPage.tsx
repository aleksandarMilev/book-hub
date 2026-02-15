import './TermsPage.css';

import { MDBCard, MDBCardBody, MDBCol, MDBContainer, MDBRow } from 'mdb-react-ui-kit';
import type { FC } from 'react';
import { useTranslation } from 'react-i18next';

import { CONTACT_EMAIL, LAST_UPDATED, MIN_AGE } from '@/features/legal/utils/constants';

const TermsPage: FC = () => {
  const { t } = useTranslation('legal');

  return (
    <MDBContainer className="legal-terms-page my-5">
      <MDBRow className="justify-content-center">
        <MDBCol lg="9" className="legal-terms-col">
          <MDBCard className="legal-terms-card">
            <MDBCardBody className="legal-terms-body">
              <div className="legal-terms-header">
                <h1 className="legal-terms-title">{t('terms.title')}</h1>
                <div className="legal-terms-updated">
                  {t('common.lastUpdated', { date: LAST_UPDATED })}
                </div>
              </div>

              <p className="legal-terms-intro">{t('terms.intro')}</p>

              <section className="legal-terms-section">
                <h2 className="legal-terms-h2">{t('terms.sections.who.title')}</h2>
                <p className="legal-terms-text">
                  {t('terms.sections.who.text', { email: CONTACT_EMAIL })}
                </p>
              </section>

              <section className="legal-terms-section">
                <h2 className="legal-terms-h2">{t('terms.sections.age.title')}</h2>
                <p className="legal-terms-text">{t('terms.sections.age.text', { age: MIN_AGE })}</p>
              </section>

              <section className="legal-terms-section">
                <h2 className="legal-terms-h2">{t('terms.sections.accounts.title')}</h2>
                <ul className="legal-terms-list">
                  {(t('terms.sections.accounts.items', { returnObjects: true }) as string[]).map(
                    (item) => (
                      <li key={item} className="legal-terms-li">
                        {item}
                      </li>
                    ),
                  )}
                </ul>
              </section>

              <section className="legal-terms-section">
                <h2 className="legal-terms-h2">{t('terms.sections.content.title')}</h2>
                <p className="legal-terms-text">{t('terms.sections.content.text')}</p>
              </section>

              <section className="legal-terms-section">
                <h2 className="legal-terms-h2">{t('terms.sections.acceptable.title')}</h2>
                <p className="legal-terms-text">{t('terms.sections.acceptable.lead')}</p>
                <ul className="legal-terms-list">
                  {(t('terms.sections.acceptable.items', { returnObjects: true }) as string[]).map(
                    (item) => (
                      <li key={item} className="legal-terms-li">
                        {item}
                      </li>
                    ),
                  )}
                </ul>
              </section>

              <section className="legal-terms-section">
                <h2 className="legal-terms-h2">{t('terms.sections.moderation.title')}</h2>
                <p className="legal-terms-text">{t('terms.sections.moderation.text')}</p>
              </section>

              <section className="legal-terms-section">
                <h2 className="legal-terms-h2">{t('terms.sections.deletion.title')}</h2>
                <p className="legal-terms-text">{t('terms.sections.deletion.text')}</p>
              </section>

              <section className="legal-terms-section">
                <h2 className="legal-terms-h2">{t('terms.sections.disclaimers.title')}</h2>
                <p className="legal-terms-text">{t('terms.sections.disclaimers.text')}</p>
              </section>

              <section className="legal-terms-section">
                <h2 className="legal-terms-h2">{t('terms.sections.liability.title')}</h2>
                <p className="legal-terms-text">{t('terms.sections.liability.text')}</p>
              </section>

              <section className="legal-terms-section legal-terms-section-last">
                <h2 className="legal-terms-h2">{t('terms.sections.changes.title')}</h2>
                <p className="legal-terms-text">{t('terms.sections.changes.text')}</p>
              </section>
            </MDBCardBody>
          </MDBCard>
        </MDBCol>
      </MDBRow>
    </MDBContainer>
  );
};

export default TermsPage;
