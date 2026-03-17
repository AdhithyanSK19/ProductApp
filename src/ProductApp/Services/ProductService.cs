using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProductApp.DatabaseContext;
using ProductApp.Models;

namespace ProductApp.Services
{
    public class ProductService(AppDbContext appDbContext):IProductService
    {
        private readonly AppDbContext _appDbContext = appDbContext;

        public async Task<List<Product>> GetProducts()
        {
            return await _appDbContext.Products.ToListAsync();
        }
        public async Task<Product> AddProduct(Product product)
        {
            await _appDbContext.AddAsync(product);
            await _appDbContext.SaveChangesAsync();
            return product;
        }

        public async Task<Product> GetProductById(int id)
        {
            return await _appDbContext.Products.FindAsync(id);
        }

        public async Task DeleteProduct(Product product)
        {
            _appDbContext.Products.Remove(product);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<Product> UpdateProduct(Product updatedProduct)
        {
            var product = await _appDbContext.Products.FindAsync(updatedProduct.Id);
            if (product == null)
                return null;
            product.Name = updatedProduct.Name;
            product.Price = updatedProduct.Price;
            await _appDbContext.SaveChangesAsync();
            return product;
        }
    }
}