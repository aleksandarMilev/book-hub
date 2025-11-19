import './RenderStars.css';

import type { FC } from 'react';
import { FaRegStar,FaStar } from 'react-icons/fa';

export const RenderStars: FC<{
  rating: number;
}> = ({ rating }) => {
  const filled = Math.round(rating);
  const total = 5;

  return (
    <div className="bh-stars">
      <span className="bh-stars-number">{rating.toFixed(2)}</span>
      {[...Array(total)].map((_, i) =>
        i < filled ? (
          <FaStar key={i} className="bh-star filled" />
        ) : (
          <FaRegStar key={i} className="bh-star empty" />
        ),
      )}
    </div>
  );
};
