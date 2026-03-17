using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProductApp.Models;

namespace ProductApp.Services
{
    public interface IProductService
    {
        Task<List<Product>> GetProducts();
        Task<Product> AddProduct(Product product);
        Task<Product> GetProductById(int id);
        Task DeleteProduct(Product product);
        Task<Product> UpdateProduct(Product product);
    }
}