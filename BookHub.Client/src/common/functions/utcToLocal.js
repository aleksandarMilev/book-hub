export const utcToLocal = (utcDateStr) => {
    const isoDateStr = utcDateStr.replace(' ', 'T') + 'Z'

    const utcDate = new Date(isoDateStr)

    const options = {
        weekday: 'short',  
        year: 'numeric',  
        month: 'short',
        day: 'numeric',   
        hour: '2-digit',  
        minute: '2-digit', 
        hour12: false 
    };

    const localDate = utcDate.toLocaleString('en-GB', options)

    return localDate
}