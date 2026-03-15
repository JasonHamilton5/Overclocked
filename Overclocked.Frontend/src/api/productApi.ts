import type { Product } from "../types/Product";

const API_URL = "http://localhost:5001/api/products";

async function getAllProducts(): Promise<Product[]>
{
  const settings = {
    url: `${API_URL}/GetProducts`,
    method: "GET",
    headers: {
      "Content-Type": "application/json"
    }
  };

  const response = await fetch(settings.url, {
    method: settings.method,
    headers: settings.headers
  });

  if (!response.ok)
  {
    throw new Error("Failed to fetch products");
  }

  const data = await response.json();
  return data;
}

async function getProductById(id: number): Promise<Product>
{
  const settings = {
    url: `${API_URL}/GetProducts/${id}`,
    method: "GET",
    headers: {
      "Content-Type": "application/json"
    }
  };

  const response = await fetch(settings.url, {
    method: settings.method,
    headers: settings.headers
  });

  if (!response.ok)
  {
    throw new Error("Product not found");
  }

  const data = await response.json();
  return data;
}

async function getFeaturedProducts(): Promise<Product[]>
{
  const settings = {
    url: `${API_URL}/GetFeaturedProducts`,
    method: "GET",
    headers: {
      "Content-Type": "application/json"
    }
  };

  const response = await fetch(settings.url, {
    method: settings.method,
    headers: settings.headers
  });

  if (!response.ok)
  {
    throw new Error("Failed to fetch featured products");
  }

  const data = await response.json();
  return data;
}

export { getAllProducts, getProductById, getFeaturedProducts };