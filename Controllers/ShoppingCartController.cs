using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DrinkAndGo.Data.Interfaces;
using DrinkAndGo.Data.Models;
using DrinkAndGo.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DrinkAndGo.Controllers
{
    public class ShoppingCartController : Controller
    {
        readonly IShoppingCartRepository _repository;
        readonly IRepository<Drink, int> _drinkRepository;

        public ShoppingCartController(IRepository<Drink, int> drinkRepository, IShoppingCartRepository repository)
        {
            _repository = repository;
            _drinkRepository = drinkRepository;
        }


        /// <summary>
        /// This  is used to show to show the shopping cart of user.
        /// </summary>
        /// <returns></returns>
        public ViewResult Index()
        {
            ShoppingCart shoppingCart = new ShoppingCart
            {
                ShoppingCartId = _repository.GetCart().ShoppingCartId,
                ShoppingCartItems = _repository.GetShoppingCartItems().ToList()
            };

            var shoppingCartVM = new ShoppingCartViewModel
            {
                ShoppingCart = shoppingCart,
                ShoppingCartTotal = _repository.GetShoppingCartTotal()
            };

            return View(shoppingCartVM);
        }


        public IActionResult AddToShoppingCart(int drinkId)
        {
            var drink = _drinkRepository.SingleOrDefault(x => x.Id == drinkId);

            if (drink != null)
            {
                _repository.AddToCart(drinkId, 1);
            }
            else
            {
                Response.StatusCode = 404;
                return View("DrinkNotFound", drinkId);
            }

            return RedirectToAction("Index");
        }

        public RedirectToActionResult RemoveFromShoppingCart(int drinkId)
        {
            var drink = _repository.SingleOrDefault(x => x.DrinkId == drinkId);

            if (drink != null)
            {
                _repository.RemoveFromCart(drinkId);
            }

            return RedirectToAction("Index");
        }
    }
}

