let baseUrl = 'https://localhost:7216';

export async function loginAsync(username, password){
    let credentials = {
        username,
        password
    }

    let options = {
        Mehtod: "POST",  
        Headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify(credentials)     
    }

    let response = await fetch(`${baseUrl}/login`, options)
    return response.json();
}

export async function loginAsync(username, password){
    let credentials = {
        username,
        password
    }

    let options = {
        Mehtod: "POST",  
        Headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify(credentials)     
    }

    let response = await fetch(`${baseUrl}/register`, options)
    return response.json();
}
