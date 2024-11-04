import { baseUrl, routes } from '../common/constants/api'

export async function getAllAsync(signal){
    const response = await fetch(baseUrl + routes.books, signal)
    return response.ok ? await response.json() : null
}

export async function getDetailsAsync(id, signal){
    const response = await fetch(baseUrl + routes.books + `/${id}`, signal)
    return response.ok ? await response.json() : null
}