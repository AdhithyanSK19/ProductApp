using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using ProductApp.Controllers;
using ProductApp.DatabaseContext;
using ProductApp.Models;
using ProductApp.Services;

namespace ProductApp.Test;

public class UnitTest1
{
    [Fact]
    public async Task GetAllProductsAsync_ReturnsOkWithProducts_WhenProductsExist()
    {
        // Arrange
        var mockService = new Mock<IProductService>();
        var sampleProducts = new List<Product>
        {
            new () { Id = 1, Name = "Product A",Price = 1 },
            new () { Id = 2, Name = "Product B" ,Price=2}
        };
        mockService.Setup(s => s.GetProducts()).ReturnsAsync(sampleProducts);
        var controller = new ProductController(mockService.Object);
        // Act
        var result = await controller.GetAllProductsAsync();
        // Assert
        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(200);

        var returnedProducts = okResult.Value as List<Product>;
        returnedProducts.Should().NotBeNull();
        returnedProducts!.Should().BeEquivalentTo(sampleProducts);
    }
    [Fact]
    public async Task GetProducts_ShouldReturnAllProducts()
    {
        // Arrange
        var sampleProducts = new List<Product>
        {
            new Product { Id = 1, Name = "Item A", Price = 10 },
            new Product { Id = 2, Name = "Item B", Price = 20 }
        }.AsQueryable();

        var mockSet = new Mock<DbSet<Product>>();
        mockSet.As<IQueryable<Product>>().Setup(m => m.Provider).Returns(sampleProducts.Provider);
        mockSet.As<IQueryable<Product>>().Setup(m => m.Expression).Returns(sampleProducts.Expression);
        mockSet.As<IQueryable<Product>>().Setup(m => m.ElementType).Returns(sampleProducts.ElementType);
        mockSet.As<IQueryable<Product>>().Setup(m => m.GetEnumerator()).Returns(sampleProducts.GetEnumerator());

        var mockContext = new Mock<AppDbContext>();
        mockContext.Setup(c => c.Products).Returns(mockSet.Object);

        var service = new ProductService(mockContext.Object);

        // Act
        var result = await service.GetProducts();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Select(p => p.Name).Should().Contain(new[] { "Item A", "Item B" });
    }
}