using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Managers;

public interface IProductManager
{
    Task<List<Product>> GetProducts();
    Task<Product> GetProduct(int id);
    Task<Product> AddProduct(Product product);
    Task<Product> UpdateProduct(Product product);
    Task<bool> DeleteProduct(int id);
}

public class ProductManager : IProductManager
{
    private readonly DataContext _context;

    public ProductManager(DataContext context)
    {
        _context = context;
    }

    public async Task<List<Product>> GetProducts()
    {
        return await _context.Products.ToListAsync();
    }

    public async Task<Product> GetProduct(int id)
    {
        return await _context.Products.FindAsync(id);
    }

    public async Task<Product> AddProduct(Product product)
    {
        _context.Products.Add(product);
        var result = await _context.SaveChangesAsync();
        if (result == 0)
            throw new HttpRequestException("Error when saving product", null, HttpStatusCode.BadRequest);

        return product;
    }

    public async Task<Product> UpdateProduct(Product product)
    {
        var existingProduct = await GetProduct(product.Id);
        if (existingProduct == null)
            throw new HttpRequestException("Product not found", null, HttpStatusCode.NotFound);

        _context.Products.Update(product);
        var result = await _context.SaveChangesAsync();
        if (result == 0)
            throw new HttpRequestException("Error when updating product", null, HttpStatusCode.BadRequest);

        return product;
    }

    public async Task<bool> DeleteProduct(int id)
    {
        var existingProduct = await GetProduct(id);
        if (existingProduct == null)
            throw new HttpRequestException("Product not found", null, HttpStatusCode.NotFound);

        _context.Products.Remove(existingProduct);
        var result = await _context.SaveChangesAsync();

        return result > 0;
    }
}