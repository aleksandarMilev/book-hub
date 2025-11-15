import './Hero.css';

import { MDBBtn } from 'mdb-react-ui-kit';
import { type FC } from 'react';
import { Link } from 'react-router-dom';

import { routes } from '@/shared/lib/constants/api.js';

const Hero: FC = () => {
  return (
    <div className="hero-container">
      <div className="hero-mask">
        <div className="hero-content">
          <div className="hero-text">
            <h2 className="hero-subtitle">Find Your Next Great Read Today</h2>
            <MDBBtn tag={Link} to={routes.book} outline size="lg" className="hero-button">
              View Books
            </MDBBtn>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Hero;
