using DrinkAndGo.Data.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DrinkAndGo
{
    /// <summary>
    /// one category : many category
    /// </summary>
    public class Category
    {
        //[Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Drink> Drinks { get; set; }
    }
}