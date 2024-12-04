import { useContext } from 'react'
import { Link } from 'react-router-dom'
import CountUp from 'react-countup'  
import { 
    FaUsers, 
    FaBook, 
    FaUserTie, 
    FaCommentDots, 
    FaTags, 
    FaNewspaper,
    FaBookReader
} from 'react-icons/fa'

import { routes } from '../../../common/constants/api'
import { UserContext } from '../../../contexts/userContext'
import * as useStatistics from '../../../hooks/useStatistics'

import DefaultSpinner from '../../common/default-spinner/DefaultSpinner'

import './Statistics.css'

export default function Statistics() {
    const { isAuthenticated } = useContext(UserContext)
    const { statistics, isFetching, error } = useStatistics.useGet()

    if (isFetching || !statistics) {
        return <DefaultSpinner />
    }

    if (error) {
        return (
            <div className="d-flex flex-column align-items-center justify-content-center vh-50">
                <div className="text-center">
                    <FaBookReader size={100} color="red" className="mb-3" />
                    <p className="lead">{error}</p> 
                </div>
            </div>
        )
    }

    return (
        <div className="statistics-container">
            <h2 className="statistics-title">Welocome to BookHub!</h2>
            <h4>Dive Into a Universe of Books and Beyond</h4>
            <div className="row">
                <div className="col-md-4 mb-4">
                    <div className="stat-card">
                        <FaUsers className="stat-icon" />
                        <p className="stat-label">Over</p>
                        <h4 className="stat-number">
                            <CountUp start={0} end={statistics.users} duration={5} />
                        </h4>
                        <p className="stat-label">users have joined our community</p>
                    </div>
                </div>
                <div className="col-md-4 mb-4">
                    <div className="stat-card">
                        <FaBook className="stat-icon" />
                        <p className="stat-label">More than</p>
                        <h4 className="stat-number">
                            <CountUp start={0} end={statistics.books} duration={5} />
                        </h4>
                        <p className="stat-label">books have been created</p>
                    </div>
                </div>
                <div className="col-md-4 mb-4">
                    <div className="stat-card">
                        <FaUserTie className="stat-icon" />
                        <p className="stat-label">Over</p>
                        <h4 className="stat-number">
                            <CountUp start={0} end={statistics.authors} duration={5} />
                        </h4>
                        <p className="stat-label">authors have been added</p>
                    </div>
                </div>
            </div>
            <div className="row">
                <div className="col-md-4 mb-4">
                    <div className="stat-card">
                        <FaCommentDots className="stat-icon" />
                        <p className="stat-label">A total of</p>
                        <h4 className="stat-number">
                            <CountUp start={0} end={statistics.reviews} duration={5} />
                        </h4>
                        <p className="stat-label">reviews have been written</p>
                    </div>
                </div>
                <div className="col-md-4 mb-4">
                    <div className="stat-card">
                        <FaTags className="stat-icon" />
                        <p className="stat-label">Choose from over</p>
                        <h4 className="stat-number">
                            <CountUp start={0} end={statistics.genres} duration={5} />
                        </h4>
                        <p className="stat-label">different genres</p>
                    </div>
                </div>
                <div className="col-md-4 mb-4">
                    <div className="stat-card">
                        <FaNewspaper className="stat-icon" />
                        <p className="stat-label">Read over</p>
                        <h4 className="stat-number">
                            <CountUp start={0} end={statistics.articles} duration={5} />
                        </h4>
                        <p className="stat-label">articles on various topics</p>
                    </div>
                </div>
                {isAuthenticated || 
                    <Link to={routes.login} className="link-button">
                        Join Now!
                    </Link>}
            </div>
        </div>
    )
}
