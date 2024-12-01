import * as useProfile from '../../../hooks/useProfile'

import ProfileForm from '../profile-form/ProfileForm'
import DefaultSpinner from '../../common/default-spinner/DefaultSpinner'

export default function EditProfile(){
    const { profile } = useProfile.useMineProfile()

    return profile 
        ? <ProfileForm profile={profile} isEditMode={true} /> 
        : <DefaultSpinner/ >
}