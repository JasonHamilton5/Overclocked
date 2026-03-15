import { Link, useNavigate } from "react-router-dom";

function Header()
{

  const navigate = useNavigate();
  const username = localStorage.getItem("username");

  function handleLogout()
  {
    localStorage.removeItem("token");
    localStorage.removeItem("username");
    navigate("/login");
  }

  return (
    <nav className="navbar navbar-dark px-4 py-2" style={{ backgroundColor: "#1e1e2e", borderBottom: "1px solid #2a2a3e" }}>
      <Link className="navbar-brand fw-bold text-warning" to="/">
        Overclocked
      </Link>
      <div className="d-flex gap-3 align-items-center">
        <Link
          className="text-decoration-none text-secondary"
          style={{ fontSize: "0.9rem" }}
          to="/"
        >
          Home
        </Link>
        <Link
          className="text-decoration-none text-secondary"
          style={{ fontSize: "0.9rem" }}
          to="/products"
        >
          All Products
        </Link>

        {username != null ? (
          <div className="d-flex gap-3 align-items-center">
            <Link
              className="text-decoration-none text-secondary"
              style={{ fontSize: "0.9rem" }}
              to="/cart"
            >
              Cart
            </Link>
            <span className="text-light" style={{ fontSize: "0.9rem" }}>
              {username}
            </span>
            <button
              className="btn btn-outline-warning btn-sm"
              onClick={handleLogout}
            >
              Logout
            </button>
          </div>
        ) : (
          <div className="d-flex gap-2">
            <Link to="/login" className="btn btn-outline-secondary btn-sm">
              Login
            </Link>
            <Link to="/register" className="btn btn-warning btn-sm text-dark fw-bold">
              Register
            </Link>
          </div>
        )}
      </div>
    </nav>
  );
}

export default Header;