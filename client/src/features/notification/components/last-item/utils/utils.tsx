import { FaBook, FaUser } from 'react-icons/fa';

import type { ResourceType } from '@/features/notification/components/last-item/types/resourceType';
import { routes } from '@/shared/lib/constants/api';

export const getIcon = (resourceType: ResourceType) => {
  switch (resourceType) {
    case 'Book':
      return <FaBook className="notification-icon" />;
    case 'Author':
      return <FaUser className="notification-icon" />;
    default:
      return null;
  }
};

export const mapResourceRoute = (resourceType: ResourceType) => {
  switch (resourceType) {
    case 'Book':
      return routes.book;
    case 'Author':
      return routes.author;
    default:
      return routes.home;
  }
};
