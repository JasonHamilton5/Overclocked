import { useEffect, useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import { getCart, removeFromCart } from "../api/cartApi";

interface CartItem
{
  cartItemId: number;
  productId: number;
  quantity: number;
  productName: string;
  productSku: string;
  productPrice: number;
  productImage: string | null;
}

interface Cart
{
  cartId: number;
  items: CartItem[];
}

function CartPage()
{

  const [cart, setCart] = useState<Cart | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  const navigate = useNavigate();
  const token = localStorage.getItem("token");

  useEffect(function ()
  {
    if (token == null)
    {
      navigate("/login");
      return;
    }

    getCart()
      .then(function (data)
      {
        setCart(data);
        setLoading(false);
      })
      .catch(function (err)
      {
        setError(err.message);
        setLoading(false);
      });
  }, []);

  function handleRemove(cartItemId: number)
  {
    removeFromCart(cartItemId)
      .then(function ()
      {
        return getCart();
      })
      .then(function (data)
      {
        setCart(data);
      })
      .catch(function (err)
      {
        setError(err.message);
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

  const items = cart?.items || [];

  let total = 0;
  for (let i = 0; i < items.length; i++)
  {
    total += items[i].productPrice * items[i].quantity;
  }

  return (
    <div className="container py-4">
      <h5 className="text-light mb-4">Your Cart</h5>

      {items.length === 0 ? (
        <div className="text-center mt-5">
          <p className="text-secondary">Your cart is empty.</p>
          <Link to="/products" className="btn btn-outline-warning btn-sm mt-2">
            Browse Products
          </Link>
        </div>
      ) : (
        <div className="row g-4">
          <div className="col-md-8">
            {items.map(function (item)
            {
              return (
                <div key={item.cartItemId} className="d-flex align-items-center gap-3 p-3 mb-2 rounded-2"
                  style={{ backgroundColor: "#1e1e2e", border: "1px solid #2a2a3e" }}>
                  <img
                    src={item.productImage ?? "https://placehold.co/80x80?text=No+Image"}
                    alt={item.productName}
                    style={{ width: "80px", height: "80px", objectFit: "cover", borderRadius: "6px" }}
                  />
                  <div className="flex-grow-1">
                    <p className="text-light fw-semibold mb-1" style={{ fontSize: "0.9rem" }}>
                      {item.productName}
                    </p>
                    <p className="text-secondary mb-1" style={{ fontSize: "0.8rem" }}>
                      SKU: {item.productSku}
                    </p>
                    <p className="text-warning mb-0" style={{ fontSize: "0.85rem" }}>
                      {"R" + item.productPrice.toFixed(2)} x {item.quantity}
                    </p>
                  </div>
                  <div className="text-end">
                    <p className="text-warning fw-bold mb-2" style={{ fontSize: "0.95rem" }}>
                      {"R" + (item.productPrice * item.quantity).toFixed(2)}
                    </p>
                    <button
                      className="btn btn-outline-danger btn-sm"
                      onClick={function () { handleRemove(item.cartItemId); }}
                    >
                      Remove
                    </button>
                  </div>
                </div>
              );
            })}
          </div>

          <div className="col-md-4">
            <div className="p-3 rounded-2" style={{ backgroundColor: "#1e1e2e", border: "1px solid #2a2a3e" }}>
              <h6 className="text-light mb-3">Order Summary</h6>
              <hr style={{ borderColor: "#2a2a3e" }} />
              <div className="d-flex justify-content-between mb-2">
                <span className="text-secondary" style={{ fontSize: "0.85rem" }}>Items ({items.length})</span>
                <span className="text-light" style={{ fontSize: "0.85rem" }}>{"R" + total.toFixed(2)}</span>
              </div>
              <hr style={{ borderColor: "#2a2a3e" }} />
              <div className="d-flex justify-content-between mb-3">
                <span className="text-light fw-bold">Total</span>
                <span className="text-warning fw-bold">{"R" + total.toFixed(2)}</span>
              </div>
              <button className="btn btn-warning btn-sm w-100 text-dark fw-bold">
                Checkout
              </button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}

export default CartPage;