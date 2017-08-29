using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using Newtonsoft.Json;
using AutoMapper;
using phoodchef.Models;
using phoodchef.Models.DTOs;

namespace phoodchef
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            HttpConfiguration config = GlobalConfiguration.Configuration;
            config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

            Mapper.Initialize(cfg => {
                cfg.CreateMap<recipe, RecipeDto>().ReverseMap();
            });

            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
