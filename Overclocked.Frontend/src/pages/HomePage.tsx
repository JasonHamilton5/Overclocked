import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import type { Product } from "../types/Product";
import { getFeaturedProducts } from "../api/productApi";

function HomePage()
{

  const [products, setProducts] = useState<Product[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  useEffect(function ()
  {
    getFeaturedProducts()
      .then(function (data)
      {
        setProducts(data);
        setLoading(false);
      })
      .catch(function (err)
      {
        setError(err.message);
        setLoading(false);
      });
  }, []);

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

  return (
    <div className="container-fluid px-4 py-4">
      <h5 className="text-light mb-4">Featured Products</h5>

      {products.length === 0 ? (
        <div className="text-center mt-5">
          <p className="text-secondary">No featured products available.</p>
        </div>
      ) : (
        <div className="row row-cols-2 row-cols-sm-3 row-cols-md-4 row-cols-lg-5 g-3">
          {products.map(function (product)
          {
            return (
              <div className="col" key={product.productId}>
                <Link to={"/product/" + product.productId} className="text-decoration-none">
                  <div
                    className="h-100 rounded-2 overflow-hidden"
                    style={{
                      backgroundColor: "#1e1e2e",
                      border: "1px solid #2a2a3e",
                      transition: "border-color 0.2s, transform 0.2s"
                    }}
                    onMouseEnter={function (e)
                    {
                      (e.currentTarget as HTMLDivElement).style.borderColor = "#f5a623";
                      (e.currentTarget as HTMLDivElement).style.transform = "translateY(-2px)";
                    }}
                    onMouseLeave={function (e)
                    {
                      (e.currentTarget as HTMLDivElement).style.borderColor = "#2a2a3e";
                      (e.currentTarget as HTMLDivElement).style.transform = "translateY(0)";
                    }}
                  >
                    <img
                      src={product.image ?? "https://placehold.co/400x300?text=No+Image"}
                      alt={product.name}
                      style={{ width: "100%", height: "160px", objectFit: "cover", display: "block" }}
                    />
                    <div className="p-2">
                      <p className="text-warning mb-1" style={{ fontSize: "0.7rem" }}>{product.category}</p>
                      <p className="text-light mb-1 fw-semibold" style={{ fontSize: "0.82rem", lineHeight: "1.3" }}>
                        {product.name}
                      </p>
                      <p className="text-secondary mb-1" style={{ fontSize: "0.72rem" }}>{product.sku}</p>
                      <p className="text-warning fw-bold mb-1" style={{ fontSize: "0.95rem" }}>
                        {"R" + product.price.toFixed(2)}
                      </p>
                      <p className="text-secondary mb-0" style={{ fontSize: "0.7rem" }}>
                        {product.qty > 0 ? product.qty + " in stock" : "Out of stock"}
                      </p>
                    </div>
                  </div>
                </Link>
              </div>
            );
          })}
        </div>
      )}
    </div>
  );
}

export default HomePage;