using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace phoodchef.Models.DTOs
{
    public class LibraryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<int> Recipes { get; set; }
    }
}