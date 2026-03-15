import { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import { login } from "../api/authApi";

function LoginPage()
{

  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");
  const [loading, setLoading] = useState(false);

  const navigate = useNavigate();

  function handleLogin()
  {
    setError("");
    setLoading(true);

    login(email, password)
      .then(function (data)
      {
        localStorage.setItem("token", data.token);
        localStorage.setItem("username", data.username);
        setLoading(false);
        navigate("/");
      })
      .catch(function (err)
      {
        setError(err.message);
        setLoading(false);
      });
  }

  return (
    <div className="container mt-5">
      <div className="row justify-content-center">
        <div className="col-md-4">
          <div className="p-4 rounded-2" style={{ backgroundColor: "#1e1e2e", border: "1px solid #2a2a3e" }}>
            <h5 className="text-light mb-4">Login</h5>

            {error != "" && (
              <div className="alert alert-danger py-2">{error}</div>
            )}

            <div className="mb-3">
              <label className="text-secondary mb-1" style={{ fontSize: "0.85rem" }}>Email</label>
              <input
                type="email"
                className="form-control form-control-sm text-light"
                style={{ backgroundColor: "#13131a", border: "1px solid #3a3a5e" }}
                value={email}
                onChange={function (e) { setEmail(e.target.value); }}
              />
            </div>

            <div className="mb-4">
              <label className="text-secondary mb-1" style={{ fontSize: "0.85rem" }}>Password</label>
              <input
                type="password"
                className="form-control form-control-sm text-light"
                style={{ backgroundColor: "#13131a", border: "1px solid #3a3a5e" }}
                value={password}
                onChange={function (e) { setPassword(e.target.value); }}
              />
            </div>

            <button
              className="btn btn-warning btn-sm w-100 text-dark fw-bold"
              onClick={handleLogin}
              disabled={loading}
            >
              {loading ? "Logging in..." : "Login"}
            </button>

            <p className="text-secondary text-center mt-3 mb-0" style={{ fontSize: "0.85rem" }}>
              Don't have an account?{" "}
              <Link to="/register" className="text-warning">Register</Link>
            </p>
          </div>
        </div>
      </div>
    </div>
  );
}

export default LoginPage;