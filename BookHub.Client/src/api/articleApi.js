import { baseUrl, baseAdminUrl, routes } from '../common/constants/api'
import { errors } from '../common/constants/messages'

export async function detailsAsync(id, token) {
    const options = {
        method: "GET",
        headers: {
            'Authorization': `Bearer ${token}`
        }
    }

    const url = baseUrl + routes.article + `/${id}`
    const response = await fetch(url, options)

    if(response.ok){
        return await response.json()
    } 

    throw new Error(errors.article.get)
}

export async function createAsync(article, token){
    const options = {
        method: "POST",
        headers: {
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(article)
    }

    const url = baseAdminUrl + routes.article
    const response = await fetch(url, options)

    if(!response.ok){
        throw new Error(errors.article.create)
    } 
}

export async function editAsync(id, article, token){
    const options = {
        method: "PUT",
        headers: {
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(article)
    }

    const url = baseAdminUrl + routes.article + `/${id}`
    const response = await fetch(url, options)

    if(!response.ok){
        throw new Error(errors.article.edit)
    }
}

export async function deleteAsync(id, token){
    const options = {
        method: "DELETE",
        headers: {
            'Authorization': `Bearer ${token}`
        }
    }

    const url = baseAdminUrl + routes.article + `/${id}`
    const response = await fetch(url, options)

    if(!response.ok){
        throw new Error(errors.article.delete)
    }
}
