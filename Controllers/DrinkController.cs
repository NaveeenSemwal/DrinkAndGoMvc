using DrinkAndGo.Data;
using DrinkAndGo.Data.Interfaces;
using DrinkAndGo.Data.Models;
using DrinkAndGo.Data.Repositories;
using DrinkAndGo.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DrinkAndGo.Controllers
{
    public class DrinkController : Controller
    {
        readonly IRepository<Drink, int> _drinkRepository;

        public DrinkController(IRepository<Drink, int> repository)
        {
            _drinkRepository = repository;
        }

        [ActionName("List")]
        public async Task<IActionResult> Index(string category)
        {
            IEnumerable<Drink> drinks;

            if (!string.IsNullOrEmpty(category))
            {
                drinks = await _drinkRepository.Find(x => string.Compare(x.Category.Name, category, System.StringComparison.OrdinalIgnoreCase) == 0)
                           .OrderByDescending(x => x.Id).ToListAsync();
            }
            else
            {
                drinks = await _drinkRepository.GetAll();
            }

            DrinkViewModel drinkViewModel = new DrinkViewModel() { Drinks = drinks };
            return View(drinkViewModel);
        }
    }
}