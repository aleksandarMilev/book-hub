import { Link } from 'react-router-dom'
import { MDBBtn } from 'mdb-react-ui-kit'

import heroImage from '../../assets/images/hero.jpg'
import { routes } from '../../common/constants/api'

export default function Hero(){
    return(
        <div
            className='p-5 text-center bg-image'
            style={{ backgroundImage: `url(${heroImage})`, height: '400px' }}
        >
            <div className='mask' style={{ backgroundColor: 'rgba(0, 0, 0, 0.6)' }}>
                <div className='d-flex justify-content-center align-items-center h-100'>
                    <div className='text-white'>
                        <h1 className='mb-3'>BookHub</h1>
                        <h4 className='mb-3'>Find Your Next Great Read Today</h4>
                        <MDBBtn tag={Link} to={routes.books} outline size="lg">
                            View Books
                        </MDBBtn>
                    </div>
                </div>
            </div>
        </div>
    )
}