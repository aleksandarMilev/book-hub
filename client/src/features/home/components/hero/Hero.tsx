import './Hero.css';

import { FC } from 'react';
import { Link } from 'react-router-dom';

import { routes } from '@/shared/lib/constants/api.js';

const Hero: FC = () => {
  return (
    <div className="hero-container">
      <div className="hero-overlay"></div>
      <div className="hero-glow one"></div>
      <div className="hero-glow two"></div>
      <div className="hero-content">
        <div className="hero-box">
          <h1 className="hero-title">Discover Amazing Stories</h1>
          <p className="hero-subtitle">
            Explore books, connect with authors, read articles, and join a community of passionate
            readers.
          </p>
          <div className="hero-buttons">
            <Link to={routes.book} className="hero-button hero-button-main">
              Browse Books
            </Link>
            <Link to={routes.profiles} className="hero-button hero-button-secondary">
              Join Our Community
            </Link>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Hero;
