import { baseUrl, routes } from '../common/constants/api'
import { errors } from '../common/constants/messages'
import { pagination } from '../common/constants/defaultValues'

export async function searchBooksAsync(
    token,
    searchTerm,
    page = pagination.defaultPageIndex,
    pageSize = pagination.defaultPageSize) {

    const options = {
        method: "GET",
        headers: {
            'Authorization': `Bearer ${token}`
        }
    }

    const url = `${baseUrl}${routes.searchBooks}?searchTerm=${encodeURIComponent(searchTerm)}&page=${page}&pageSize=${pageSize}`
    const response = await fetch(url, options)

    if(response.ok){
        return await response.json()
    }

    throw new Error(errors.search.badRequest)
}