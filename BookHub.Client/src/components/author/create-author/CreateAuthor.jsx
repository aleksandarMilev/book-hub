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

import image from '../../../assets/images/createAuthor.jpg'
import NationalitySearch from './nationality-search/NationalitySearch'
import GenderRadio from './gender-radio/GenderRadio'

import './CreateAuthor.css';

export default function CreateAuthor() {
    const navigate = useNavigate()
    const createHandler = useAuthor.useCreate()
    const { nationalities, loading } = useAuthor.useNationalities()

    const validationSchema = Yup.object({
        name: Yup
            .string()
            .min(2, 'Name must be at least 2 characters long!')
            .max(200, 'Name cannot be longer than 200 characters!')
            .required('Name is required!'),
        penName: Yup
            .string()
            .min(2, 'Pen name must be at least 2 characters long!')
            .max(200, 'Pen name cannot be longer than 200 characters!'),
        imageUrl: Yup.string()
            .url('Enter a valid URL')
            .min(10, 'Image URL must be at least 10 characters long!')
            .max(2000, 'Image URL cannot be longer than 2000 characters!'),
        bornAt: Yup.date(),
        diedAt: Yup
            .date()
            .min(
                Yup.ref('bornAt'),
                'Date of death cannot be earlier than date of birth'
            ),
        gender: Yup
            .string()
            .required('Gender is required!'),
        nationality: Yup
            .string()
            .required('Nationality is required!'),
        biography: Yup
            .string()
            .min(50, 'Biography must be at least 50 characters long!')
            .max(10000, 'Biography cannot be longer than 10,000 characters!')
            .required('Biography is required!')
    })

    const formik = useFormik({
        initialValues: {
            name: '',
            penName: '',
            imageUrl: '',
            bornAt: '',
            diedAt: '',
            gender: '',
            nationality: '',
            biography: ''
        },
        validationSchema,
        onSubmit: async (values, { setErrors }) => {
            try {
                const authorId = await createHandler(values)
                navigate(routes.author + `/${authorId}`)
            } catch (error) {
                setErrors({ submit: error.message })
            }
        }
    })

    return (
        <MDBContainer fluid className="create-author-container">
            <MDBRow className="d-flex justify-content-center align-items-center h-100">
                <MDBCol>
                    <MDBCard className="my-4 create-author-card">
                        <MDBRow className="g-0">
                            <MDBCol md="6" className="d-none d-md-block">
                                <MDBCardImage src={image} alt="Sample photo" className="create-author-image" fluid />
                            </MDBCol>
                            <MDBCol md="6">
                                <MDBCardBody className="text-black d-flex flex-column justify-content-center">
                                    <h3 className="mb-5 fw-bold">Add a New Author to Support Our Community!</h3>
                                    <form onSubmit={formik.handleSubmit}>
                                        <MDBRow>
                                            <MDBCol md="12">
                                                {formik.touched.name && formik.errors.name && (
                                                    <div className="text-danger">{formik.errors.name}</div>
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
                                                />
                                            </MDBCol>
                                        </MDBRow>
                                        <MDBRow>
                                            <MDBCol md="12">
                                                {formik.touched.imageUrl && formik.errors.imageUrl && (
                                                    <div className="text-danger">{formik.errors.imageUrl}</div>
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
                                                <MDBInput
                                                    wrapperClass="mb-4"
                                                    label="Date of Birth"
                                                    size="lg"
                                                    id="bornAt"
                                                    type="date"
                                                    {...formik.getFieldProps('bornAt')}
                                                />
                                            </MDBCol>
                                        </MDBRow>
                                        <MDBRow>
                                            <MDBCol md="12">
                                                {formik.touched.diedAt && formik.errors.diedAt && (
                                                    <div className="text-danger">{formik.errors.diedAt}</div>
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
                                        />
                                        <MDBRow>
                                            <MDBCol md="12">
                                                {formik.touched.biography && formik.errors.biography && (
                                                    <div className="text-danger">{formik.errors.biography}</div>
                                                )}
                                                <textarea
                                                    id="biography"
                                                    rows="12"
                                                    {...formik.getFieldProps('biography')}
                                                    className={formik.touched.biography && formik.errors.biography ? 'is-invalid biography-textarea' : 'biography-textarea'}
                                                    placeholder="Write the author's biography here..."
                                                />
                                            </MDBCol>
                                        </MDBRow>

                                        <p className="text-danger fw-bold">Fields marked with * are required</p>
                                        <div className="d-flex justify-content-end pt-3">
                                            <MDBBtn color="light" size="lg" onClick={formik.handleReset}>
                                                Reset All
                                            </MDBBtn>
                                            <MDBBtn className="ms-2" color="warning" size="lg" type="submit">
                                                Submit Form
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
