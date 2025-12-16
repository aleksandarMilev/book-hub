import type { FC } from 'react';
import { useLogoutEffect } from '../../hooks/useLogoutEffect.js';

const Logout: FC = () => {
  useLogoutEffect();
  return null;
};

export default Logout;
