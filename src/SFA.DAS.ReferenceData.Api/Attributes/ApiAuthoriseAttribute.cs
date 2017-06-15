using System.Web.Http;
using System.Web.Http.Controllers;

namespace SFA.DAS.ReferenceData.Api.Attributes
{
    public class ApiAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            return actionContext.Request.RequestUri.IsLoopback || base.IsAuthorized(actionContext);
        }
    }
}