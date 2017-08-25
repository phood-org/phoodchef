﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace phoodchef.Models.DTOs
{
    public class RecipeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CookTime { get; set; }
        public string CookUnit { get; set; }
        public string Instructions { get; set; }
        public string Yield { get; set; }
        public int ServeMin { get; set; }
        public int ServeMax { get; set; }
    }
}