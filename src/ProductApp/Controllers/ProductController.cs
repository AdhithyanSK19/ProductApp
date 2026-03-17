using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProductApp.Models;
using ProductApp.Services;

namespace ProductApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        [Route("GetAllProdusts")]
        public async Task<ActionResult<List<Product>>> GetAllProductsAsync()
        {
            Console.WriteLine("Entered");
            var allProds = await _productService.GetProducts();
            Console.WriteLine(allProds.Count);
            if (allProds.Count == 0)
                return NotFound("No records");
            return Ok(allProds);
        }

        [HttpPost]
        [Route("AddProduct")]
        public async Task<ActionResult<Product>> AddProductAsync(Product product)
        {
            Product pro = await _productService.AddProduct(product);
            return CreatedAtAction(nameof(AddProductAsync), new { id = pro.Id, pro });
        }

        [HttpGet]
        [Route("GetProductById/{id}")]
        public async Task<ActionResult<Product>> GetProductByIdAsync(int id)
        {
            Product pro = await _productService.GetProductById(id);
            if (pro == null)
                return NotFound("No record found");
            return Ok(pro);
        }
        
        [HttpDelete]
        [Route("DeleteProductById/{id}")]
        public async Task<ActionResult> DeleteProductByIdAsync(int id)
        {
            Product product = await _productService.GetProductById(id);
            if (product == null)
                return BadRequest("No such record");
            await _productService.DeleteProduct(product);
            return Ok("Item deleted Successfully");
        }

        [HttpPut]
        [Route("UpdateProduct/{id}")]
        public async Task<ActionResult<Product>> UpdateProductAsync(int id, [FromBody] Product product)
        {
            if (id != product.Id)
                return BadRequest("ID mismatch");
            Product updatedProd = await _productService.UpdateProduct(product);
            if (updatedProd == null)
                return NotFound("No such record");
            return Ok(updatedProd);

        }

    }
}