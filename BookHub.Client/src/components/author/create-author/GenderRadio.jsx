import { MDBRadio } from 'mdb-react-ui-kit'

export default function GenderRadio({formik}){
    return(
        <div className="d-md-flex justify-content-start align-items-center mb-4">
            <h6 className="fw-bold mb-0 me-4">Gender: *</h6>
            <MDBRadio
                name="gender"
                id="genderMale"
                value="male"
                label="Male"
                inline
                onChange={formik.handleChange}
                checked={formik.values.gender === 'male'}
            />
            <MDBRadio
                name="gender"
                id="genderFemale"
                value="female"
                label="Female"
                inline
                onChange={formik.handleChange}
                checked={formik.values.gender === 'female'}
            />
            <MDBRadio
                name="gender"
                id="genderOther"
                value="other"
                label="Other"
                inline
                onChange={formik.handleChange}
                checked={formik.values.gender === 'other'}
            />
            {formik.touched.gender && formik.errors.gender && (
                <div className="text-danger">{formik.errors.gender}</div>
            )}
        </div>
    )
}