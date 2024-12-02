using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Azure;
using System.Configuration;
using System.Net;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace SFA.DAS.ReferenceData.Api
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            TelemetryConfiguration.Active.ConnectionString = ConfigurationManager.AppSettings["APPLICATIONINSIGHTS_CONNECTION_STRING"];
        }
    }
}
