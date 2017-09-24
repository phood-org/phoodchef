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
    public class IngredientController : ApiController
    {
        private readonly phoodbEntities db = new phoodbEntities();
        // For reference: public async Task<IHttpActionResult> GetAllLibraries()

        [HttpGet]
        [Route("api/ingredients")]
        public IHttpActionResult GetAllIngredients([FromUri] int limit = 10, [FromUri] int page = 1, [FromUri] string search = null)
        {
            var result = db.ingredients
                .Where(i => search == null || i.Name.Contains(search))
                .OrderBy(i => i.ID)
                .Skip((page - 1) * limit)
                .Take(limit)
                .ProjectTo<IngredientDto>()
                .ToList();
            return new ReturnWrapper(result);
        }

        [HttpGet]
        [Route("api/ingredients/{id:int}")]
        public IHttpActionResult GetSingleIngredient(int id)
        {
            var dbIngredient = db.ingredients.FirstOrDefault(i => i.ID == id);
            if (dbIngredient != default(ingredient))
            {
                return new ReturnWrapper(Mapper.Map<ingredient, IngredientDto>(dbIngredient));
            }
            else
            {
                return new ReturnWrapper(HttpStatusCode.BadRequest, $"Could not find ingredient with ID {id}");
            }
        }

        [HttpPost]
        [Route("api/ingredients")]
        public IHttpActionResult AddIngredient([FromBody] IngredientDto newIngredient)
        {
            try
            {
                var dbIngredient = Mapper.Map<IngredientDto, ingredient>(newIngredient);

                db.ingredients.Add(dbIngredient);
                db.SaveChanges();
                return new ReturnWrapper(Mapper.Map<ingredient, IngredientDto>(dbIngredient));
            }
            catch (Exception)
            {
                return new ReturnWrapper(HttpStatusCode.BadRequest, "Could not add ingredient to database");
            }
        }

        [HttpDelete]
        [Route("api/ingredients/{id:int}")]
        public IHttpActionResult DeleteIngredient(int id)
        {
            var dbIngredient = db.ingredients.FirstOrDefault(i => i.ID == id);
            if (dbIngredient != default(ingredient))
            {
                db.ingredients.Remove(dbIngredient);
                db.SaveChanges();
                return new ReturnWrapper(HttpStatusCode.OK, $"Ingredient with ID {id} was deleted from the database");
            }
            else
            {
                return new ReturnWrapper(HttpStatusCode.BadRequest, $"Ingredient with ID {id} does not exist and could not be deleted.");
            }
        }
    }
}
