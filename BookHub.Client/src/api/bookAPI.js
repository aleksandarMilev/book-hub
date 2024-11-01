import { baseUrl, apiRoutes } from '../common/constants'

export async function getAllAsync(signal = null){
    const response = await fetch(baseUrl + apiRoutes.books, signal)
    const data = response.ok ? await response.json() : null
    return data
}

export async function getDetailsAsync(id, signal = null){
    const response = await fetch(baseUrl + apiRoutes.books + `/${id}`, signal)
    const data = response.ok ? await response.json() : null
    return data
}