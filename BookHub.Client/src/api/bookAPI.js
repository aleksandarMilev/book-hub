import { baseUrl } from '../common/constants'
import * as requester from './requester'

export const getAllAsync = async () => await requester.getAsync(baseUrl)