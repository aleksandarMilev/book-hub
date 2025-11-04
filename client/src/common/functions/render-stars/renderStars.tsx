import type { FC } from 'react';
import { FaStar, FaRegStar } from 'react-icons/fa';
import type { Props } from './types/props';

export const RenderStars: FC<Props> = ({ rating }) => {
  const filledStars = Math.round(rating);
  const totalStars = 5;

  return (
    <div className="d-flex align-items-center justify-content-center">
      <span
        style={{
          fontSize: '1.25rem',
          color: '#FFA500',
          marginRight: '6px',
        }}
      >
        {rating.toFixed(2)}
      </span>

      {[...Array(filledStars)].map((_, i) => (
        <FaStar key={`filled-${i}`} color="gold" size={20} />
      ))}

      {[...Array(totalStars - filledStars)].map((_, i) => (
        <FaRegStar key={`empty-${i}`} color="grey" size={20} />
      ))}
    </div>
  );
};
