using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DrinkAndGo.Data.Models
{
    public class ShoppingCartItem
    {
        public int Id { get; set; }
        public int Quantity { get; set; }


        public int DrinkId { get; set; }
        public virtual Drink Drink { get; set; }

        public string ShoppingCartId { get; set; }
    }
}
