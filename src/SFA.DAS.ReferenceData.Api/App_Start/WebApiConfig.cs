using System.Web.Http;
using Microsoft.Owin.Security.OAuth;

namespace SFA.DAS.ReferenceData.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();
        }
    }
}