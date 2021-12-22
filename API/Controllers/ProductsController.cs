using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Data;
using API.Entities;
using API.Managers;
using API.Models.Product;
using AutoMapper;

namespace API.Controllers
{
    public class ProductsController : BaseApiController
    {
        private readonly IProductManager _productManager;
        private readonly IMapper _mapper;

        public ProductsController(IProductManager productManager, IMapper mapper)
        {
            _productManager = productManager;
            _mapper = mapper;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await _productManager.GetProducts();
            return Ok(products);
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductRs>> GetProduct(int id)
        {
            var product = await _productManager.GetProduct(id);
            var response = _mapper.Map<ProductRs>(product);
            return Ok(response);
        }
        
        [HttpPost]
        public async Task<ActionResult<ProductRs>> PostProduct(ProductRq request)
        {
            var product = _mapper.Map<Product>(request);
            product = await _productManager.AddProduct(product);
            var response = _mapper.Map<ProductRs>(product);

            return CreatedAtAction("GetProduct", new { id = product.Id }, response);
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, ProductRq request)
        {
            var product = _mapper.Map<Product>(request);
            product.Id = id;
            
            product = await _productManager.UpdateProduct(product);
            var response = _mapper.Map<ProductRs>(product);
            
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var response = await _productManager.DeleteProduct(id);
            return Ok(response);
        }
    }
}
