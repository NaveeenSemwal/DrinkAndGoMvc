using DrinkAndGo.Data.Interfaces;
using DrinkAndGo.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DrinkAndGo.Data.Repositories
{
    public class ShoppingCartRepository : EntityRepository<ShoppingCartItem, Int32>, IShoppingCartRepository
    {
        IHttpContextAccessor _httpContextAccessor;
        public ShoppingCartRepository(DrinkDbContext context, IHttpContextAccessor httpContextAccessor) : base(context)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void AddToCart(int drinkId, int quantity)
        {
            string shoppingCartId = GetCart().ShoppingCartId;
            var ShoppingcartItem = SingleOrDefault(x => x.DrinkId == drinkId && x.ShoppingCartId == shoppingCartId);

            if (ShoppingcartItem == null)
            {
                ShoppingcartItem = new ShoppingCartItem { ShoppingCartId = shoppingCartId, DrinkId = drinkId, Quantity = quantity };
                base.Add(ShoppingcartItem);
            }
            else
            {
                ShoppingcartItem.Quantity = ShoppingcartItem.Quantity + quantity;
                base.Update(ShoppingcartItem);
            }
        }

        public void ClearCart()
        {
            string shoppingCartId = GetCart().ShoppingCartId;
            var ShoppingcartItems = Find(x => x.ShoppingCartId == shoppingCartId).AsEnumerable();
            RemoveRange(ShoppingcartItems);
        }

        public  ShoppingCart GetCart()
        {
            string cartId = _httpContextAccessor.HttpContext.Session.GetString("CartId") ?? Guid.NewGuid().ToString();
            _httpContextAccessor.HttpContext.Session.SetString("CartId", cartId);
            return new ShoppingCart { ShoppingCartId = cartId };
        }

        public IEnumerable<ShoppingCartItem> GetShoppingCartItems()
        {
            string shoppingCartId = GetCart().ShoppingCartId;
            var ShoppingcartItems = Find(x => x.ShoppingCartId == shoppingCartId).Include(s => s.Drink).ToList();
            return ShoppingcartItems;
        }

        public void RemoveFromCart(int drinkId)
        {
            string shoppingCartId = GetCart().ShoppingCartId;
            var ShoppingcartItem = SingleOrDefault(x => x.DrinkId == drinkId && x.ShoppingCartId == shoppingCartId);

            if (ShoppingcartItem != null)
            {
                if (ShoppingcartItem.Quantity > 1)
                {
                    ShoppingcartItem.Quantity--;
                    base.Update(ShoppingcartItem);
                }
                else
                {
                    base.Remove(ShoppingcartItem);
                }
            }
        }

        public decimal GetShoppingCartTotal()
        {
            string shoppingCartId = GetCart().ShoppingCartId;
            var total = Find(x => x.ShoppingCartId == shoppingCartId).Select(x => x.Quantity * x.Drink.Price).Sum();
            return total;

        }
    }
}
