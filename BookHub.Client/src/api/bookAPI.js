import { baseUrl, apiRoutes } from '../common/constants'

export async function getAllAsync(signal){
    const response = await fetch(baseUrl + apiRoutes.books, signal)
    return response.ok ? await response.json() : null
}

export async function getDetailsAsync(id, signal){
    const response = await fetch(baseUrl + apiRoutes.books + `/${id}`, signal)
    return response.ok ? await response.json() : null
}