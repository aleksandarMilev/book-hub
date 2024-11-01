import { useState } from "react";

export function useForm(initValues, submitCallback){
    const[values, setValues] = useState(initValues)

    function onChange(e){
        setValues(old => ({
            ...old,
            [e.target.name]: e.target.value
        }))
    }

    function onSubmit(e){
        e.preventDefault()
        submitCallback()
    }

    return{
        values,
        onChange,
        onSubmit
    }
}