export const getResourceType = (serverResourceType: number) => {
  switch (serverResourceType) {
    case 0:
      return 'Book';
    case 1:
      return 'Author';
    default:
      return 'Unknown resource type';
  }
};

