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
    public class LibraryController : ApiController
    {
        private readonly phoodbEntities db = new phoodbEntities();
        // For reference: public async Task<IHttpActionResult> GetAllLibraries()

        [HttpGet]
        [Route("api/Libraries")]
        public IHttpActionResult GetAllLibraries([FromUri] int limit = 10, [FromUri] int page = 1, [FromUri] string search = null)
        {
            var result = db.libraries
                .Where(l => search == null || l.Name.Contains(search))
                .OrderBy(l => l.ID)
                .Skip((page - 1) * limit)
                .Take(limit)
                .ProjectTo<LibraryDto>()
                .ToList();
            return new ReturnWrapper(result);
        }

        [HttpGet]
        [Route("api/libraries/{id:int}")]
        public IHttpActionResult GetSingleLibrary(int id)
        {
            var dbLibrary = db.libraries.FirstOrDefault(l => l.ID == id);
            if (dbLibrary != default(library))
            {
                return new ReturnWrapper(Mapper.Map<library, LibraryDto>(dbLibrary));
            }
            else
            {
                return new ReturnWrapper(HttpStatusCode.BadRequest, $"Could not find library with ID {id}");
            }
        }

        [HttpPost]
        [Route("api/libraries")]
        public IHttpActionResult AddLibrary([FromBody] LibraryDto newLibrary)
        {
            try
            {
                var dbLibrary = Mapper.Map<LibraryDto, library>(newLibrary);

                db.libraries.Add(dbLibrary);
                db.SaveChanges();
                return new ReturnWrapper(Mapper.Map<library, LibraryDto>(dbLibrary), $"Library {dbLibrary.ID} added to database");
            }
            catch (Exception)
            {
                return new ReturnWrapper(HttpStatusCode.BadRequest, "Could not add library to database");

            }
        }

        [HttpDelete]
        [Route("api/libraries/{id:int}")]
        public IHttpActionResult DeleteLibrary(int id)
        {
            var dbLibrary = db.libraries.FirstOrDefault(l => l.ID == id);
            if (dbLibrary != default(library))
            {
                db.libraries.Remove(dbLibrary);
                db.SaveChanges();
                return new ReturnWrapper(HttpStatusCode.OK, $"Library with ID {id} has been deleted from the database.");
            }
            else
            {
                return new ReturnWrapper(HttpStatusCode.BadRequest, $"Library with ID {id} does not exist and could not be deleted.");
            }
        }
        
        //Patch involves changes to two different tables in the db.
        // Table 1: library - change to library name (only that as of 9/10/17)
        // Table 2: reclib - adding recipies to library
        // two patches?
        //[HttpPatch]
        //[Route("api/libraries/{id:int}")]
        //public IHttpActionResult UpdateLibrary(int id, [FromBody]JObject patchLibrary)
        //{
        //    return new NotImplementedException();
        //}

        [HttpPost]
        [Route("api/libraries/{id:int}")]
        public IHttpActionResult AddRecipeToLibrary(int id, [FromBody]int recipeId)
        {
            var dbLibrary = db.libraries.FirstOrDefault(l => l.ID == id);
            var dbRecipe = db.recipes.FirstOrDefault(r => r.ID == recipeId);

            List<string> errors = new List<string>();
            
            if (dbLibrary == default(library))
            {
                errors.Add($"Library with ID {id} does not exist and could not be deleted.");
            }
            if (dbRecipe == default(recipe))
            {
                errors.Add($"Recipe with ID {recipeId} does not exist and could not be deleted.");
            }
            if (errors.Any())
            {
                return new ReturnWrapper(HttpStatusCode.BadRequest, errors);
            }
            
            dbLibrary.recipes.Add(dbRecipe);
            db.SaveChanges();
            return new ReturnWrapper(Mapper.Map<library, LibraryDto>(dbLibrary));
        }

    }
}
