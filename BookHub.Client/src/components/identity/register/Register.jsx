import React from 'react'
import { useNavigate } from 'react-router-dom'
import * as Yup from 'yup'
import { useFormik } from 'formik'
import { MDBBtn, MDBContainer, MDBRow, MDBCol, MDBCard, MDBCardBody, MDBCardImage, MDBInput, MDBIcon, MDBCheckbox } 
from 'mdb-react-ui-kit'

import { routes } from '../../../common/constants/api'
import * as useIdentity from '../../../hooks/useIdentity'

export default function Register() {
    const navigate = useNavigate()
    const onRegister = useIdentity.useRegister()

    const validationSchema = Yup.object({
        username: Yup.string().required('Username is required'),
        email: Yup.string().email('Invalid email format').required('Email is required'),
        password: Yup.string().required('Password is required'),
        confirmPassword: Yup
            .string()
            .oneOf([Yup.ref('password'), null], 'Passwords must match')
            .required('Please confirm your password')
    })

    const formik = useFormik({
        initialValues: {
            username: '',
            email: '',
            password: '',
            confirmPassword: ''
        },
        validationSchema,
        onSubmit: async (values, { setErrors }) => {
            try {
                await onRegister(values.username, values.email, values.password)
                navigate(routes.home);
            } catch (error) {
                setErrors({ email: error.message || 'An error occurred' })
            }
        }
    })

    return (
        <MDBContainer fluid>
            <MDBCard className='text-black m-5' style={{ borderRadius: '25px' }}>
                <MDBCardBody>
                    <MDBRow>
                        <MDBCol md='10' lg='6' className='order-2 order-lg-1 d-flex flex-column align-items-center'>
                            <p className="text-center h1 fw-bold mb-5 mx-1 mx-md-4 mt-4">Sign up</p>
                            <form onSubmit={formik.handleSubmit} className="w-100">
                                <div className="d-flex flex-row align-items-center mb-4">
                                    <MDBIcon fas icon="user me-3" size='lg'/>
                                    <MDBInput 
                                        label='Your Name' 
                                        id='username' 
                                        type='text' 
                                        name="username"
                                        value={formik.values.username}
                                        onChange={formik.handleChange}
                                        onBlur={formik.handleBlur} 
                                        className='w-100'
                                    />
                                </div>
                                {formik.touched.username && formik.errors.username && (
                                    <div className="text-danger mb-3">{formik.errors.username}</div>
                                )}

                                <div className="d-flex flex-row align-items-center mb-4">
                                    <MDBIcon fas icon="envelope me-3" size='lg'/>
                                    <MDBInput 
                                        label='Your Email' 
                                        id='email' 
                                        type='email' 
                                        name="email" 
                                        value={formik.values.email} 
                                        onChange={formik.handleChange}
                                        onBlur={formik.handleBlur}
                                        className='w-100'
                                    />
                                </div>
                                {formik.touched.email && formik.errors.email && (
                                    <div className="text-danger mb-3">{formik.errors.email}</div>
                                )}

                                <div className="d-flex flex-row align-items-center mb-4">
                                    <MDBIcon fas icon="lock me-3" size='lg'/>
                                    <MDBInput 
                                        label='Password' 
                                        id='password' 
                                        type='password' 
                                        name="password" 
                                        value={formik.values.password} 
                                        onChange={formik.handleChange}
                                        onBlur={formik.handleBlur}
                                        className='w-100'
                                    />
                                </div>
                                {formik.touched.password && formik.errors.password && (
                                    <div className="text-danger mb-3">{formik.errors.password}</div>
                                )}

                                <div className="d-flex flex-row align-items-center mb-4">
                                    <MDBIcon fas icon="key me-3" size='lg'/>
                                    <MDBInput 
                                        label='Repeat your password' 
                                        id='confirmPassword' 
                                        type='password' 
                                        name="confirmPassword" 
                                        value={formik.values.confirmPassword} 
                                        onChange={formik.handleChange}
                                        onBlur={formik.handleBlur}
                                        className='w-100'
                                    />
                                </div>
                                {formik.touched.confirmPassword && formik.errors.confirmPassword && (
                                    <div className="text-danger mb-3">{formik.errors.confirmPassword}</div>
                                )}

                                <div className='mb-4'>
                                    <MDBCheckbox name='flexCheck' value='' id='flexCheckDefault' label='Subscribe to our newsletter' />
                                </div>
                                <MDBBtn className='mb-4' size='lg' type="submit">Register</MDBBtn>
                            </form>
                        </MDBCol>
                        <MDBCol md='10' lg='6' className='order-1 order-lg-2 d-flex align-items-center'>
                            <MDBCardImage src='https://mdbcdn.b-cdn.net/img/Photos/new-templates/bootstrap-registration/draw1.webp' fluid/>
                        </MDBCol>
                    </MDBRow>
                </MDBCardBody>
            </MDBCard>
        </MDBContainer>
    )
}
