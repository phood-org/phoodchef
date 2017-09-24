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
    public class CategoryController : ApiController
    {
        private readonly phoodbEntities db = new phoodbEntities();
        // For reference: public async Task<IHttpActionResult> GetAllLibraries()

        [HttpGet]
        [Route("api/categories")]
        public IHttpActionResult GetAllCategories([FromUri] int limit = 10, [FromUri] int page = 1, [FromUri] string search = null)
        {
            var result = db.categories
                .Where(c => search == null || c.Name.Contains(search))
                .OrderBy(c => c.ID)
                .Skip((page - 1) * limit)
                .Take(limit)
                .ProjectTo<CategoryDto>()
                .ToList();
            return new ReturnWrapper(result);
        }

        [HttpGet]
        [Route("api/categories/{id:int}")]
        public IHttpActionResult GetSingleCategory(int id)
        {
            var dbCategory = db.categories.FirstOrDefault(c => c.ID == id);
            if (dbCategory != default(category))
            {
                return new ReturnWrapper(Mapper.Map<category, CategoryDto>(dbCategory));
            }
            else
            {
                return new ReturnWrapper(HttpStatusCode.BadRequest, $"Could not find category with ID {id}");
            }
        }

        [HttpPost]
        [Route("api/categories")]
        public IHttpActionResult AddCategory([FromBody] CategoryDto newCategory)
        {
            try
            {
                var dbCategory = Mapper.Map<CategoryDto, category>(newCategory);

                db.categories.Add(dbCategory);
                db.SaveChanges();
                return new ReturnWrapper(Mapper.Map<category, CategoryDto>(dbCategory));
            }
            catch (Exception)
            {
                return new ReturnWrapper(HttpStatusCode.BadRequest, "Could not add category to database");
            }
        }

        [HttpDelete]
        [Route("api/categories/{id:int}")]
        public IHttpActionResult DeleteCategory(int id)
        {
            var dbCategory = db.categories.FirstOrDefault(c => c.ID == id);
            if (dbCategory != default(category))
            {
                db.categories.Remove(dbCategory);
                db.SaveChanges();
                return new ReturnWrapper(HttpStatusCode.OK, $"Category with ID {id} was deleted from the database");
            }
            else
            {
                return new ReturnWrapper(HttpStatusCode.BadRequest, $"Category with ID {id} does not exist and could not be deleted.");
            }
        }
    }
}
