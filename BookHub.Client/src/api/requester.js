import { httpCodes } from "../common/constants"

async function requester(url, data, method = httpCodes.get){
    const options = { 
        method
    }

    if(data){
        options.headers = {
            'Content-Type': 'application/json'
        }

        options.body = JSON.stringify(data)
    }

    const response = await fetch(url, options)
    return await response.json()
} 

export const getAsync = requester.bind(null)
export const postAsync = requester.bind(null, httpCodes.post)
export const putAsync = requester.bind(null, httpCodes.put)
export const deleteAsync = requester.bind(null, httpCodes.delete)