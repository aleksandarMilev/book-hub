import './Hero.css';

import type { FC } from 'react';
import { useTranslation } from 'react-i18next';
import { Link } from 'react-router-dom';

import { routes } from '@/shared/lib/constants/api.js';

const Hero: FC = () => {
  const { t } = useTranslation('home');

  return (
    <div className="hero-container">
      <div className="hero-overlay"></div>
      <div className="hero-glow one"></div>
      <div className="hero-glow two"></div>
      <div className="hero-content">
        <div className="hero-box">
          <h1 className="hero-title">{t('hero.title')}</h1>
          <p className="hero-subtitle">{t('hero.subtitle')}</p>
          <div className="hero-buttons">
            <Link to={routes.book} className="hero-button hero-button-main">
              {t('hero.primaryCta')}
            </Link>
            <Link to={routes.profiles} className="hero-button hero-button-secondary">
              {t('hero.secondaryCta')}
            </Link>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Hero;
