import './ProfileDetails.css'

export default function ProfileDetails() {
    return (
        <div className="profile-details container">
            <div className="main-body">
                <div className="row">
                    <div className="col-lg-4">
                        <div className="card">
                            <div className="card-body">
                                <div className="d-flex flex-column align-items-center text-center">
                                    <img
                                        src="https://bootdey.com/img/Content/avatar/avatar6.png"
                                        alt="Admin"
                                        className="rounded-circle p-1 bg-primary"
                                        width="110"
                                    />
                                    <div className="mt-3">
                                        <h4>John Doe</h4>
                                        <p className="text-secondary mb-1">Full Stack Developer</p>
                                        <p className="text-muted font-size-sm">Bay Area, San Francisco, CA</p>
                                        <button className="btn btn-primary">Follow</button>
                                        <button className="btn btn-outline-primary">Message</button>
                                    </div>
                                </div>
                                <hr className="my-4" />
                                <ul className="list-group list-group-flush">
                                    <li className="list-group-item d-flex justify-content-between align-items-center flex-wrap">
                                        <h6 className="mb-0">
                                            <svg
                                                xmlns="http://www.w3.org/2000/svg"
                                                width="24"
                                                height="24"
                                                viewBox="0 0 24 24"
                                                fill="none"
                                                stroke="currentColor"
                                                strokeWidth="2"
                                                strokeLinecap="round"
                                                strokeLinejoin="round"
                                                className="feather feather-globe me-2 icon-inline"
                                            >
                                                <circle cx="12" cy="12" r="10"></circle>
                                                <line x1="2" y1="12" x2="22" y2="12"></line>
                                                <path d="M12 2a15.3 15.3 0 0 1 4 10 15.3 15.3 0 0 1-4 10 15.3 15.3 0 0 1-4-10 15.3 15.3 0 0 1 4-10z"></path>
                                            </svg>
                                            Website
                                        </h6>
                                        <span className="text-secondary">https://bootdey.com</span>
                                    </li>
                                    <li className="list-group-item d-flex justify-content-between align-items-center flex-wrap">
                                        <h6 className="mb-0">
                                            <svg
                                                xmlns="http://www.w3.org/2000/svg"
                                                width="24"
                                                height="24"
                                                viewBox="0 0 24 24"
                                                fill="none"
                                                stroke="currentColor"
                                                strokeWidth="2"
                                                strokeLinecap="round"
                                                strokeLinejoin="round"
                                                className="feather feather-github me-2 icon-inline"
                                            >
                                                <path d="M9 19c-5 1.5-5-2.5-7-3m14 6v-3.87a3.37 3.37 0 0 0-.94-2.61c3.14-.35 6.44-1.54 6.44-7A5.44 5.44 0 0 0 20 4.77 5.07 5.07 0 0 0 19.91 1S18.73.65 16 2.48a13.38 13.38 0 0 0-7 0C6.27.65 5.09 1 5.09 1A5.07 5.07 0 0 0 5 4.77a5.44 5.44 0 0 0-1.5 3.78c0 5.42 3.3 6.61 6.44 7A3.37 3.37 0 0 0 9 18.13V22"></path>
                                            </svg>
                                            Github
                                        </h6>
                                        <span className="text-secondary">bootdey</span>
                                    </li>
                                </ul>
                            </div>
                        </div>
                    </div>
                    <div className="col-lg-8">
                        <div className="card">
                            <div className="card-body">
                                <div className="row mb-3">
                                    <div className="col-sm-3">
                                        <h6 className="mb-0">Full Name</h6>
                                    </div>
                                    <div className="col-sm-9 text-secondary">
                                        <input type="text" className="form-control" defaultValue="John Doe" />
                                    </div>
                                </div>
                                <div className="row">
                                    <div className="col-sm-3"></div>
                                    <div className="col-sm-9 text-secondary">
                                        <input type="button" className="btn btn-primary px-4" value="Save Changes" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div className="row">
                            <div className="col-sm-12">
                                <div className="card">
                                    <div className="card-body">
                                        <h5 className="d-flex align-items-center mb-3">Project Status</h5>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    )
}
