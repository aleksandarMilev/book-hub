import { useContext } from "react"
import { useParams } from "react-router-dom"
import { format } from 'date-fns'
import { FaBirthdayCake, FaMapMarkerAlt, FaCalendarAlt } from 'react-icons/fa'

import renderStars from '../../common/functions/renderStars'
import * as useAuthor from '../../hooks/useAuthor'
import { UserContext } from "../../contexts/userContext"

import DefaultSpinner from "../common/DefaultSpinner"

export default function AuthorDetails() {
    const { id } = useParams()
    const { userId } = useContext(UserContext)
    const { author, isFetching } = useAuthor.useGetDetails(id)

    const creatorId = author ? author.creatorId : null
    const isCreator = userId === creatorId

    return (
        !isFetching ? (
            author ? (
                <div style={{ maxWidth: '1200px', margin: '0 auto', padding: '20px', backgroundColor: '#f9f9f9', borderRadius: '10px', boxShadow: '0 4px 8px rgba(0, 0, 0, 0.1)' }}>
                    <div style={{ display: 'flex', flexDirection: 'row', justifyContent: 'space-between', gap: '20px' }}>
                        <div style={{ flex: 1 }}>
                            <div style={{ boxShadow: '0 4px 8px rgba(0, 0, 0, 0.2)', borderRadius: '12px' }}>
                                <img
                                    style={{ maxWidth: '100%', borderRadius: '12px', boxShadow: '0 4px 8px rgba(0, 0, 0, 0.2)' }}
                                    className="img-fluid rounded"
                                    src={author.imageUrl}
                                    alt={`${author.name}'s profile`}
                                />
                            </div>
                        </div>
                        <div style={{ flex: 2 }}>
                            <div style={{ padding: '0 20px' }}>
                                <div style={{ color: '#007bff', fontSize: '1rem', fontWeight: 'bold', textTransform: 'uppercase' }} className="category">
                                    <FaMapMarkerAlt className="me-2" /> {author.nationality}
                                </div>
                                <div style={{ fontSize: '2.2rem', fontWeight: 'bold', color: '#333' }} className="product-title my-3">
                                    <h2>{author.name}</h2>
                                </div>
                                <div style={{ fontSize: '2rem', fontWeight: 'bold', color: '#333' }} className="product-title my-3">
                                    <h5><span>Pen Name: </span>{author.penName ? author.penName : 'Unknown'}</h5>
                                </div>
                                <div style={{ marginBottom: '20px' }}>
                                    <p style={{ fontSize: '1rem', color: '#555', lineHeight: '1.7' }}>
                                        <FaBirthdayCake className="me-2 text-warning" />
                                        {author.bornAt ? `Born: ${format(new Date(author.bornAt), 'MMMM dd, yyyy')}` : 'Birthdate unknown'}
                                    </p>
                                    {author.diedAt ? (
                                        <p style={{ fontSize: '1rem', color: '#555', lineHeight: '1.7' }}>
                                            <FaCalendarAlt className="me-2 text-danger" />
                                            Died: {format(new Date(author.diedAt), 'MMMM dd, yyyy')}
                                        </p>
                                    ) : (
                                        <p style={{ fontSize: '1rem', color: '#555', lineHeight: '1.7' }}>
                                            <FaCalendarAlt className="me-2 text-success" />
                                            Still Alive
                                        </p>
                                    )}
                                </div>
                                <div className="d-flex align-items-center">
                                    {renderStars(author.rating)}
                                </div>
                                <p style={{ fontWeight: 'bold', marginBottom: '1rem' }}>Biography:</p>
                                <p style={{ fontSize: '1rem', color: '#555', lineHeight: '1.7' }}>{author.biography}</p>
                                {isCreator && (
                                    <div style={{ marginTop: '20px' }}>
                                        <button 
                                            style={{
                                                padding: '10px 20px', 
                                                fontSize: '1rem', 
                                                backgroundColor: '#007bff', 
                                                color: 'white', 
                                                border: 'none', 
                                                borderRadius: '5px', 
                                                cursor: 'pointer', 
                                                marginRight: '10px'
                                            }}
                                            onClick={() => alert('Edit functionality to be implemented')}
                                        >
                                            Edit
                                        </button>
                                        <button 
                                            style={{
                                                padding: '10px 20px', 
                                                fontSize: '1rem', 
                                                backgroundColor: '#dc3545', 
                                                color: 'white', 
                                                border: 'none', 
                                                borderRadius: '5px', 
                                                cursor: 'pointer'
                                            }}
                                            onClick={() => alert('Delete functionality to be implemented')}
                                        >
                                            Delete
                                        </button>
                                    </div>
                                )}
                            </div>
                        </div>
                    </div>
                </div>
            ) : (
                <div style={{ backgroundColor: '#f8d7da', borderColor: '#f5c6cb', textAlign: 'center', padding: '20px', borderRadius: '8px' }}>
                    <h4 style={{ fontSize: '1.5rem', fontWeight: 'bold', color: '#721c24' }}>Oops!</h4>
                    <p>The author you are looking for was not found.</p>
                </div>
            )
        ) : (
            <div style={{ display: 'flex', justifyContent: 'center', alignItems: 'center', minHeight: '50vh' }}>
                <DefaultSpinner />
            </div>
        )
    )
}
