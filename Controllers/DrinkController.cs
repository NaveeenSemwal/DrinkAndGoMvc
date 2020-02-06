using DrinkAndGo.Data;
using DrinkAndGo.Data.Interfaces;
using DrinkAndGo.Data.Models;
using DrinkAndGo.Data.Repositories;
using DrinkAndGo.ViewModels;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DrinkAndGo.Controllers
{
    public class DrinkController : Controller
    {
        readonly IRepository<Drink, int> _drinkRepository;
        private readonly TelemetryClient telemetryClient;

        public DrinkController(IRepository<Drink, int> repository, TelemetryClient telemetryClient)
        {
            _drinkRepository = repository;
            this.telemetryClient = telemetryClient;
        }

        [ActionName("List")]
        public async Task<IActionResult> Index(string category)
        {
            this.telemetryClient.TrackEvent("DrinkListEvent");

            IEnumerable<Drink> drinks = null;

            try
            {
                int b = 0;
                int result = 5 / b;
                if (!string.IsNullOrEmpty(category))
                {
                    drinks = await _drinkRepository.Find(x => string.Compare(x.Category.Name, category, System.StringComparison.OrdinalIgnoreCase) == 0)
                               .OrderByDescending(x => x.Id).ToListAsync();
                }
                else
                {
                    drinks = await _drinkRepository.GetAll();
                }
            }
            catch (ApplicationException ex)
            {
                this.telemetryClient.TrackException(ex);
            }
            catch (Exception ex)
            {
                this.telemetryClient.TrackException(ex);
            }

            DrinkViewModel drinkViewModel = new DrinkViewModel() { Drinks = drinks };
            return View(drinkViewModel);
        }
    }
}