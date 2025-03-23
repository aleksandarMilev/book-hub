import { Link, useLocation } from "react-router-dom";

import { routes } from "../../../common/constants/api";

import image from "../../../assets/images/forbidden.png";

export default function AccessDenied() {
  const location = useLocation();
  const message =
    location.state?.message || "Sorry, you can not access this resource.";

  return (
    <div className="d-flex align-items-center justify-content-center vh-100">
      <div className="text-center">
        <img
          src={image}
          alt="Not found"
          className="img-fluid mb-4"
          style={{ maxWidth: "300px" }}
        />
        <p className="fs-3 text-danger mb-3">Oops!</p>
        <p className="lead">{message}</p>
        <Link to={routes.home} className="btn btn-primary">
          Go Home
        </Link>
      </div>
    </div>
  );
}
