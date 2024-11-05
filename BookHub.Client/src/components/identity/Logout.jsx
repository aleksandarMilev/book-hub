import { useContext } from "react"

import { routes } from '../../common/constants/api'
import { UserContext } from "../../contexts/userContext"
import { useNavigate } from 'react-router-dom'

export default function Logout(){
    const { logout } = useContext(UserContext)
    logout()
    
    const navigate = useNavigate()
    navigate(routes.home)
}