using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace phoodchef.Models.DTOs
{
    public class RecIngredDto
    {
        public int RecId { get; set; }
        public int IngredId { get; set; }
        public string Unit { get; set; }
        public Decimal Amt { get; set; }
        public string IngredRole { get; set; }
        public int IngredRec { get; set; }
        public int SubFor { get; set; }
    }
}