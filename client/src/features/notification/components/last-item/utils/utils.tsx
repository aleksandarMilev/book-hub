import { FaBook, FaUser } from 'react-icons/fa';

import { routes } from '@/shared/lib/constants/api.js';

export const getIcon = (resourceType: number) => {
  switch (resourceType) {
    case 0:
      return <FaBook className="notification-icon" />;
    case 1:
      return <FaUser className="notification-icon" />;
    default:
      return null;
  }
};

export const mapResourceRoute = (resourceType: number) => {
  switch (resourceType) {
    case 0:
      return routes.book;
    case 1:
      return routes.author;
    default:
      return routes.home;
  }
};
