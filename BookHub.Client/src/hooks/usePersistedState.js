import { useState } from "react";

export default function usePersistedState(key, initState){
    const[state, setState] = useState(initState)

    function setPersistedState(value){
        localStorage.setItem(key, JSON.stringify(value))
        setState(value)
    }

    return [state, setPersistedState]
}