using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Data.Repositories.Interfaces;

namespace Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        public ProductService(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductDTO>> GetAllProducts()
        {
            var products = await _productRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ProductDTO>>(products);
        }

        public async Task<ProductDTO> GetProductById(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            return _mapper.Map<ProductDTO>(product);
        }

        public async Task<ProductDTO> CreateProduct(ProductDTO product)
        {
            var mappedProduct = _mapper.Map<Product>(product);
            var newProduct = await _productRepository.AddAsync(mappedProduct);
            await _productRepository.SaveAsync();
            
            return _mapper.Map<ProductDTO>(newProduct);
        }

        public async void SubtractStock(int id, int quantity)
        {
            var product = await _productRepository.GetByIdAsync(id);
            product.Stock -= quantity;
            _productRepository.Update(product.Id,product);
            await _productRepository.SaveAsync();
        }

        public async void AddStock(int id, int quantity)
        {
            var product = await _productRepository.GetByIdAsync(id);
            product.Stock += quantity;
            _productRepository.Update(product.Id, product);
            await _productRepository.SaveAsync();
        }

        public async Task UpdateProduct(ProductDTO product)
        {
            var mappedProduct = _mapper.Map<Product>(product);
            _productRepository.Update(mappedProduct.Id, mappedProduct);
            await _productRepository.SaveAsync();
        }

        public async Task DeleteProduct(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if(product == null)
                throw new Exception("Product not found");
                
            _productRepository.Delete(product);
            await _productRepository.SaveAsync();
        }

    }
}
