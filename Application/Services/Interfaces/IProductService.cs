using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDTO>> GetAllProducts();
        Task<ProductDTO> CreateProduct(ProductDTO product);
        Task UpdateProduct(ProductDTO product);
        Task DeleteProduct(int id);
        Task<ProductDTO> GetProductById(int id);
        void SubtractStock(int id, int quantity);
        void AddStock(int id, int quantity);
    }
}
