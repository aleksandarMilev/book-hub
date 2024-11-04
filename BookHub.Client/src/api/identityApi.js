import { baseUrl } from "../common/constants/api"
import { routes } from "../common/constants/api"

export async function registerAsync(username, email, password) {
    const user = {
        username,
        email,
        password
    };

    const options = {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(user)
    };

    const url = baseUrl + routes.register;

    try {
        const response = await fetch(url, options);

        if (!response.ok) {
            const errorData = await response.json(); 
            throw new Error(errorData.errorMessage || 'Registration failed');
        }

        return await response.json(); 
    } catch (error) {
        throw new Error(error.message || 'An unexpected error occurred');
    }
}

export async function loginAsync(username, password) {
    const user = { 
        username,
        password 
    }

    const options =  {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(user)
    } 

    const url = baseUrl + routes.login
    return await fetch(url, options)
}
