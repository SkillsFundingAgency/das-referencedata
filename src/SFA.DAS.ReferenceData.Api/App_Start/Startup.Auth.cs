using Microsoft.Azure;
using Microsoft.Owin.Security.ActiveDirectory;
using Owin;
using System.Configuration;

namespace SFA.DAS.ReferenceData.Api
{
    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            app.UseWindowsAzureActiveDirectoryBearerAuthentication(
                new WindowsAzureActiveDirectoryBearerAuthenticationOptions
                {
                    TokenValidationParameters = new System.IdentityModel.Tokens.TokenValidationParameters
                    {
                        RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",
                        ValidAudiences = ConfigurationManager.AppSettings["idaAudience"].Split(',')
                    },
                    Tenant = ConfigurationManager.AppSettings["idaTenant"]
                });
        }
    }
}
