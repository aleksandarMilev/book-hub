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
                        <h1 className="hero-title">BookHub</h1>
                        <h4 className="hero-subtitle">Find Your Next Great Read Today</h4>
                        <MDBBtn tag={Link} to={routes.books} outline size="lg" className="hero-button">
                            View Books
                        </MDBBtn>
                    </div>
                </div>
            </div>
        </div>
    )
}
