using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace phoodchef.Models.DTOs
{
    public class EnduserDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        // Will Add in these fields later. Didn't want to get bogged down in the mapping while just roughing everything out
        //public List<int> Libraries { get; set; } 
        //public List<int> Utensils { get; set; }
        //public List<int> Ingredients { get; set; }
    }
}