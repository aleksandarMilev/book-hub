import axios from 'axios';

import type { Statistics } from './types/statistics';

import { baseUrl, routes } from '../../common/constants/api';
import { errors } from '../../common/constants/messages';


export async function all() {
  try {
    const url = `${baseUrl}${routes.statistics}`;
    const response = await axios.get<Statistics>(url);

    return response.data;
  } catch {
    throw new Error(errors.statistics.get);
  }
}
