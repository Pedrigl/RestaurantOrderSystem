﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Data.Repositories.Interfaces;

namespace Application.Services
{
    public class ProductService
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

        public async void CreateProduct(ProductDTO product)
        {
            var newProduct = _mapper.Map<Product>(product);
            await _productRepository.AddAsync(newProduct);
            await _productRepository.SaveAsync();
        }

        public async void SubtractStock(int id, int quantity)
        {
            var product = await _productRepository.GetByIdAsync(id);
            product.Stock -= quantity;
            _productRepository.Update(product);
            await _productRepository.SaveAsync();
        }

        public async void AddStock(int id, int quantity)
        {
            var product = await _productRepository.GetByIdAsync(id);
            product.Stock += quantity;
            _productRepository.Update(product);
            await _productRepository.SaveAsync();
        }

        public async void UpdateProduct(ProductDTO product)
        {
            var updatedProduct = _mapper.Map<Product>(product);
            _productRepository.Update(updatedProduct);
            await _productRepository.SaveAsync();
        }

        public async void DeleteProduct(int id)
        {
            var product = _productRepository.GetByIdAsync(id).Result;
            _productRepository.Delete(product);
            await _productRepository.SaveAsync();
        }



    }
}