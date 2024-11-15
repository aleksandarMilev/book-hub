import { baseUrl, routes } from '../common/constants/api'

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

    throw new Error(`Something went wrong, please try again: ${response.status}`)
}