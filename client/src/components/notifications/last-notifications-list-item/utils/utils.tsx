import { FaBook, FaUser } from 'react-icons/fa';
import { routes } from '../../../../common/constants/api';

export const getIcon = (resourceType: string) => {
  switch (resourceType) {
    case 'Book':
      return <FaBook className="notification-icon" />;
    case 'Author':
      return <FaUser className="notification-icon" />;
    default:
      return null;
  }
};

export const mapResourceRoute = (resourceType: string) => {
  switch (resourceType) {
    case 'Book':
      return routes.book;
    case 'Author':
      return routes.author;
    default:
      return '/';
  }
};
