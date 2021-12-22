using System.Collections.Generic;

namespace API.Models.Basket;

public class BasketRs
{
    public int Id { get; set; }
    public string BuyerId { get; set; }
    public List<BasketItemRs> Items { get; set; }
}