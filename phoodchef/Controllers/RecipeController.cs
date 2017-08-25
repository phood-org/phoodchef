using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using phoodchef.Models.DTOs;
using phoodchef.Models;

namespace phoodchef.Controllers
{
    public class RecipeController : ApiController
    {
        private readonly phoodbEntities db = new phoodbEntities();
        // For reference: public async Task<IHttpActionResult> GetAllRecipes()

        [HttpGet]
        [Route("api/recipes")]
        public IHttpActionResult GetAllRecipes()
        {
            return Ok(db.recipes);
        }

        [HttpPost]
        [Route("api/recipes")]
        public IHttpActionResult AddRecipe([FromBody] RecipeDto newRecipe)
        {
            try
            {
                var dbRecipe = new recipe
                {
                    name = newRecipe.Name,
                    cookTime = newRecipe.CookTime,
                    cookunit = newRecipe.CookUnit,
                    serveMax = newRecipe.ServeMax,
                    serveMin = newRecipe.ServeMin,
                    //yield = newRecipe.Yield,
                    instructions = newRecipe.Instructions
                };

                db.recipes.Add(dbRecipe);
                db.SaveChanges();

                return Created($"api/recipes/{dbRecipe.id}", dbRecipe);
            }
            catch(Exception ex)
            {
                return BadRequest("Could not add recipe to database");
            }
        }
      
    }
}
