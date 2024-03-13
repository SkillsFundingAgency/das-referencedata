using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(SFA.DAS.ReferenceData.Api.Startup))]

namespace SFA.DAS.ReferenceData.Api
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //ConfigureAuth(app);
        }
    }
}
