import { baseUrl } from "../common/constants"
import { apiRoutes } from "../common/constants"

export async function register({ username, email, password }) {
    const user = { 
        username,
        email,
        password 
    }

    const options =  {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(user)
    } 

    const url = baseUrl + apiRoutes.register
    const _ = await fetch(url, options)
}

export async function login({ username, password }) {
    const user = { 
        username,
        password 
    }

    const options =  {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(user)
    } 

    const url = baseUrl + apiRoutes.login
    const token = await fetch(url, options)
    localStorage.setItem('token', token)
}
