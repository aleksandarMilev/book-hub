import { baseUrl, routes } from '../common/constants/api'
import { errors } from '../common/constants/messages'

export async function getNamesAsync(token){
    const options = {
        method: "GET",
        headers: {
            'Authorization': `Bearer ${token}`
        }
    }

    const url = baseUrl + routes.profileNames
    const response = await fetch(url, options)

    if(response.ok){
        return await response.json()
    }

    throw new Error(errors.profile.names)
}

export async function hasProfileAsync(token) {
    const options = {
        method: "GET",
        headers: {
            'Authorization': `Bearer ${token}`
        }
    }

    const url = baseUrl + routes.hasProfile
    const response = await fetch(url, options)

    if(response.ok){
        return await response.json()
    } 

    throw new Error(errors.profile.get)
}

export async function topThreeAsync() {
    const url = baseUrl + routes.topProfiles
    const response = await fetch(url)

    if(response.ok){
        return await response.json()
    } 

    throw new Error(errors.profile.topThree)
}

export async function mineAsync(token) {
    const options = {
        method: "GET",
        headers: {
            'Authorization': `Bearer ${token}`
        }
    }

    const url = baseUrl + routes.profile
    const response = await fetch(url, options)

    if(response.ok){
        return await response.json()
    } else if (response.status === 404){
        return null
    } 

    throw new Error(errors.profile.get)
}

export async function otherAsync(id, token) {
    const options = {
        method: "GET",
        headers: {
            'Authorization': `Bearer ${token}`
        }
    }

    const url = baseUrl + routes.profile + `/${id}`
    const response = await fetch(url, options)

    if(response.ok){
        return await response.json()
    } 

    throw new Error(errors.profile.getOther)
}

export async function createAsync(profile, token){
    const options = {
        method: "POST",
        headers: {
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(profile)
    }

    const url = baseUrl + routes.profile
    const response = await fetch(url, options)

    if(!response.ok){
        throw new Error(errors.profile.create)
    } 
}

export async function editAsync(profile, token){
    const options = {
        method: "PUT",
        headers: {
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(profile)
    }

    const url = baseUrl + routes.profile
    const response = await fetch(url, options)

    if(!response.ok){
        throw new Error(errors.profile.edit)
    }
}

export async function deleteAsync(token){
    const options = {
        method: "DELETE",
        headers: {
            'Authorization': `Bearer ${token}`
        }
    }

    const url = baseUrl + routes.profile
    const response = await fetch(url, options)

    if(!response.ok){
        throw new Error(errors.profile.delete)
    }
}
