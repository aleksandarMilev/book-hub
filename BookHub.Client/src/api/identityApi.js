import { baseUrl } from "../common/constants/api"
import { routes } from "../common/constants/api"
import { errors } from "../common/constants/messages"

export async function registerAsync(username, email, password) {
    const user = {
        username,
        email,
        password
    }

    const options = {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(user)
    }

    const url = baseUrl + routes.register

    const response = await fetch(url, options)
    
    if (response.ok) {
        return await response.json()
    }

    const errorData = await response.json()
    throw new Error(errorData?.errorMessage || errors.identity.register)
}

export async function loginAsync(username, password) {
    const user = {
        username,
        password
    }

    const options = {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(user)
    }

    const url = baseUrl + routes.login

    const response = await fetch(url, options)
    
    if (response.ok) {
        return await response.json()
    }

    const errorData = await response.json()
    throw new Error(errorData?.errorMessage || errors.identity.login)
}
