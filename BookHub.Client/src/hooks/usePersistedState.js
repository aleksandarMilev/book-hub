import { useState } from 'react'

export default function usePersistedState(localStorageKey, initState){
    const[state, setState] = useState(initState)

    function setPersistedState(value){
        localStorage.setItem(localStorageKey, JSON.stringify(value))
        setState(value)
    }

    return [state, setPersistedState]
}
