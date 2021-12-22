using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Managers;

public interface IBasketManager
{
    Task<Basket> GetBasket(string buyerId);
    Task<Basket> AddItemToBasket(string buyerId, int productId, int quantity);
    Task<Basket> RemoveItemFromBasket(string buyerId, int productId, int quantity);
}

public class BasketManager : IBasketManager
{
    private readonly DataContext _context;

    public BasketManager(DataContext context)
    {
        _context = context;
    }

    public async Task<Basket> GetBasket(string buyerId)
    {
        return await _context.Baskets
            .Include(x => x.Items)
            .ThenInclude(x => x.Product)
            .FirstOrDefaultAsync(x => x.BuyerId == buyerId);
    }

    public async Task<Basket> AddItemToBasket(string buyerId, int productId, int quantity)
    {
        var basket = await GetBasket(buyerId) ?? CreateBasket(buyerId);

        var product = await _context.Products.FindAsync(productId);
        if (product == null)
            throw new HttpRequestException("Product not found", null, HttpStatusCode.NotFound);
            
        basket.AddItem(product, quantity);
            
        var result = await _context.SaveChangesAsync();
        if (result == 0)
            throw new HttpRequestException("Error saving item to basket", null, HttpStatusCode.BadRequest);

        return basket;
    }

    public async Task<Basket> RemoveItemFromBasket(string buyerId, int productId, int quantity)
    {
        var basket = await GetBasket(buyerId);
        if (basket == null)
            throw new HttpRequestException("Basket not found", null, HttpStatusCode.NotFound);

        var product = await _context.Products.FindAsync(productId);
        if (product == null)
            throw new HttpRequestException("Product not found", null, HttpStatusCode.NotFound);
        
        basket.RemoveItem(product, quantity);

        var result = await _context.SaveChangesAsync();
        if (result == 0)
            throw new HttpRequestException("Error removing item from basket", null, HttpStatusCode.BadRequest);

        return basket;
    }

    private Basket CreateBasket(string buyerId)
    {
        var basket = new Basket
        {
            BuyerId = buyerId
        };
        
        _context.Baskets.Add(basket);
        return basket;
    }
}