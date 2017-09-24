using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace phoodchef.Models.DTOs
{
    public class RecIngredDto
    {
       public int RecipeId { get; set; }
       public int IngredientId { get; set; }
       public string Unit { get; set; }
       public Decimal Amt { get; set; }
    }
}