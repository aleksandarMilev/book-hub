import { baseUrl, apiRoutes } from '../common/constants'
import * as requester from './requester'

export async function getAllAsync(){
    const response =  await requester.getAsync(baseUrl + apiRoutes.books)
    return response.ok ? response.json() : null
}

export async function getDetailsAsync(id){
    const response = await requester.getAsync(baseUrl + apiRoutes.books + `/${id}`)
    return response.ok ? response.json() : null
}