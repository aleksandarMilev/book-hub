import { useNavigate } from 'react-router-dom'
import { Link } from 'react-router-dom'
import * as Yup from 'yup'
import { useFormik } from 'formik'
import { 
    MDBBtn,
    MDBContainer, 
    MDBRow, 
    MDBCol, 
    MDBInput, 
    MDBIcon, 
    MDBCheckbox 
} from 'mdb-react-ui-kit'

import * as useIdentity from '../../../hooks/useIdentity'
import { routes } from '../../../common/constants/api'

export default function Login() {
    const navigate = useNavigate()
    const loginHandler = useIdentity.useLogin()

    const validationSchema = Yup.object({
        credentials: Yup
            .string()
            .required('Username or Email is required'),
        password: Yup
            .string()
            .required('Password is required'),
        rememberMe: Yup
            .boolean()
    })

    const formik = useFormik({
        initialValues: {
            credentials: '',
            password: '',
            rememberMe: false
        },
        validationSchema,
        onSubmit: async (values, { setErrors }) => {
            try {
                await loginHandler(values.credentials, values.password, values.rememberMe)
                navigate(routes.home)
            } catch (error) {
                setErrors({ credentials: error.message || 'An error occurred' })
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
                <MDBCol
                    style={{marginTop: '3.4em'}} 
                    col='4'
                    md='6'>
                    <div className="d-flex flex-row align-items-center justify-content-center">
                        <p className="lead fw-normal mb-3 me-3">Sign in</p>
                    </div>
                    <form onSubmit={formik.handleSubmit}>
                        <div className="mb-4">
                            {formik.touched.credentials && formik.errors.credentials && (
                                <div className="text-danger mb-2">{formik.errors.credentials}</div>
                            )}
                            <MDBInput 
                                label='Username or Email' 
                                id='credentials' 
                                type='text' 
                                size="lg"
                                name='credentials'
                                value={formik.values.credentials}
                                onChange={formik.handleChange}
                                onBlur={formik.handleBlur}
                            />
                        </div>
                        <div className="mb-4">
                            {formik.touched.password && formik.errors.password && (
                                <div className="text-danger mb-2">{formik.errors.password}</div>
                            )}
                            <MDBInput 
                                label='Password' 
                                id='password' 
                                type='password' 
                                size="lg"
                                name='password'
                                value={formik.values.password}
                                onChange={formik.handleChange}
                                onBlur={formik.handleBlur}
                            />
                        </div>
                        <div className="d-flex justify-content-between mb-4">
                        <MDBCheckbox 
                            name='rememberMe'
                            id='flexCheckDefault'
                            label='Remember me'
                            checked={formik.values.rememberMe} 
                            onChange={formik.handleChange} 
                            onBlur={formik.handleBlur}
                        />
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
