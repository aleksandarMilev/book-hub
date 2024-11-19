import { Link, useLocation } from "react-router-dom"

import { routes } from '../../../common/constants/api'

import image from '../../../assets/images/bad-request.png'

export default function BadRequest() {
    const location = useLocation()
    const message = location.state?.message || "Something went wrong with your request. Please try again."

    return (
        <div className="d-flex align-items-center justify-content-center vh-100">
            <div className="text-center">
                <img 
                    src={image} 
                    alt="Bad Request" 
                    className="img-fluid mb-4" 
                    style={{ maxWidth: '300px' }} 
                />
                <p className="fs-3 text-danger mb-3">Oops!</p>
                <p className="lead">
                    {message}
                </p>
                <Link to={routes.home} className="btn btn-primary">Go Home</Link>
            </div>
        </div>
    )
}
