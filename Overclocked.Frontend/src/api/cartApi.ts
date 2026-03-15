const API_URL = "http://localhost:5002/api/cart";

async function getCart()
{
  const token = localStorage.getItem("token");

  const settings = {
    url: `${API_URL}/GetCart`,
    method: "GET",
    headers: {
      "Content-Type": "application/json",
      "Authorization": "Bearer " + token
    }
  };

  const response = await fetch(settings.url, {
    method: settings.method,
    headers: settings.headers
  });

  if (!response.ok)
  {
    throw new Error("Failed to get cart");
  }

  const data = await response.json();
  return data;
}

async function addToCart(productId: number, quantity: number)
{
  const token = localStorage.getItem("token");

  const settings = {
    url: `${API_URL}/AddToCart`,
    method: "POST",
    headers: {
      "Content-Type": "application/json",
      "Authorization": "Bearer " + token
    },
    body: JSON.stringify({ productId, quantity })
  };

  const response = await fetch(settings.url, {
    method: settings.method,
    headers: settings.headers,
    body: settings.body
  });

  if (!response.ok)
  {
    throw new Error("Failed to add to cart");
  }

  const data = await response.json();
  return data;
}

async function removeFromCart(cartItemId: number)
{
  const token = localStorage.getItem("token");

  const settings = {
    url: `${API_URL}/RemoveFromCart/${cartItemId}`,
    method: "DELETE",
    headers: {
      "Content-Type": "application/json",
      "Authorization": "Bearer " + token
    }
  };

  const response = await fetch(settings.url, {
    method: settings.method,
    headers: settings.headers
  });

  if (!response.ok)
  {
    throw new Error("Failed to remove from cart");
  }

  const data = await response.json();
  return data;
}

export { getCart, addToCart, removeFromCart };