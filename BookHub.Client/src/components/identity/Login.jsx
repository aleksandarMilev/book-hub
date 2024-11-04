import React from 'react'
import { useNavigate } from 'react-router-dom'
import {MDBContainer, MDBCol, MDBRow, MDBBtn, MDBIcon, MDBInput, MDBCheckbox } 
from 'mdb-react-ui-kit'

import { useForm } from '../../hooks/useForm'
import * as useIdentity from '../../hooks/useIdentity'
import { routes } from '../../common/constants/api'

export default function Login() {
    const navigate = useNavigate()
    const loginAsync = useIdentity.useLogin()

    const initValues = {
        username: '',
        password: ''
    }

    async function onLogin({username, password}){
        await loginAsync(username, password)
        navigate(routes.home)
    }

    const { values, onChange, onSubmit } = useForm(initValues, onLogin)

    return (
        <MDBContainer fluid className="p-3 my-5 h-custom">
        <MDBRow>
            <MDBCol col='10' md='6'>
            <img 
            src="https://mdbcdn.b-cdn.net/img/Photos/new-templates/bootstrap-login-form/draw2.webp" 
            className="img-fluid" 
            alt="Sample image" />
            </MDBCol>
            <MDBCol col='4' md='6'>
            <div className="d-flex flex-row align-items-center justify-content-center">
                <p className="lead fw-normal mb-0 me-3">Sign in with</p>
                <MDBBtn floating size='md' tag='a' className='me-2'>
                <MDBIcon fab icon='facebook-f' />
                </MDBBtn>
                <MDBBtn floating size='md' tag='a'  className='me-2'>
                <MDBIcon fab icon='twitter' />
                </MDBBtn>
                <MDBBtn floating size='md' tag='a'  className='me-2'>
                <MDBIcon fab icon='linkedin-in' />
                </MDBBtn>
            </div>
            <div className="divider d-flex align-items-center my-4">
                <p className="text-center fw-bold mx-3 mb-0">Or</p>
            </div>
            <MDBInput 
                wrapperClass='mb-4' 
                label='Username' 
                id='formControlLg' 
                type='text' 
                size="lg"
                name='username'
                value={values.username}
                onChange={onChange}
            />
            <MDBInput 
                wrapperClass='mb-4'
                label='Password' 
                id='formControlLg' 
                type='password' 
                size="lg"
                name='password'
                value={values.password}
                onChange={onChange}
            />
            <div className="d-flex justify-content-between mb-4">
                <MDBCheckbox name='flexCheck' value='' id='flexCheckDefault' label='Remember me' />
                <a href="!#">Forgot password?</a>
            </div>
            <div className='text-center text-md-start mt-4 pt-2'>
                <MDBBtn 
                    className="mb-0 px-5" 
                    size='lg'
                    onSubmit={onSubmit}
                >
                    Login
                </MDBBtn>
                <p 
                className="small fw-bold mt-2 pt-1 mb-2">
                    Don't have an account? <a href="#!" className="link-danger">Register</a>
                </p>
            </div>
            </MDBCol>
        </MDBRow>
        </MDBContainer>
    )
}
