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
    public class UtensilController : ApiController
    {
        private readonly phoodbEntities db = new phoodbEntities();
        // For reference: public async Task<IHttpActionResult> GetAllLibraries()

        [HttpGet]
        [Route("api/utensils")]
        public IHttpActionResult GetAllUtensils([FromUri] int limit = 10, [FromUri] int page = 1, [FromUri] string search = null)
        {
            var result = db.utensils
                    .Where(u => search == null || u.Name.Contains(search))
                    .OrderBy(u => u.ID)
                    .Skip((page- 1) * limit)
                    .Take(limit)
                    .ProjectTo<UtensilDto>()
                    .ToList();
 
            return new ReturnWrapper(result);
        }

        [HttpGet]
        [Route("api/utensils/{id:int}")]
        public IHttpActionResult GetSingleUtensil(int id)
        {
            var dbUtensil = db.utensils.FirstOrDefault(u => u.ID == id);
            if (dbUtensil != default(utensil))
            {
                return new ReturnWrapper(Mapper.Map<utensil, UtensilDto>(dbUtensil));
            }
            else
            {
                return new ReturnWrapper(HttpStatusCode.BadRequest, $"Could not find utensil with ID {id}");
            }
        }

        [HttpPost]
        [Route("api/utensils")]
        public IHttpActionResult AddUtensil([FromBody] UtensilDto newUtensil)
        {
            try
            {
                var dbUtensil = Mapper.Map<UtensilDto, utensil>(newUtensil);

                db.utensils.Add(dbUtensil);
                db.SaveChanges();
                return new ReturnWrapper(Mapper.Map<utensil, UtensilDto>(dbUtensil));
            }
            catch (Exception)
            {
                return new ReturnWrapper(HttpStatusCode.BadRequest, "Could not add utensil to database");
            }
        }

        [HttpDelete]
        [Route("api/utensils/{id:int}")]
        public IHttpActionResult DeleteUtensil(int id)
        {
            var dbUtensil = db.utensils.FirstOrDefault(u => u.ID == id);
            if (dbUtensil != default(utensil))
            {
                db.utensils.Remove(dbUtensil);
                db.SaveChanges();
                return new ReturnWrapper(HttpStatusCode.OK, $"Utensil with ID {id} was deleted from the database");
            }
            else
            {
                return new ReturnWrapper(HttpStatusCode.BadRequest, $"Utensil with ID {id} does not exist and could not be deleted.");
            }
        }
    }
}
