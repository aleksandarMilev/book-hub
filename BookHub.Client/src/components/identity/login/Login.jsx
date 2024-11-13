import { useNavigate } from 'react-router-dom'
import { Link } from 'react-router-dom'
import * as Yup from 'yup'
import { useFormik } from 'formik'
import { MDBBtn, MDBContainer, MDBRow, MDBCol, MDBInput, MDBIcon, MDBCheckbox } from 'mdb-react-ui-kit'

import { routes } from '../../../common/constants/api'
import * as useIdentity from '../../../hooks/useIdentity'

export default function Login() {
    const navigate = useNavigate()
    const loginHandler = useIdentity.useLogin()

    const validationSchema = Yup.object({
        username: Yup.string().required('Username is required'),
        password: Yup.string().required('Password is required')
    })

    const formik = useFormik({
        initialValues: {
            username: '',
            password: ''
        },
        validationSchema,
        onSubmit: async (values, { setErrors }) => {
            try {
                await loginHandler(values.username, values.password)
                navigate(routes.home);
            } catch (error) {
                setErrors({ username: error.message || 'An error occurred' })
            }
        }
    })

    return (
        <MDBContainer fluid className="p-3 my-5 h-custom">
            <MDBRow>
                <MDBCol col='10' md='6'>
                    <img 
                        src="https://mdbcdn.b-cdn.net/img/Photos/new-templates/bootstrap-login-form/draw2.webp" 
                        className="img-fluid" 
                        alt="Sample" 
                    />
                </MDBCol>
                <MDBCol col='4' md='6'>
                    <div className="d-flex flex-row align-items-center justify-content-center">
                        <p className="lead fw-normal mb-0 me-3">Sign in with</p>
                        <MDBBtn floating size='md' tag='a' className='me-2'>
                            <MDBIcon fab icon='facebook-f' />
                        </MDBBtn>
                        <MDBBtn floating size='md' tag='a' className='me-2'>
                            <MDBIcon fab icon='twitter' />
                        </MDBBtn>
                        <MDBBtn floating size='md' tag='a' className='me-2'>
                            <MDBIcon fab icon='linkedin-in' />
                        </MDBBtn>
                    </div>
                    <div className="divider d-flex align-items-center my-4">
                        <p className="text-center fw-bold mx-3 mb-0">Or</p>
                    </div>
                    <form onSubmit={formik.handleSubmit}>
                        <MDBInput 
                            wrapperClass='mb-4' 
                            label='Username' 
                            id='username' 
                            type='text' 
                            size="lg"
                            name='username'
                            value={formik.values.username}
                            onChange={formik.handleChange}
                            onBlur={formik.handleBlur}
                        />
                        {formik.touched.username && formik.errors.username && (
                            <div className="text-danger mb-3">{formik.errors.username}</div>
                        )}
                        <MDBInput 
                            wrapperClass='mb-4'
                            label='Password' 
                            id='password' 
                            type='password' 
                            size="lg"
                            name='password'
                            value={formik.values.password}
                            onChange={formik.handleChange}
                            onBlur={formik.handleBlur}
                        />
                        {formik.touched.password && formik.errors.password && (
                            <div className="text-danger mb-3">{formik.errors.password}</div>
                        )}
                        <div className="d-flex justify-content-between mb-4">
                            <MDBCheckbox name='flexCheck' value='' id='flexCheckDefault' label='Remember me' />
                            <a href="!#">Forgot password?</a>
                        </div>
                        <div className='text-center text-md-start mt-4 pt-2'>
                            <MDBBtn 
                                className="mb-0 px-5" 
                                size='lg' 
                                type="submit"
                            >
                                Login
                            </MDBBtn>
                            <p className="small fw-bold mt-2 pt-1 mb-2">
                                Don't have an account?
                                <Link to={routes.register} className="link-danger"> Register</Link>
                            </p>
                        </div>
                    </form>
                </MDBCol>
            </MDBRow>
        </MDBContainer>
    )
}
