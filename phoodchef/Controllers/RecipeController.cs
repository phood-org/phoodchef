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
            return Ok(db.recipes.Select(r => new RecipeDto()
            {
                CookTime = (int) r.CookTime ,
                CookUnit = r.CookUnit,
                Id = r.ID,
                Instructions = r.Instructions,
                Name = r.Name,
                ServeMax = (int) r.ServeMax,
                ServeMin = (int) r.ServeMin,
                Yield = r.Yield
            }));
        }

        [HttpPost]
        [Route("api/recipes")]
        public IHttpActionResult AddRecipe([FromBody] RecipeDto newRecipe)
        {
            try
            {
                var dbRecipe = new recipe
                {
                    Name = newRecipe.Name,
                    CookTime = newRecipe.CookTime,
                    CookUnit = newRecipe.CookUnit,
                    ServeMax = newRecipe.ServeMax,
                    ServeMin = newRecipe.ServeMin,
                    //yield = newRecipe.Yield,
                    Instructions = newRecipe.Instructions
                };

                db.recipes.Add(dbRecipe);
                db.SaveChanges();

                return Created($"api/recipes/{dbRecipe.ID}", dbRecipe);
            }
            catch(Exception)
            {
                return BadRequest("Could not add recipe to database");
            }
        }
      
    }
}
