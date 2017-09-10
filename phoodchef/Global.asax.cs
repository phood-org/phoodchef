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
                cfg.CreateMap<library, LibraryDto>()
                    .ForMember(
                        dest => dest.Recipes,
                        opt => opt.MapFrom(src => src.recipes.Select(r => r.ID).ToList()));//.ReverseMap();
                cfg.CreateMap<LibraryDto, library>();
            });

            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
