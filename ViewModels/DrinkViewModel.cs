using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DrinkAndGo.Data.Models;

namespace DrinkAndGo.ViewModels
{
    public class DrinkViewModel
    {
        public IEnumerable<Drink> Drinks { get; set; }
    }
}
