using DrinkAndGo.Data.Interfaces;
using DrinkAndGo.Data.Models;
using DrinkAndGo.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DrinkAndGo.Components
{
    /// <summary>
    /// It is very similar to partial view but it very powerful compared to partial view. 
    /// It does not use model binding but works only with the data we provide when calling into it.
    /// </summary>
    public class ShoppingCartSummary : ViewComponent
    {
        readonly IShoppingCartRepository _repository;

        public ShoppingCartSummary(IShoppingCartRepository repository)
        {
            _repository = repository;
        }

        public IViewComponentResult Invoke()
        {
            ShoppingCart shoppingCart = new ShoppingCart
            {
                ShoppingCartId = _repository.GetCart().ShoppingCartId,
                ShoppingCartItems = _repository.GetShoppingCartItems().Result
            };

            var shoppingCartVM = new ShoppingCartViewModel
            {
                ShoppingCart = shoppingCart,
                ShoppingCartTotal = _repository.GetShoppingCartTotal()
            };
            return View(shoppingCartVM);
        }

    }
}
