using Overclocked.Products.API.Models;

namespace Overclocked.Products.API.Data
{
    public static class PopulateProducts
    {
        public static void Seed(AppDbContext context)
        {
            if (context.Products.Any()) return;

            var products = new List<Product>
            {
                new Product { Name = "Gaming Console Pro", SKU = "CON-001", Price = 12999.99m, Qty = 20, Category = "Consoles", IsFeatured = true, Image = "https://images.unsplash.com/photo-1606144042614-b2417e99c4e3?w=400&h=300&fit=crop" },
                new Product { Name = "Gaming Console Lite", SKU = "CON-002", Price = 6999.99m, Qty = 25, Category = "Consoles", IsFeatured = false, Image = "https://images.unsplash.com/photo-1578303512597-81e6cc155b3e?w=400&h=300&fit=crop" },
                new Product { Name = "Wireless Gaming Controller", SKU = "CTR-001", Price = 1299.99m, Qty = 40, Category = "Controllers", IsFeatured = true, Image = "https://images.unsplash.com/photo-1592840062661-a5a7f78e2056?w=400&h=300&fit=crop" },
                new Product { Name = "Wired Gaming Controller", SKU = "CTR-002", Price = 899.99m, Qty = 35, Category = "Controllers", IsFeatured = false, Image = "https://images.unsplash.com/photo-1600080972464-8e5f35f63d08?w=400&h=300&fit=crop" },
                new Product { Name = "Pro Gaming Headset", SKU = "HDS-001", Price = 2499.99m, Qty = 30, Category = "Headsets", IsFeatured = true, Image = "https://images.unsplash.com/photo-1599669454699-248893623440?w=400&h=300&fit=crop" },
                new Product { Name = "Wireless Gaming Headset", SKU = "HDS-002", Price = 3499.99m, Qty = 20, Category = "Headsets", IsFeatured = false, Image = "https://images.unsplash.com/photo-1612444530582-fc66183b16f7?w=400&h=300&fit=crop" },
                new Product { Name = "RGB Gaming Mouse", SKU = "MSE-001", Price = 799.99m, Qty = 50, Category = "Mice", IsFeatured = false, Image = "https://images.unsplash.com/photo-1527864550417-7fd91fc51a46?w=400&h=300&fit=crop" },
                new Product { Name = "Wireless Gaming Mouse", SKU = "MSE-002", Price = 1499.99m, Qty = 40, Category = "Mice", IsFeatured = true, Image = "https://images.unsplash.com/photo-1563297007-0686b7003af7?w=400&h=300&fit=crop" },
                new Product { Name = "Mechanical Gaming Keyboard", SKU = "KBD-001", Price = 1999.99m, Qty = 35, Category = "Keyboards", IsFeatured = true, Image = "https://images.unsplash.com/photo-1587829741301-dc798b83add3?w=400&h=300&fit=crop" },
                new Product { Name = "TKL Mechanical Keyboard", SKU = "KBD-002", Price = 1499.99m, Qty = 30, Category = "Keyboards", IsFeatured = false, Image = "https://images.unsplash.com/photo-1618384887929-16ec33fab9ef?w=400&h=300&fit=crop" },
                new Product { Name = "27 Inch Gaming Monitor 144Hz", SKU = "MON-001", Price = 4999.99m, Qty = 15, Category = "Monitors", IsFeatured = true, Image = "https://images.unsplash.com/photo-1547119957-637f8679db1e?w=400&h=300&fit=crop" },
                new Product { Name = "32 Inch Curved Gaming Monitor", SKU = "MON-002", Price = 7999.99m, Qty = 8, Category = "Monitors", IsFeatured = false, Image = "https://images.unsplash.com/photo-1585792180666-f7347c490ee2?w=400&h=300&fit=crop" },
                new Product { Name = "Racing Gaming Chair", SKU = "CHR-001", Price = 3999.99m, Qty = 12, Category = "Chairs", IsFeatured = false, Image = "https://images.unsplash.com/photo-1598550476439-6847785fcea6?w=400&h=300&fit=crop" },
                new Product { Name = "Pro Gaming Chair", SKU = "CHR-002", Price = 6999.99m, Qty = 7, Category = "Chairs", IsFeatured = true, Image = "https://images.unsplash.com/photo-1598550476439-6847785fcea6?w=400&h=300&fit=crop" },
                new Product { Name = "Gaming Desk", SKU = "DSK-001", Price = 4499.99m, Qty = 10, Category = "Furniture", IsFeatured = false, Image = "https://images.unsplash.com/photo-1518455027359-f3f8164ba6bd?w=400&h=300&fit=crop" },
                new Product { Name = "XL Gaming Mousepad", SKU = "ACC-001", Price = 299.99m, Qty = 80, Category = "Accessories", IsFeatured = false, Image = "https://images.unsplash.com/photo-1616588589676-62b3bd4ff6d2?w=400&h=300&fit=crop" },
                new Product { Name = "Gaming Webcam 1080p", SKU = "CAM-001", Price = 1299.99m, Qty = 25, Category = "Accessories", IsFeatured = false, Image = "https://images.unsplash.com/photo-1587826080692-f439cd0b70da?w=400&h=300&fit=crop" },
                new Product { Name = "USB Gaming Microphone", SKU = "MIC-001", Price = 1499.99m, Qty = 20, Category = "Accessories", IsFeatured = false, Image = "https://images.unsplash.com/photo-1590602847861-f357a9332bbc?w=400&h=300&fit=crop" },
                new Product { Name = "Gaming Capture Card", SKU = "CAP-001", Price = 1999.99m, Qty = 15, Category = "Accessories", IsFeatured = false, Image = "https://images.unsplash.com/photo-1601524909162-ae8725290836?w=400&h=300&fit=crop" },
                new Product { Name = "RGB Gaming Speaker Set", SKU = "SPK-001", Price = 2499.99m, Qty = 18, Category = "Audio", IsFeatured = false, Image = "https://images.unsplash.com/photo-1545454675-3531b543be5d?w=400&h=300&fit=crop" },
                new Product { Name = "Gaming Router WiFi 6", SKU = "NET-001", Price = 2999.99m, Qty = 12, Category = "Networking", IsFeatured = false, Image = "https://images.unsplash.com/photo-1606904825846-647eb07f5be2?w=400&h=300&fit=crop" },
                new Product { Name = "1TB NVMe SSD", SKU = "SSD-001", Price = 1299.99m, Qty = 30, Category = "Storage", IsFeatured = false, Image = "https://images.unsplash.com/photo-1597848212624-a19eb35e2651?w=400&h=300&fit=crop" },
                new Product { Name = "16GB DDR5 Gaming RAM", SKU = "RAM-001", Price = 999.99m, Qty = 25, Category = "Components", IsFeatured = false, Image = "https://images.unsplash.com/photo-1562976540-1502c2145186?w=400&h=300&fit=crop" },
                new Product { Name = "Gaming Controller Charging Dock", SKU = "ACC-002", Price = 499.99m, Qty = 50, Category = "Accessories", IsFeatured = false, Image = "https://images.unsplash.com/photo-1593508512255-86ab42a8e620?w=400&h=300&fit=crop" },
                new Product { Name = "Gaming Glasses Anti Blue Light", SKU = "ACC-003", Price = 399.99m, Qty = 60, Category = "Accessories", IsFeatured = false, Image = "https://images.unsplash.com/photo-1574258495973-f010dfbb5371?w=400&h=300&fit=crop" }
            };

            context.Products.AddRange(products);
            context.SaveChanges();
        }
    }
}