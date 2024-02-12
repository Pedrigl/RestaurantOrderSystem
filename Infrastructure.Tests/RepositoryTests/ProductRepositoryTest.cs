using Infrastructure.Data.Contexts;
using Infrastructure.Data.Repositories;
using Infrastructure.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Tests.ProductRepositoryTests
{
    [TestClass]
    public class ProductRepositoryTest
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
    }
}
