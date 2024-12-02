import { Link } from 'react-router-dom'
import { MDBBtn } from 'mdb-react-ui-kit'

import { routes } from '../../../common/constants/api'

import './Hero.css'

export default function Hero() {
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
    )
}
