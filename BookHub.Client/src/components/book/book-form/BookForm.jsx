import { useState } from 'react'
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
import * as useBook from '../../../hooks/useBook'
import * as useAuthor from '../../../hooks/useAuthor'

import AuthorSearch from './author-search/AuthorSearch'
import GenreSearch from './genre-search/GenreSearch' 

import bookImage from '../../../assets/images/createBook.jpg'

import './BookForm.css'

export default function BookForm({ bookData = null, isEditMode = false }) {
    const navigate = useNavigate()
    const createHandler = useBook.useCreate()
    const editHandler = useBook.useEdit()
    const { authors, loading: authorsLoading } = useAuthor.useNames()
    const { genres, isFetching: genresLoading } = useBook.useGenres()

    const [selectedGenres, setSelectedGenres] = useState([])

    const validationSchema = Yup.object({
        title: Yup.string().min(2).max(200).required('Title is required!'),
        authorId: Yup.number().nullable(),
        imageUrl: Yup.string().url().min(10).max(2000),
        shortDescription: Yup.string().min(10).max(100).required('You should provide some short description'),
        longDescription: Yup.string().min(100).max(5000).required('You should provide a long description!'),
        genres: Yup.array().min(1, 'At least one genre is required').required('Genres are required'), 
        publishedDate: Yup.date().max(new Date(), 'Published date must be in the past')
    })

    const formik = useFormik({
        initialValues: {
            title: bookData?.title || '',
            //authorName: bookData?.authorName || '',
            authorId: bookData?.authorId || '',
            imageUrl: bookData?.imageUrl || '',
            publishedDate: bookData?.publishedDate || '',
            shortDescription: bookData?.shortDescription || '',
            longDescription: bookData?.longDescription || '',
            genres: selectedGenres.map(g => g.id)
        },
        validationSchema,
        onSubmit: async (values, { setErrors }) => {
            try {
                if (isEditMode) {
                    await editHandler(bookData.id, { ...values }) 
                    navigate(routes.books + `/${bookData.id}`)
                } else {
                    const bookId = await createHandler({ ...values }) 
                    navigate(routes.books + `/${bookId}`)
                }
            } catch (error) {
                setErrors({ submit: error.message })
            }
        }
    })

    return (
        <MDBContainer fluid className="book-form-container">
            <MDBRow className="d-flex justify-content-center align-items-center h-100">
                <MDBCol>
                    <MDBCard className="my-4 book-form-card">
                        <MDBRow className="g-0">
                            <MDBCol md="6" className="book-form-image-col">
                                <MDBCardImage src={bookImage} alt="Book" className="book-form-image" fluid />
                            </MDBCol>
                            <MDBCol md="6">
                                <MDBCardBody className="text-black d-flex flex-column justify-content-center">
                                    <h3 className="mb-5 fw-bold">
                                        {isEditMode ? 'Edit Book' : 'Add a New Book to Our Collection!'}
                                    </h3>
                                    <form onSubmit={formik.handleSubmit}>
                                        <MDBRow>
                                            <MDBCol md="12">
                                                {formik.touched.title && formik.errors.title && (
                                                    <div className="text-danger">{formik.errors.title}</div>
                                                )}
                                                <MDBInput
                                                    wrapperClass="mb-4"
                                                    label="Title *"
                                                    size="lg"
                                                    id="title"
                                                    type="text"
                                                    {...formik.getFieldProps('title')}
                                                    className={formik.touched.title && formik.errors.title ? 'is-invalid' : ''}
                                                />
                                            </MDBCol>
                                        </MDBRow>
                                        <MDBRow>
                                            <MDBCol md="12">
                                                <AuthorSearch
                                                    authors={authors}
                                                    loading={authorsLoading}
                                                    formik={formik}
                                                />
                                            </MDBCol>
                                        </MDBRow>
                                        <MDBRow>
                                            <MDBCol md="12">
                                                {formik.touched.genres && formik.errors.genres && (
                                                    <div className="text-danger">{formik.errors.genres}</div>
                                                )}
                                                <GenreSearch
                                                    genres={genres}
                                                    loading={genresLoading}
                                                    formik={formik}
                                                    selectedGenres={selectedGenres}
                                                    setSelectedGenres={setSelectedGenres}
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
                                                {formik.touched.publishedDate && formik.errors.publishedDate && (
                                                    <div className="text-danger">{formik.errors.publishedDate}</div>
                                                )}
                                                <MDBInput
                                                    wrapperClass="mb-4"
                                                    label="Published Date"
                                                    size="lg"
                                                    id="publishedDate"
                                                    type="date"
                                                    {...formik.getFieldProps('publishedDate')}
                                                    className={formik.touched.publishedDate && formik.errors.publishedDate ? 'is-invalid' : ''}
                                                />
                                            </MDBCol>
                                        </MDBRow>
                                        <MDBRow>
                                            <MDBCol md="12">
                                                {formik.touched.shortDescription && formik.errors.shortDescription && (
                                                    <div className="text-danger">{formik.errors.shortDescription}</div>
                                                )}
                                                <MDBInput
                                                    wrapperClass="mb-4"
                                                    label="Short Description *"
                                                    size="lg"
                                                    id="shortDescription"
                                                    type="text"
                                                    {...formik.getFieldProps('shortDescription')}
                                                    className={formik.touched.shortDescription && formik.errors.shortDescription ? 'is-invalid' : ''}
                                                />
                                            </MDBCol>
                                        </MDBRow>
                                        <MDBRow>
                                            <MDBCol md="12">
                                                {formik.touched.longDescription && formik.errors.longDescription && (
                                                    <div className="text-danger">{formik.errors.longDescription}</div>
                                                )}
                                                <textarea
                                                    id="longDescription"
                                                    rows="10"
                                                    {...formik.getFieldProps('longDescription')}
                                                    className={`form-control ${formik.touched.longDescription && formik.errors.longDescription ? 'is-invalid' : ''}`}
                                                    placeholder="Write the book's full description here... *"
                                                />
                                            </MDBCol>
                                        </MDBRow>
                                        <p className="text-danger fw-bold mt-2">Fields marked with * are required</p>
                                        <div className="d-flex justify-content-end pt-3">
                                            <MDBBtn color="light" size="lg" onClick={formik.handleReset}>
                                                Reset All
                                            </MDBBtn>
                                            <MDBBtn className="ms-2" color="warning" size="lg" type="submit">
                                                {isEditMode ? 'Update Book' : 'Submit Form'}
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
