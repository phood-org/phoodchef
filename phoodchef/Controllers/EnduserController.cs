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
    public class EnduserController : ApiController
    {
        private readonly phoodbEntities db = new phoodbEntities();
        // For reference: public async Task<IHttpActionResult> GetAllLibraries()

        [HttpGet]
        [Route("api/endusers")]
        public IHttpActionResult GetAllEndusers()
        {
            var result = db.endusers.ProjectTo<EnduserDto>().ToList();
            return new ReturnWrapper(result);
        }

        [HttpGet]
        [Route("api/endusers/{id:int}")]
        public IHttpActionResult GetSingleEnduser(int id)
        {
            var dbEnduser = db.endusers.FirstOrDefault(u => u.ID == id);
            if (dbEnduser != default(enduser))
            {
                return new ReturnWrapper(Mapper.Map<enduser, EnduserDto>(dbEnduser));
            }
            else
            {
                return new ReturnWrapper(HttpStatusCode.BadRequest, $"Could not find enduser with ID {id}");
            }
        }

        [HttpPost]
        [Route("api/endusers")]
        public IHttpActionResult AddEnduser([FromBody] EnduserDto newEnduser)
        {
            try
            {
                var dbEnduser = Mapper.Map<EnduserDto, enduser>(newEnduser);

                db.endusers.Add(dbEnduser);
                db.SaveChanges();
                return new ReturnWrapper(Mapper.Map<enduser, EnduserDto>(dbEnduser));
            }
            catch (Exception)
            {
                return new ReturnWrapper(HttpStatusCode.BadRequest, "Could not add enduser to database");
            }
        }

        [HttpDelete]
        [Route("api/endusers/{id:int}")]
        public IHttpActionResult DeleteEnduser(int id)
        {
            var dbEnduser = db.endusers.FirstOrDefault(u => u.ID == id);
            if (dbEnduser != default(enduser))
            {
                db.endusers.Remove(dbEnduser);
                db.SaveChanges();
                return new ReturnWrapper(HttpStatusCode.OK, $"Enduser with ID {id} was deleted from the database");
            }
            else
            {
                return new ReturnWrapper(HttpStatusCode.BadRequest, $"Enduser with ID {id} does not exist and could not be deleted.");
            }
        }
    }
}
