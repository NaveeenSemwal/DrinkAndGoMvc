using DrinkAndGo.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DrinkAndGo.Data.Interfaces
{
    public interface IShoppingCartRepository : IRepository<ShoppingCartItem, Int32>
    {
        void AddToCart(int drinkId, int quantity);

        ShoppingCart GetCart();

        void RemoveFromCart(int drinkId);

        Task<IEnumerable<ShoppingCartItem>> GetShoppingCartItems();

        void ClearCart();

        decimal GetShoppingCartTotal();


    }
}
