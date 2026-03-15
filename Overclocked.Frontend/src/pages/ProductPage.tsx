import { useEffect, useState } from "react";
import { useParams, Link, useNavigate } from "react-router-dom";
import type { Product } from "../types/Product";
import { getProductById } from "../api/productApi";
import { addToCart } from "../api/cartApi";

function ProductPage()
{

  const { id } = useParams<{ id: string }>();
  const [product, setProduct] = useState<Product | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");
  const [cartMessage, setCartMessage] = useState("");

  const navigate = useNavigate();
  const token = localStorage.getItem("token");

  useEffect(function ()
  {
    if (!id) return;

    const productId = Number(id);

    getProductById(productId)
      .then(function (data)
      {
        setProduct(data);
        setLoading(false);
      })
      .catch(function (err)
      {
        setError(err.message);
        setLoading(false);
      });
  }, [id]);

  function handleAddToCart()
  {
    if (token == null)
    {
      navigate("/login");
      return;
    }

    addToCart(Number(id), 1)
      .then(function ()
      {
        setCartMessage("Item added to cart!");
        setTimeout(function () { setCartMessage(""); }, 3000);
      })
      .catch(function (err)
      {
        setCartMessage(err.message);
      });
  }

  if (loading)
  {
    return (
      <div className="text-center mt-5">
        <div className="spinner-border text-warning" role="status"></div>
      </div>
    );
  }

  if (error != "")
  {
    return (
      <div className="container mt-4">
        <div className="alert alert-danger">Error: {error}</div>
      </div>
    );
  }

  if (product === null)
  {
    return (
      <div className="container mt-4">
        <div className="alert alert-warning">Product not found.</div>
      </div>
    );
  }

  let stockMessage = "";
  if (product.qty > 0)
  {
    stockMessage = product.qty + " available";
  } else
  {
    stockMessage = "Out of Stock";
  }

  return (
    <div className="container py-4">
      <div className="mb-3">
        <Link to="/" className="text-warning text-decoration-none" style={{ fontSize: "0.85rem" }}>
          Home
        </Link>
        <span className="text-secondary mx-2" style={{ fontSize: "0.85rem" }}>/</span>
        {product.category != null && (
          <span className="text-secondary" style={{ fontSize: "0.85rem" }}>{product.category}</span>
        )}
        {product.category != null && (
          <span className="text-secondary mx-2" style={{ fontSize: "0.85rem" }}>/</span>
        )}
        <span className="text-light" style={{ fontSize: "0.85rem" }}>{product.name}</span>
      </div>

      <div className="row g-4">
        <div className="col-md-5">
          <div className="rounded-2 overflow-hidden"
            style={{ backgroundColor: "#1e1e2e", border: "1px solid #2a2a3e" }}>
            <img
              src={product.image ?? "https://placehold.co/500x400?text=No+Image"}
              alt={product.name}
              className="w-100"
              style={{ height: "320px", objectFit: "cover", display: "block" }}
            />
          </div>
        </div>

        <div className="col-md-7">
          {product.category != null && (
            <span className="badge bg-warning text-dark mb-2" style={{ fontSize: "0.75rem" }}>
              {product.category}
            </span>
          )}

          <h4 className="text-light fw-bold mb-1">{product.name}</h4>
          <p className="text-secondary mb-3" style={{ fontSize: "0.85rem" }}>SKU: {product.sku}</p>

          <hr style={{ borderColor: "#2a2a3e" }} />

          <div className="mb-3">
            <p className="text-secondary mb-1" style={{ fontSize: "0.8rem" }}>Price</p>
            <p className="text-warning fw-bold mb-0" style={{ fontSize: "1.6rem" }}>
              {"R" + product.price.toFixed(2)}
            </p>
          </div>

          <div className="mb-3">
            <p className="text-secondary mb-1" style={{ fontSize: "0.8rem" }}>Stock</p>
            {product.qty > 0 ? (
              <div>
                <span className="badge bg-success me-2">In Stock</span>
                <span className="text-light" style={{ fontSize: "0.85rem" }}>{stockMessage}</span>
              </div>
            ) : (
              <span className="badge bg-danger">{stockMessage}</span>
            )}
          </div>

          <hr style={{ borderColor: "#2a2a3e" }} />

          {cartMessage != "" && (
            <div className="alert alert-success py-2 mb-3">{cartMessage}</div>
          )}

          <div className="d-flex gap-2 mt-3">
            <button
              className="btn btn-warning btn-sm text-dark fw-bold px-4"
              onClick={handleAddToCart}
              disabled={product.qty === 0}
            >
              Add to Cart
            </button>
            <Link to="/" className="btn btn-outline-light btn-sm">
              Back to Products
            </Link>
          </div>
        </div>
      </div>
    </div>
  );
}

export default ProductPage;