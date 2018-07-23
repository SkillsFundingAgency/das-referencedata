using Microsoft.Web.Administration;
using Microsoft.WindowsAzure.ServiceRuntime;
using System.Linq;

namespace SFA.DAS.ReferenceData.Api
{
    public class WebRole : RoleEntryPoint
    {
        public override void Run()
        {
            using (var serverManager = new ServerManager())
            {
                foreach (var application in serverManager.Sites.SelectMany(x => x.Applications))
                {
                    application["preloadEnabled"] = true;
                }

                foreach (var applicationPool in serverManager.ApplicationPools)
                {
                    applicationPool["startMode"] = "AlwaysRunning";
                    applicationPool.ProcessModel.IdleTimeout = new System.TimeSpan(0);
                }

                serverManager.CommitChanges();
            }

            base.Run();
        }
    }
}