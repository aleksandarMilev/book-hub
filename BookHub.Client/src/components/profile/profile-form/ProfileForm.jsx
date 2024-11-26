import { useNavigate } from 'react-router-dom'
import * as Yup from 'yup'
import { useFormik } from 'formik'
import {
    MDBBtn,
    MDBContainer,
    MDBCard,
    MDBCardBody,
    MDBRow,
    MDBCol,
    MDBInput
} from 'mdb-react-ui-kit'

import * as useProfile from '../../../hooks/useProfile'
import { routes } from '../../../common/constants/api'

import './ProfileForm.css'

export default function ProfileForm({ profile = null, isEditMode = false }) {
    const navigate = useNavigate()

    const createHandler = useProfile.useCreate()
    const editHandler = useProfile.useEdit()

    const validationSchema = Yup.object({
        firstName: Yup
            .string()
            .min(2, 'First name length should be between 2 and 100 characters long')
            .max(100, 'First name length should be between 2 and 100 characters long')
            .required('First name is required'),
        lastName: Yup
            .string()
            .min(2, 'Last name length should be between 2 and 100 characters long')
            .max(100, 'Last name length should be between 2 and 100 characters long')
            .required('Last name is required'),
        imageUrl: Yup
            .string()
            .url('Invalid URL')
            .min(10, 'Image URL length should be between 10 and 2000 characters long')
            .max(2000, 'Image URL length should be between 10 and 2000 characters long')
            .required('Image URL is required'),
        phoneNumber: Yup
            .string()
            .matches(
                /^(?:\+359|0)\d{8,14}$/,
                'Phone number must start with +359 or 0 and be followed by 8 to 14 digits'
            )  
            .min(8, 'Phone Number length should be between 8 and 15 characters long')
            .max(15, 'Phone Number length should be between 8 and 15 characters long')
            .required('Phone Number is required'),
        dateOfBirth: Yup
            .date()
            .required('Date of birth is required')
            .typeError('Invalid date'),
        socialMediaUrl: Yup
            .string()
            .url('Invalid URL')
            .min(10, 'Social Media URL length should be between 10 and 2000 characters long')
            .max(2000, 'Social Media URL length should be between 10 and 2000 characters long')
            .nullable(),
        biography: Yup
            .string()
            .min(10, 'Biography length should be between 10 and 1000 characters long')
            .max(1000, 'Biography length should be between 10 and 1000 characters long')
            .nullable(),
        isPrivate: Yup
            .boolean()
    })

    const formik = useFormik({
        initialValues: {
            firstName: profile?.firstName || '',
            lastName: profile?.lastName || '',
            imageUrl: profile?.imageUrl || '',
            phoneNumber: profile?.phoneNumber || '',
            dateOfBirth: profile?.dateOfBirth || '',
            socialMediaUrl: profile?.socialMediaUrl || '',
            biography: profile?.biography || '',
            isPrivate: profile?.isPrivate !== undefined ? profile.isPrivate : false
        },
        validationSchema,
        onSubmit: async (values) => {
            if (isEditMode) {
                await editHandler(values) 
            } else {
                await createHandler(values)
            }

            navigate(routes.profile)
        }
    })

    return(
            <div className="profile-form-container">
                <MDBContainer fluid>
                    <MDBRow className="form-row">
                        <MDBCol className="form-col">
                            <MDBCard className="my-4 profile-details-card">
                                <MDBCardBody className="text-black">
                                    <h3 className="mb-5 fw-bold">
                                        {isEditMode ? 'Edit Your Profile' : 'Create Your Profile'}
                                    </h3>
                                    <form onSubmit={formik.handleSubmit}>
                                        <MDBRow>
                                            <MDBCol md="12">
                                                {formik.touched.firstName && formik.errors.firstName && (
                                                    <div className="text-danger mb-2">{formik.errors.firstName}</div>
                                                )}
                                                <MDBInput
                                                    wrapperClass="mb-4"
                                                    label="First Name *"
                                                    size="lg"
                                                    id="firstName"
                                                    type="text"
                                                    {...formik.getFieldProps('firstName')}
                                                    className={
                                                        formik.touched.firstName && formik.errors.firstName
                                                            ? 'is-invalid'
                                                            : ''
                                                    }
                                                />
                                            </MDBCol>
                                        </MDBRow>
                                        <MDBRow>
                                            <MDBCol md="12">
                                                {formik.touched.lastName && formik.errors.lastName && (
                                                    <div className="text-danger mb-2">{formik.errors.lastName}</div>
                                                )}
                                                <MDBInput
                                                    wrapperClass="mb-4"
                                                    label="Last Name *"
                                                    size="lg"
                                                    id="lastName"
                                                    type="text"
                                                    {...formik.getFieldProps('lastName')}
                                                    className={
                                                        formik.touched.lastName && formik.errors.lastName
                                                            ? 'is-invalid'
                                                            : ''
                                                    }
                                                />
                                            </MDBCol>
                                        </MDBRow>
                                        <MDBRow>
                                            <MDBCol md="12">
                                                {formik.touched.imageUrl && formik.errors.imageUrl && (
                                                    <div className="text-danger mb-2">{formik.errors.imageUrl}</div>
                                                )}
                                                <MDBInput
                                                    wrapperClass="mb-4"
                                                    label="Image URL *"
                                                    size="lg"
                                                    id="imageUrl"
                                                    type="text"
                                                    {...formik.getFieldProps('imageUrl')}
                                                    className={
                                                        formik.touched.imageUrl && formik.errors.imageUrl
                                                            ? 'is-invalid'
                                                            : ''
                                                    }
                                                />
                                            </MDBCol>
                                        </MDBRow>
                                        <MDBRow>
                                            <MDBCol md="12">
                                                {formik.touched.phoneNumber && formik.errors.phoneNumber && (
                                                    <div className="text-danger mb-2">{formik.errors.phoneNumber}</div>
                                                )}
                                                <MDBInput
                                                    wrapperClass="mb-4"
                                                    label="Phone Number *"
                                                    size="lg"
                                                    id="phoneNumber"
                                                    type="text"
                                                    {...formik.getFieldProps('phoneNumber')}
                                                    className={
                                                        formik.touched.phoneNumber && formik.errors.phoneNumber
                                                            ? 'is-invalid'
                                                            : ''
                                                    }
                                                />
                                            </MDBCol>
                                        </MDBRow>
                                        <MDBRow>
                                            <MDBCol md="12">
                                                {formik.touched.dateOfBirth && formik.errors.dateOfBirth && (
                                                    <div className="text-danger mb-2">{formik.errors.dateOfBirth}</div>
                                                )}
                                                <MDBInput
                                                    wrapperClass="mb-4"
                                                    label="Date of Birth *"
                                                    size="lg"
                                                    id="dateOfBirth"
                                                    type="date"
                                                    {...formik.getFieldProps('dateOfBirth')}
                                                    className={
                                                        formik.touched.dateOfBirth && formik.errors.dateOfBirth
                                                            ? 'is-invalid'
                                                            : ''
                                                    }
                                                />
                                            </MDBCol>
                                        </MDBRow>
                                        <MDBRow>
                                            <MDBCol md="12">
                                                {formik.touched.socialMediaUrl && formik.errors.socialMediaUrl && (
                                                    <div className="text-danger mb-2">
                                                        {formik.errors.socialMediaUrl}
                                                    </div>
                                                )}
                                                <MDBInput
                                                    wrapperClass="mb-4"
                                                    label="Social Media URL"
                                                    size="lg"
                                                    id="socialMediaUrl"
                                                    type="text"
                                                    {...formik.getFieldProps('socialMediaUrl')}
                                                    className={
                                                        formik.touched.socialMediaUrl && formik.errors.socialMediaUrl
                                                            ? 'is-invalid'
                                                            : ''
                                                    }
                                                />
                                            </MDBCol>
                                        </MDBRow>
                                        <MDBRow>
                                            <MDBCol md="12">
                                                {formik.touched.biography && formik.errors.biography && (
                                                    <div className="text-danger mb-2">{formik.errors.biography}</div>
                                                )}
                                                <textarea
                                                    id="biography"
                                                    rows="5"
                                                    {...formik.getFieldProps('biography')}
                                                    className={`form-control ${
                                                        formik.touched.biography && formik.errors.biography
                                                            ? 'is-invalid'
                                                            : ''
                                                    }`}
                                                    placeholder="Write a short biography... (Optional)"
                                                />
                                            </MDBCol>
                                        </MDBRow>
                                        <div className="form-check mb-4">
                                        <input
                                            type="checkbox"
                                            id="isPrivate"
                                            className="form-check-input"
                                            checked={formik.values.isPrivate}
                                            onChange={formik.handleChange}
                                            onBlur={formik.handleBlur}
                                        />
                                            <label htmlFor="isPrivate" className="form-check-label">
                                                Keep my profile private
                                            </label>
                                        </div>
                                        <p className="text-danger fw-bold mt-2">Fields marked with * are required</p>
                                        <div className="d-flex justify-content-end pt-3">
                                            <MDBBtn color="light" size="lg" onClick={formik.handleReset}>
                                                Reset All
                                            </MDBBtn>
                                            <MDBBtn className="ms-2" color="warning" size="lg" type="submit">
                                                {isEditMode ? 'Update Profile' : 'Create Profile'}
                                            </MDBBtn>
                                        </div>
                                    </form>
                                    </MDBCardBody>
                                </MDBCard>
                            </MDBCol>
                        </MDBRow>
                    </MDBContainer>
                </div>
            )
}