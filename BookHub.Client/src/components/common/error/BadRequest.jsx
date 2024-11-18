import { Link, useLocation } from "react-router-dom"

import { routes } from '../../../common/constants/api'

export default function BadRequest(){
    const location = useLocation()
    const message = location.state?.message || 'Something went wrong with your request. Please try again'
    
    return(
        <div className="d-flex align-items-center justify-content-center vh-100">
        <div className="text-center">
            <h1 className="display-1 fw-bold">400</h1>
            <p className="fs-3 text-danger mb-3">Oops!</p>
            <p className="lead">
                {message}
            </p>
            <Link to={routes.home} className="btn btn-primary">Go Home</Link>
        </div>
    </div>
    )
}