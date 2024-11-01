import React from 'react';
import { useNavigate } from 'react-router-dom';
import { MDBBtn, MDBContainer, MDBRow, MDBCol, MDBCard, MDBCardBody, MDBCardImage, MDBInput, MDBIcon, MDBCheckbox } from 'mdb-react-ui-kit';

import { apiRoutes } from '../../common/constants';
import { useForm } from '../../hooks/useForm';
import { register } from '../../api/identityApi.js';

export default function Register() {
    const navigate = useNavigate()

    const initValues = {
        username: '',
        email: '',
        password: '',
        confirmPassword: ''
    }

    async function onRegister() {
        const user = { 
            username: values.username,
            email: values.email,
            password: values.password
        }

        await register({...user})
        navigate(apiRoutes.home)
    }

    const { values, onChange, onSubmit } = useForm(initValues, onRegister)

    return (
        <MDBContainer fluid>
            <MDBCard className='text-black m-5' style={{ borderRadius: '25px' }}>
                <MDBCardBody>
                    <MDBRow>
                        <MDBCol md='10' lg='6' className='order-2 order-lg-1 d-flex flex-column align-items-center'>
                            <p className="text-center h1 fw-bold mb-5 mx-1 mx-md-4 mt-4">Sign up</p>
                            <div className="d-flex flex-row align-items-center mb-4 ">
                                <MDBIcon fas icon="user me-3" size='lg'/>
                                <MDBInput 
                                    label='Your Name' 
                                    id='form1' 
                                    type='text' 
                                    name="username"
                                    value={values.username} 
                                    onChange={onChange} 
                                    className='w-100'
                                />
                            </div>
                            <div className="d-flex flex-row align-items-center mb-4">
                                <MDBIcon fas icon="envelope me-3" size='lg'/>
                                <MDBInput 
                                    label='Your Email' 
                                    id='form2' 
                                    type='email' 
                                    name="email" 
                                    value={values.email} 
                                    onChange={onChange}
                                />
                            </div>
                            <div className="d-flex flex-row align-items-center mb-4">
                                <MDBIcon fas icon="lock me-3" size='lg'/>
                                <MDBInput 
                                    label='Password' 
                                    id='form3' 
                                    type='password' 
                                    name="password" 
                                    value={values.password} 
                                    onChange={onChange}
                                />
                            </div>
                            <div className="d-flex flex-row align-items-center mb-4">
                                <MDBIcon fas icon="key me-3" size='lg'/>
                                <MDBInput 
                                    label='Repeat your password' 
                                    id='form4' 
                                    type='password' 
                                    name="confirmPassword" 
                                    value={values.confirmPassword} 
                                    onChange={onChange}
                                />
                            </div>
                            <div className='mb-4'>
                                <MDBCheckbox name='flexCheck' value='' id='flexCheckDefault' label='Subscribe to our newsletter' />
                            </div>
                            <MDBBtn className='mb-4' size='lg' onClick={onSubmit}>Register</MDBBtn>
                        </MDBCol>
                        <MDBCol md='10' lg='6' className='order-1 order-lg-2 d-flex align-items-center'>
                            <MDBCardImage src='https://mdbcdn.b-cdn.net/img/Photos/new-templates/bootstrap-registration/draw1.webp' fluid/>
                        </MDBCol>
                    </MDBRow>
                </MDBCardBody>
            </MDBCard>
        </MDBContainer>
    );
}
