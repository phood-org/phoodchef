using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using phoodchef.Models.DTOs;
using phoodchef.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Newtonsoft.Json.Linq;
using System.Reflection;

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
            var result = db.recipes.ProjectTo<RecipeDto>().ToList();
            return Ok(result);
        }

        [HttpGet]
        [Route("api/recipes/{id:int}")]
        public IHttpActionResult GetSingleRecipe(int id)
        {
            var dbRecipe = db.recipes.FirstOrDefault(r => r.ID == id);
            if (dbRecipe != default(recipe))
            {
                var result = Mapper.Map<recipe, RecipeDto>(dbRecipe);
                return Ok(result);
            }
            else
            {
                return BadRequest($"Could not find recipe with ID {id}");
            }
        }

        [HttpPost]
        [Route("api/recipes")]
        public IHttpActionResult AddRecipe([FromBody] RecipeDto newRecipe)
        {
            try
            {
                var dbRecipe = Mapper.Map<RecipeDto, recipe>(newRecipe);

                db.recipes.Add(dbRecipe);
                db.SaveChanges();

                return Created($"api/recipes/{dbRecipe.ID}", dbRecipe);
            }
            catch(Exception)
            {
                return BadRequest("Could not add recipe to database");
            }
        }

        [HttpDelete]
        [Route("api/recipes/{id:int}")]
        public IHttpActionResult DeleteRecipe(int id)
        {
            var dbRecipe = db.recipes.FirstOrDefault(r => r.ID == id);
            if (dbRecipe != default(recipe))
            {
                db.recipes.Remove(dbRecipe);
                db.SaveChanges();
                return Ok();
            }
            else
            {
                return BadRequest($"Recipe with ID {id} does not exist and could not be deleted.");
            }
        }

        [HttpPatch]
        [Route("api/recipes/{id:int}")]
        public IHttpActionResult UpdateRecipe(int id, [FromBody]JObject patchRecipe)
        {

            // get the expense from the repository
            var dbRecipe = db.recipes.FirstOrDefault(r => r.ID == id);
            if(dbRecipe == default(recipe))
            {
                return new ReturnWrapper(HttpStatusCode.BadRequest, $"Recipe with ID {id} does not exist and could not be updated.");
            }

            // Convert dbRecipe to RecipeDTO
            var recipeDto = Mapper.Map<recipe, RecipeDto>(dbRecipe);

            var errorMessages = new List<string>();
            // apply the patch document 
            try
            {
                //Type recipeType = typeof(recipe);
                foreach (JProperty property in patchRecipe.Properties())
                {
                    var prop = typeof(RecipeDto)
                        .GetProperty(property.Name,
                        BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance); 
                    if(prop != null && prop.CanWrite)
                    {
                        prop.SetValue(recipeDto, property.Value.ToObject(prop.PropertyType));
                    } else
                    {
                        errorMessages.Add($"Could not update the {property.Name} property. Are you sure it exists?");
                    }
                }

                dbRecipe = Mapper.Map<RecipeDto, recipe>(recipeDto, dbRecipe);
                db.SaveChanges();

            }
            catch (Exception)
            {
                return new ReturnWrapper(HttpStatusCode.BadRequest, $"Could not apply patch to recipe with ID {id}.");
            }

            return new ReturnWrapper(Mapper.Map<recipe, RecipeDto>(dbRecipe), errorMessages);
        }
    }
}
