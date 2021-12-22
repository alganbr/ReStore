using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using API.Managers;
using API.Models.Basket;
using AutoMapper;

namespace API.Controllers
{
    public class BasketController : BaseApiController
    {
        private readonly IBasketManager _basketManager;
        private readonly IMapper _mapper;

        public BasketController(IBasketManager basketManager, IMapper mapper)
        {
            _basketManager = basketManager;
            _mapper = mapper;
        }
        
        [HttpGet]
        public async Task<ActionResult<BasketRs>> GetBasket()
        {
            var buyerId = Request.Cookies["buyerId"];
            var basket = await _basketManager.GetBasket(buyerId);
            var response = _mapper.Map<BasketRs>(basket);
            return Ok(response);
        }
        
        [HttpPost("add-item")]
        public async Task<ActionResult<BasketRs>> AddItemToBasket(BasketItemRq request)
        {
            var buyerId = Request.Cookies["buyerId"] ?? Guid.NewGuid().ToString();
            var basket = await _basketManager.AddItemToBasket(buyerId, request.ProductId, request.Quantity);
            
            Response.Cookies.Append("buyerId", buyerId, new CookieOptions
            {
                IsEssential = true,
                Expires = DateTimeOffset.Now.AddDays(30)
            });

            var response = _mapper.Map<BasketRs>(basket);
            return CreatedAtAction("GetBasket", response);
        }
        
        [HttpDelete("remove-item")]
        public async Task<ActionResult<BasketRs>> RemoveBasketItem(BasketItemRq request)
        {
            var buyerId = Request.Cookies["buyerId"];
            var basket = await _basketManager.RemoveItemFromBasket(buyerId, request.ProductId, request.Quantity);

            var response = _mapper.Map<BasketRs>(basket);
            return Ok(response);
        }
    }
}
