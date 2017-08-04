using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace phoodchef.Models.DTOs
{
    public class RecipeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public TimeSpan CookTime { get; set; }
        public string CookUnit { get; set; }
        public string Instructions { get; set; }
        public double Yield { get; set; }
        public double ServeMin { get; set; }
        public double ServeMax { get; set; }
    }
}