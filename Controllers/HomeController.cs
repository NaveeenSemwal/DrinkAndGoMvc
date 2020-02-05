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
    public class HomeController : Controller
    {
        readonly IRepository<Drink, int> _repository;

        public HomeController(IRepository<Drink, int> repository)
        {
            _repository = repository;
        }

        public IActionResult Index()
        {
            HomeViewModel drinkViewModel = new HomeViewModel() { PreferredDrinks = _repository.Find(x => x.IsPreferredDrink).ToList() };

            return View(drinkViewModel);
        }


    }
}