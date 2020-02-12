using DrinkAndGo.Data;
using DrinkAndGo.Data.Interfaces;
using DrinkAndGo.Data.Models;
using DrinkAndGo.Data.Repositories;
using DrinkAndGo.ViewModels;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
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
        private readonly IDistributedCache _cache;

        /// <summary>
        /// Added IDistributedCache for  Azure Redis cache. 
        /// Regardless of which implementation is selected, the app interacts with the cache using a common IDistributedCache interface.
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="telemetryClient"></param>
        /// <param name="cache"></param>
        public DrinkController(IRepository<Drink, int> repository, TelemetryClient telemetryClient, IDistributedCache cache)
        {
            _drinkRepository = repository;
            this.telemetryClient = telemetryClient;
            _cache = cache;
        }

        [ActionName("List")]
        public async Task<IActionResult> Index(string category)
        {
            this.telemetryClient.TrackEvent("DrinkListEvent");

            IEnumerable<Drink> drinks = null;

            try
            {
                if (!string.IsNullOrEmpty(category))
                {
                    drinks = await _drinkRepository.Find(x => string.Compare(x.Category.Name, category, System.StringComparison.OrdinalIgnoreCase) == 0)
                           .OrderByDescending(x => x.Id).ToListAsync();
                }
                else
                {
                    var cacheDrinks = await _cache.GetStringAsync("drinklist");

                    if (cacheDrinks == null)
                    {
                        drinks = await _drinkRepository.GetAll();

                        _cache.SetString("drinklist", JsonConvert.SerializeObject(drinks));
                    }
                    else
                    {
                        drinks = JsonConvert.DeserializeObject<List<Drink>>(cacheDrinks);
                    }
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