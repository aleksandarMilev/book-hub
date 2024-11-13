import { useContext, useEffect } from "react"
import { useNavigate } from 'react-router-dom'

import { routes } from '../../../common/constants/api'
import { UserContext } from "../../../contexts/userContext"

export default function Logout(){
    const { logout } = useContext(UserContext)
    const navigate = useNavigate()

    useEffect(() => {
        logout()
        navigate(routes.home)
        
    }, [logout, navigate])

    return null
}