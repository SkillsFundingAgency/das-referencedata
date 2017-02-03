using SFA.DAS.ReferenceData.Domain.Interfaces.Services;
using SFA.DAS.ReferenceData.Infrastructure.Services;
using SFA.DAS.ReferenceData.PublicSectorOrgs.WebJob.Updater;
using StructureMap;

namespace SFA.DAS.ReferenceData.PublicSectorOrgs.WebJob.DependencyResolution
{
    public class DefaultRegistry : Registry
    {
        public DefaultRegistry()
        {      
            For<IPublicOrgsUpdater>().Use<PublicOrgsUpdater>();
            For<IArchiveDownloadService>().Use<ArchiveDownloadService>();
        }
    }
}
