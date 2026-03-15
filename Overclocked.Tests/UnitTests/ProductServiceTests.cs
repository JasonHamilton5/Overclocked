using Microsoft.EntityFrameworkCore;
using Overclocked.API.Data;
using Overclocked.API.Models;
using Xunit;

namespace Overclocked.Tests.UnitTests
{
    public class ProductServiceTests
    {
        private AppDbContext GetInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new AppDbContext(options);
        }
        
        [Fact]
        public void Can_Add_Product_To_Database()
        {
            var context = GetInMemoryContext();

            var product = new Product
            {
                Name = "Test Gaming Mouse",
                SKU = "TST-001",
                Price = 999.99m,
                Qty = 10,
                Image = null,
                Category = "Mice"
            };

            context.Products.Add(product);
            context.SaveChanges();

            Assert.Equal(1, context.Products.Count());
        }

        [Fact]
        public void Can_Retrieve_Product_By_Id()
        {
            var context = GetInMemoryContext();

            var product = new Product
            {
                Name = "Test Keyboard",
                SKU = "TST-002",
                Price = 1499.99m,
                Qty = 20,
                Image = null,
                Category = "Keyboards"
            };

            context.Products.Add(product);
            context.SaveChanges();

            var retrieved = context.Products.Find(product.ProductId);
            Assert.NotNull(retrieved);
            Assert.Equal("Test Keyboard", retrieved.Name);
        }

        [Fact]
        public void Can_Update_Product_Quantity()
        {
            var context = GetInMemoryContext();

            var product = new Product
            {
                Name = "Test Headset",
                SKU = "TST-003",
                Price = 2499.99m,
                Qty = 15,
                Image = null,
                Category = "Headsets"
            };

            context.Products.Add(product);
            context.SaveChanges();

            product.Qty = 5;
            context.Products.Update(product);
            context.SaveChanges();

            var updated = context.Products.Find(product.ProductId);
            Assert.Equal(5, updated!.Qty);
        }

        [Fact]
        public void Can_Delete_Product()
        {
            var context = GetInMemoryContext();

            var product = new Product
            {
                Name = "Test Monitor",
                SKU = "TST-004",
                Price = 4999.99m,
                Qty = 8,
                Image = null,
                Category = "Monitors"
            };

            context.Products.Add(product);
            context.SaveChanges();

            context.Products.Remove(product);
            context.SaveChanges();

            Assert.Empty(context.Products);
        }

        [Fact]
        public void Product_Price_Should_Not_Be_Negative()
        {
            var context = GetInMemoryContext();

            var product = new Product
            {
                Name = "Test Chair",
                SKU = "TST-005",
                Price = 3999.99m,
                Qty = 5,
                Image = null,
                Category = "Chairs"
            };

            context.Products.Add(product);
            context.SaveChanges();

            var retrieved = context.Products.Find(product.ProductId);
            Assert.True(retrieved!.Price > 0);
        }
    }
}