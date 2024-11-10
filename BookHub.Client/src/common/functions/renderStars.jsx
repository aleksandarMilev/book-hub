import { FaStar, FaRegStar } from 'react-icons/fa'

export default function renderStars(rating){
    const filledStars = Math.round(rating)
    const totalStars = 5

    return (
        <div className="d-flex align-items-center justify-content-center">
            <span style={{ fontSize: '1.25rem', color: '#FFA500', marginRight: '6px' }}>
                {rating.toFixed(2)}
            </span>
            {[...Array(filledStars)].map((_, i) => (
                <FaStar key={i} color="gold" size={20} />
            ))}
            {[...Array(totalStars - filledStars)].map((_, i) => (
                <FaRegStar key={i + filledStars} color="grey" size={20} />
            ))}
        </div>
    )
}
