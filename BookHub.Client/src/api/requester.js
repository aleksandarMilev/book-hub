import { httpCodes } from "../common/constants";

async function requester(url, signal = null, data = null, method = httpCodes.get) {
    const options = { 
        method
    };

    if(signal){
        options.signal = signal
    }

    if (data) {
        options.headers = {
            'Content-Type': 'application/json'
        }

        options.body = JSON.stringify(data)
    }

    const response = await fetch(url, options)
    return { response, controller } 
} 

export const getAsync = (url, signal) => requester(url, signal, null, httpCodes.get)
export const postAsync = (url, data, signal) => requester(url, signal, data, httpCodes.post)
export const putAsync = (url, data, signal) => requester(url, signal, data, httpCodes.put)
export const deleteAsync = (url, signal) => requester(url, signal, null, httpCodes.delete)
