using Moq;
using ProductCRUDApp.Controllers;
using ProductCRUDApp.Data;
using ProductCRUDApp.Models;
using ProductCRUDApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace ProductCRUDApp.Tests.ControllersTests
{
    public class ProductsControllerTests
    {
        private readonly ProductsController _controller;
        private readonly Mock<EmailService> _mockEmailService;
        private readonly AppDbContext _context;

        public ProductsControllerTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "ProductDb")
                .Options;

            _context = new AppDbContext(options);
            _mockEmailService = new Mock<EmailService>();
            _controller = new ProductsController(_context, _mockEmailService.Object);
        }

        [Fact]
        public async Task GetProducts_ReturnsAllProducts()
        {
            // Arrange
            _context.Products.Add(new Product { Name = "Product1", Description = "Description1", Price = 10, Stock = 5 });
            _context.Products.Add(new Product { Name = "Product2", Description = "Description2", Price = 20, Stock = 10 });
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.GetProducts();

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<Product>>>(result);
            var products = Assert.IsType<List<Product>>(actionResult.Value);
            Assert.Equal(2, products.Count);
        }

        [Fact]
        public async Task GetProduct_ReturnsProductById()
        {
            // Arrange
            var product = new Product { Name = "Product1", Description = "Description1", Price = 10, Stock = 5 };
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.GetProduct(product.Id);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Product>>(result);
            var returnedProduct = Assert.IsType<Product>(actionResult.Value);
            Assert.Equal(product.Name, returnedProduct.Name);
        }

        [Fact]
        public async Task CreateProduct_ReturnsCreatedProduct()
        {
            // Arrange
            var newProduct = new Product { Name = "Product1", Description = "Description1", Price = 10, Stock = 5 };

            // Act
            var result = await _controller.CreateProduct(newProduct);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Product>>(result);
            var createdProduct = Assert.IsType<Product>(actionResult.Value);
            Assert.Equal(newProduct.Name, createdProduct.Name);
        }

        // Add additional tests for Update and Delete actions
    }
}
