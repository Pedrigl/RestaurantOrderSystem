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

namespace Infrastructure.Tests.RepositoryTests
{
    [TestClass]
    public class ProductRepositoryTests
    {
        private IProductRepository _productRepository;

        [TestInitialize]
        public async Task Initialize()
        {
            RestaurantDbContext dbContext = FakeDbContext.GetFakeDbContext();
            _productRepository = new ProductRepository(dbContext);

            await _productRepository.AddAsync(new Product
            {
                Id = 1,
                Name = "TestProduct",
                Price = 10,
                KitchenArea = KitchenArea.Fries,
                Description = "TestDescription",
                Stock = 10
            });

            await _productRepository.AddAsync(new Product
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
            var newProduct = await _productRepository.AddAsync(new Product
            {
                Id = 3,
                Name = "TestProduct",
                Price = 10,
                KitchenArea = KitchenArea.Fries,
                Description = "TestDescription",
                Stock = 10
            });
            await _productRepository.SaveAsync();

            var product = await _productRepository.GetByIdAsync(3);

            product.Should().NotBeNull();
        }

        [TestMethod]
        public async Task ShouldUpdateProduct()
        {
            var product = await _productRepository.GetByIdAsync(1);
            product.Name = "UpdatedProduct";
            _productRepository.Update(product.Id, product);
            await _productRepository.SaveAsync();

            var updatedProduct = await _productRepository.GetByIdAsync(1);
            updatedProduct.Name.Should().Be("UpdatedProduct");
        }

        [TestMethod]
        public async Task ShouldDeleteProduct()
        {
            var product = await _productRepository.GetByIdAsync(1);
            _productRepository.Delete(product);
            await _productRepository.SaveAsync();

            var deletedProduct = await _productRepository.GetByIdAsync(1);
            deletedProduct.Should().BeNull();
        }

        [TestMethod]
        public async Task ShouldGetProductById()
        {
            var product = await _productRepository.GetByIdAsync(1);
            product.Should().NotBeNull();
            product.Id.Should().Be(1);
        }

        [TestMethod]
        public async Task ShouldGetAllProducts()
        {
            var products = await _productRepository.GetAllAsync();
            products.Should().NotBeNullOrEmpty();
            products.Count().Should().Be(2);
        }

        [TestMethod]
        public void ShouldGetProductsByPredicate()
        {
            var products = _productRepository.GetWhere(p => p.KitchenArea == KitchenArea.Fries);
            products.Should().NotBeNullOrEmpty();
            products.Count().Should().Be(1);
        }

        [TestMethod]
        public async Task ShouldSaveChanges()
        {
            var product = await _productRepository.GetByIdAsync(1);
            product.Name = "UpdatedProduct";
            _productRepository.Update(product.Id, product);
            var result = await _productRepository.SaveAsync();
            result.Should().BeTrue();
        }
    }
}
