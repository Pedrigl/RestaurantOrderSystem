using Application.DTOs;
using Application.Services;
using Application.Services.Interfaces;
using Domain.Entities;
using Domain.Enums;
using FluentAssertions;
using Infrastructure.Data.Contexts;
using Infrastructure.Data.Repositories;
using Infrastructure.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestUtilities.Data;
using TestUtilities.Data.Configuration;

namespace Application.Tests.ServiceTests
{
    [TestClass]
    public class ProductServiceTests
    {
        private IProductService _productService;

        [TestInitialize]
        public async Task Initialize()
        {
            RestaurantDbContext dbContext = FakeDbContext.GetFakeDbContext();
            IProductRepository productRepository = new ProductRepository(dbContext);
            _productService = new ProductService(productRepository, FakeAutoMapper.GetMapper());

            await productRepository.AddAsync(new Product
            {
                Id = 1,
                Name = "TestProduct",
                Price = 10,
                KitchenArea = KitchenArea.Fries,
                Description = "TestDescription",
                Stock = 10
            });

            await productRepository.AddAsync(new Product
            {
                Id = 2,
                Name = "TestProductNoStock",
                Price = 10,
                KitchenArea = KitchenArea.Grill,
                Description = "TestDescription",
                Stock = 0
            });

            await dbContext.SaveChangesAsync();
        }

        [TestMethod]
        public async Task ShouldCreateProduct()
        {
            var newProduct = await _productService.CreateProduct(new ProductDTO
            {
                Name = "TestProduct",
                Price = 10,
                KitchenArea = KitchenArea.Fries,
                Description = "TestDescription",
                Stock = 10
            });

            newProduct.Should().NotBeNull();
            newProduct.Name.Should().Be("TestProduct");
        }

        [TestMethod]
        public async Task ShouldUpdateProduct()
        {
            await _productService.UpdateProduct(new ProductDTO
            {
                Id = 1,
                Name = "TestProductUpdated",
                Price = 10,
                KitchenArea = KitchenArea.Fries,
                Description = "TestDescription",
                Stock = 10
            });

            var updatedProduct = await _productService.GetProductById(1);

            updatedProduct.Should().NotBeNull();
            updatedProduct.Name.Should().Be("TestProductUpdated");
        }

        [TestMethod]
        public async Task ShouldDeleteProduct()
        {
            await _productService.DeleteProduct(1);
            var product = await _productService.GetProductById(1);

            product.Should().BeNull();
        }

        [TestMethod]
        public async Task ShouldGetProductById()
        {
            var product = await _productService.GetProductById(1);

            product.Should().NotBeNull();
            product.Name.Should().Be("TestProduct");
        }

        [TestMethod]
        public async Task ShouldGetAllProducts()
        {
            var products = await _productService.GetAllProducts();

            products.Should().NotBeNull();
            products.Count().Should().Be(2);
        }

        [TestMethod]
        public void ShouldSubtractStock()
        {
            _productService.SubtractStock(1, 5);
            var product = _productService.GetProductById(1).Result;

            product.Stock.Should().Be(5);
        }

        [TestMethod]
        public void ShouldAddStock()
        {
            _productService.AddStock(1, 5);
            var product = _productService.GetProductById(1).Result;

            product.Stock.Should().Be(15);
        }
    }
}
