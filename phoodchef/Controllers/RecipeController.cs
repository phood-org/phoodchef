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
        public IHttpActionResult GetAllRecipes([FromUri] int limit = 10, [FromUri] int page = 1, [FromUri] string search = null)
        {
            var result = db.recipes
                .Where(r => search == null || r.Name.Contains(search))
                .OrderBy(r => r.ID)
                .Skip((page - 1) * limit)
                .Take(limit)
                .ProjectTo<RecipeDto>()
                .ToList();

            return new ReturnWrapper(result);
        }

        [HttpGet]
        [Route("api/recipes/{id:int}")]
        public IHttpActionResult GetSingleRecipe(int id)
        {
            var dbRecipe = db.recipes.FirstOrDefault(r => r.ID == id);
            if (dbRecipe != default(recipe))
            {
                return new ReturnWrapper(Mapper.Map<recipe, RecipeDto>(dbRecipe));
            }
            else
            {
                return new ReturnWrapper(HttpStatusCode.BadRequest, $"Could not find recipe with ID {id}");
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
                return new ReturnWrapper(Mapper.Map<recipe, RecipeDto>(dbRecipe), $"Recipe {dbRecipe.ID} added to database"); 
                //return Created($"api/recipes/{dbRecipe.ID}", dbRecipe);
            }
            catch(Exception)
            {
                return new ReturnWrapper(HttpStatusCode.BadRequest, "Could not add recipe to database");
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
                //return Ok();
                return new ReturnWrapper(HttpStatusCode.OK, $"Recipe with ID {id} has been deleted from the database.");
            }
            else
            {
                return new ReturnWrapper(HttpStatusCode.BadRequest, $"Recipe with ID {id} does not exist and could not be deleted.");
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

        [HttpPost]
        [Route("api/recipes/{id:int}/ingredients")]
        public IHttpActionResult AddIngredientToRecipe(int id, [FromBody]RecIngredDto recIngredDto)
        {
            var dbRecipe = db.recipes.FirstOrDefault(r => r.ID == id);
            var dbIngredient = db.ingredients.FirstOrDefault(i => i.ID == recIngredDto.IngredientId);

            List<string> errors = new List<string>();

            if (dbRecipe == default(recipe))
            {
                errors.Add($"Recipe with ID {id} does not exist and could not be added to");
            }
            if (dbIngredient == default(ingredient))
            {
                errors.Add($"Ingredient with ID {recIngredDto.IngredientId} does not exist and could not be added to recipe.");
            }
            if (errors.Any())
            {
                return new ReturnWrapper(HttpStatusCode.BadRequest, errors);
            }

            if (db.recIngreds.Where(ri => ri.RecId == dbRecipe.ID && ri.IngredId == dbIngredient.ID).Any())
            {
                errors.Add($"Ingredient {dbIngredient.ID} already added to Recipe {dbRecipe.ID}");
                return new ReturnWrapper(HttpStatusCode.BadRequest, errors);
            }

            var dbRecIngred = new recIngred()
            {
                recipe = dbRecipe,
                ingredient = dbIngredient,
                Unit = recIngredDto.Unit,
                Amt = recIngredDto.Amt
            };

            dbRecipe.recIngreds.Add(dbRecIngred);
            db.SaveChanges();
            return new ReturnWrapper(Mapper.Map<recipe, RecipeDto>(dbRecipe));
        }

    }
}
