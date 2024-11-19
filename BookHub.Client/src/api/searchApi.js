import { baseUrl, routes } from '../common/constants/api'
import { errors } from '../common/constants/messages'

export async function searchBooksAsync(searchTerm, token){
    const options = {
        method: "GET",
        headers: {
            'Authorization': `Bearer ${token}`
        }
    }

    const url = `${baseUrl}${routes.searchBooks}?searchTerm=${encodeURIComponent(searchTerm)}`
    const response = await fetch(url, options)

    if(response.ok){
        return await response.json()
    }

    throw new Error(errors.search.badRequest)
}