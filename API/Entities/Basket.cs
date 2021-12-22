using System.Collections.Generic;
using System.Linq;

namespace API.Entities;

public class Basket
{
    public int Id { get; set; }
    public string BuyerId { get; set; }
    public List<BasketItem> Items { get; set; } = new();

    public void AddItem(Product product, int quantity)
    {
        var existingItem = Items.FirstOrDefault(item => item.ProductId == product.Id);
        if (existingItem == null)
        {
            Items.Add(new BasketItem
            {
                Product = product,
                Quantity = quantity
            });
        }
        else
        {
            existingItem.Quantity += quantity;
        }
    }

    public void RemoveItem(Product product, int quantity)
    {
        var existingItem = Items.FirstOrDefault(item => item.ProductId == product.Id);
        if (existingItem == null)
            return;

        existingItem.Quantity -= quantity;
        if (existingItem.Quantity <= 0)
            Items.Remove(existingItem);
    }
}