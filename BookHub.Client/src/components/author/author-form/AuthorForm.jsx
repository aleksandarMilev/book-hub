import { useNavigate } from 'react-router-dom'
import * as Yup from 'yup'
import { useFormik } from 'formik'
import {
    MDBBtn,
    MDBContainer,
    MDBCard,
    MDBCardBody,
    MDBCardImage,
    MDBRow,
    MDBCol,
    MDBInput
} from 'mdb-react-ui-kit'

import { routes } from '../../../common/constants/api'
import * as useAuthor from '../../../hooks/useAuthor'
import * as useNationality from '../../../hooks/useNationality'

import image from '../../../assets/images/createAuthor.jpg'
import NationalitySearch from './nationality-search/NationalitySearch'
import GenderRadio from './gender-radio/GenderRadio'

import './AuthorForm.css'

export default function AuthorForm({ authorData = null, isEditMode = false }) {
    const navigate = useNavigate()

    const createHandler = useAuthor.useCreate()
    const editHandler = useAuthor.useEdit()  

    const { nationalities, loading } = useNationality.useNationalities()

    const validationSchema = Yup.object({
        name: Yup
            .string()
            .min(2)
            .max(200)
            .required('Name is required!'),
        penName: Yup
            .string()
            .min(2)
            .max(200),
        imageUrl: Yup
            .string()
            .url()
            .min(10)
            .max(2000),
        bornAt: Yup
            .date()
            .max(new Date(), 'Date of birth must be in the past'),
        diedAt: Yup
            .date()
            .max(new Date(), 'Date of birth must be in the past')
            .min(Yup.ref('bornAt'), 'Date of death cannot be earlier than date of birth'),
        gender: Yup
            .string()
            .required('Gender is required!'),
        nationality: Yup
            .string()
            .required('Nationality is required!'),
        biography: Yup
            .string()
            .min(50)
            .max(10000)
            .required('Biography is required!')
    })

    const initialNationality = isEditMode && authorData?.nationality
        ? authorData.nationality.id
        : '';

    const initialNationalityName = isEditMode && authorData?.nationality
        ? authorData.nationality.name
        : '';

    const formik = useFormik({
        initialValues: {
            name: authorData?.name || '',
            penName: authorData?.penName || '',
            imageUrl: authorData?.imageUrl || '',
            bornAt: authorData?.bornAt || '',
            diedAt: authorData?.diedAt || '',
            gender: authorData?.gender || '',
            nationality: initialNationality, 
            nationalityName: initialNationalityName, 
            biography: authorData?.biography || ''
        },
        validationSchema,
        onSubmit: async (values, { setErrors }) => {
            try {
                const finalValues = { ...values, nationality: values.nationality || initialNationality }
                
                if (isEditMode) {
                    const isSuccessfullyEdited = await editHandler(authorData.id, finalValues)
                      
                    if(isSuccessfullyEdited){
                        navigate(routes.author + `/${authorData.id}`)
                    }
                } else {
                    const authorId = await createHandler(finalValues)  

                    if(authorId){
                        navigate(routes.author + `/${authorId}`)
                    }
                }
            } catch (error) {
                setErrors({ submit: error.message })
            }
        }
    })

    return (
        <MDBContainer fluid className="author-form-container">
            <MDBRow className="d-flex justify-content-center align-items-center h-100">
                <MDBCol>
                    <MDBCard className="my-4 author-form-card">
                        <MDBRow className="g-0">
                            <MDBCol md="6" className="d-none d-md-block">
                                <MDBCardImage src={image} alt="Author" className="author-form-image" fluid />
                            </MDBCol>
                            <MDBCol md="6">
                                <MDBCardBody className="text-black d-flex flex-column justify-content-center">
                                    <h3 className="mb-5 fw-bold">
                                        {isEditMode ? 'Edit Author' : 'Add a New Author to Support Our Community!'}
                                    </h3>
                                    <form onSubmit={formik.handleSubmit}>
                                        <MDBRow>
                                            <MDBCol md="12">
                                                {formik.touched.name && formik.errors.name && (
                                                    <div className="text-danger mb-2">{formik.errors.name}</div>
                                                )}
                                                <MDBInput
                                                    wrapperClass="mb-4"
                                                    label="Name *"
                                                    size="lg"
                                                    id="name"
                                                    type="text"
                                                    {...formik.getFieldProps('name')}
                                                    className={formik.touched.name && formik.errors.name ? 'is-invalid' : ''}
                                                />
                                            </MDBCol>
                                        </MDBRow>
                                        <MDBRow>
                                            <MDBCol md="12">
                                                <MDBInput
                                                    wrapperClass="mb-4"
                                                    label="Pen Name"
                                                    size="lg"
                                                    id="penName"
                                                    type="text"
                                                    {...formik.getFieldProps('penName')}
                                                    className={formik.touched.penName && formik.errors.penName ? 'is-invalid' : ''}
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
                                                    label="Image URL"
                                                    size="lg"
                                                    id="imageUrl"
                                                    type="text"
                                                    {...formik.getFieldProps('imageUrl')}
                                                    className={formik.touched.imageUrl && formik.errors.imageUrl ? 'is-invalid' : ''}
                                                />
                                            </MDBCol>
                                        </MDBRow>
                                        <MDBRow>
                                            <MDBCol md="12">
                                                {formik.touched.bornAt && formik.errors.bornAt && (
                                                    <div className="text-danger mb-2">{formik.errors.bornAt}</div>
                                                )}
                                                <MDBInput
                                                    wrapperClass="mb-4"
                                                    label="Date of Birth"
                                                    size="lg"
                                                    id="bornAt"
                                                    type="date"
                                                    {...formik.getFieldProps('bornAt')}
                                                    className={formik.touched.bornAt && formik.errors.bornAt ? 'is-invalid' : ''}
                                                />
                                            </MDBCol>
                                        </MDBRow>
                                        <MDBRow>
                                            <MDBCol md="12">
                                                {formik.touched.diedAt && formik.errors.diedAt && (
                                                    <div className="text-danger mb-2">{formik.errors.diedAt}</div>
                                                )}
                                                <MDBInput
                                                    wrapperClass="mb-4"
                                                    label="Date of Death (if applicable)"
                                                    size="lg"
                                                    id="diedAt"
                                                    type="date"
                                                    {...formik.getFieldProps('diedAt')}
                                                    className={formik.touched.diedAt && formik.errors.diedAt ? 'is-invalid' : ''}
                                                />
                                            </MDBCol>
                                        </MDBRow>
                                        <GenderRadio formik={formik} />
                                        <NationalitySearch
                                            nationalities={nationalities}
                                            loading={loading}
                                            formik={formik}
                                            selectedNationalityName={formik.values.nationalityName}
                                        />
                                        <MDBRow>
                                            <MDBCol md="12">
                                                {formik.touched.biography && formik.errors.biography && (
                                                    <div className="text-danger mb-2">{formik.errors.biography}</div>
                                                )}
                                                <textarea
                                                    id="biography"
                                                    rows="6"
                                                    {...formik.getFieldProps('biography')}
                                                    className={`form-control ${formik.touched.biography && formik.errors.biography ? 'is-invalid' : ''}`}
                                                    placeholder="Write the author's biography here..."
                                                />
                                            </MDBCol>
                                        </MDBRow>
                                        <p className="text-danger fw-bold mt-2">Fields marked with * are required</p>
                                        <div className="d-flex justify-content-end pt-3">
                                            <MDBBtn color="light" size="lg" onClick={formik.handleReset}>
                                                Reset All
                                            </MDBBtn>
                                            <MDBBtn className="ms-2" color="warning" size="lg" type="submit">
                                                {isEditMode ? 'Update Author' : 'Submit Form'}
                                            </MDBBtn>
                                        </div>
                                    </form>
                                </MDBCardBody>
                            </MDBCol>
                        </MDBRow>
                    </MDBCard>
                </MDBCol>
            </MDBRow>
        </MDBContainer>
    )
}
