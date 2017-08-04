using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using phoodchef.Models.DTOs;

namespace phoodchef.Controllers
{
    public class RecipeController : ApiController
    {
        // For reference: public async Task<IHttpActionResult> GetAllRecipes()
        [HttpGet]
        [Route("api/recipes")]
        public IHttpActionResult GetAllRecipes()
        {
            List<RecipeDto> recipes = new List<RecipeDto>
            {
                new RecipeDto
                {
                    Id = 1,
                    Name = "Pepperocini Beef",
                    CookTime = TimeSpan.FromHours(8),
                    CookUnit = "Hours",
                    Instructions = "Buy Beef and Pepperocinis. Put in crock pot. Eat",
                    ServeMax = 4.0,
                    ServeMin = 2.0,
                    Yield = 4.0
                },
                new RecipeDto
                {
                    Id = 1,
                    Name = "Spagetti",
                    CookTime = TimeSpan.FromMinutes(30),
                    CookUnit = "Minutes",
                    Instructions = "Noodles and Ragu. Done.",
                    ServeMax = 10.0,
                    ServeMin = 2.0,
                    Yield = 4.0
                }
            };

            return Ok(recipes);
        }
    }
}
