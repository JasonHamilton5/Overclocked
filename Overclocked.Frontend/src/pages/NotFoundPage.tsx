import { Link } from "react-router-dom";

function NotFoundPage()
{
  return (
    <div className="container text-center my-5">
      <h1 className="display-1 fw-bold text-warning">404</h1>
      <h2 className="mb-3">Page Not Found</h2>
      <p className="text-muted mb-4">
        The page you are looking for does not exist or has been moved.
      </p>
      <Link to="/" className="btn btn-outline-light">
        ← Back to Home
      </Link>
    </div>
  );
}

export default NotFoundPage;