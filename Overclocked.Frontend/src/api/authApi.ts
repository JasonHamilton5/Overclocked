const API_URL = "http://localhost:5002/api/auth";

async function register(username: string, email: string, password: string)
{
  const settings = {
    url: `${API_URL}/Register`,
    method: "POST",
    headers: {
      "Content-Type": "application/json"
    },
    body: JSON.stringify({ username, email, password })
  };

  const response = await fetch(settings.url, {
    method: settings.method,
    headers: settings.headers,
    body: settings.body
  });

  const data = await response.json();

  if (!response.ok)
  {
    throw new Error(data.message || "Registration failed");
  }

  return data;
}

async function login(email: string, password: string)
{
  const settings = {
    url: `${API_URL}/Login`,
    method: "POST",
    headers: {
      "Content-Type": "application/json"
    },
    body: JSON.stringify({ email, password })
  };

  const response = await fetch(settings.url, {
    method: settings.method,
    headers: settings.headers,
    body: settings.body
  });

  const data = await response.json();

  if (!response.ok)
  {
    throw new Error(data.message || "Login failed");
  }

  return data;
}

export { register, login };