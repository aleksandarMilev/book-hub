import axios from 'axios';

import { baseAdminUrl, baseUrl } from '../../common/constants/api';

export const http = axios.create({ baseURL: baseUrl });
export const httpAdmin = axios.create({ baseURL: baseAdminUrl });
