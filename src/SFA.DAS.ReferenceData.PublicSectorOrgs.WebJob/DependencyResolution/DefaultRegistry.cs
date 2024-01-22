using System.IO.Abstractions;
using SFA.DAS.NLog.Logger;
using SFA.DAS.ReferenceData.Domain.Interfaces.Caching;
using SFA.DAS.ReferenceData.Domain.Interfaces.Services;
using SFA.DAS.ReferenceData.Infrastructure.Caching;
using SFA.DAS.ReferenceData.Infrastructure.Services;
using SFA.DAS.ReferenceData.PublicSectorOrgs.WebJob.Updater;
using StructureMap;

namespace SFA.DAS.ReferenceData.PublicSectorOrgs.WebJob.DependencyResolution
{
    public class DefaultRegistry : Registry
    {
        public DefaultRegistry()
        {

            Scan(
               scan =>
               {
                   scan.AssembliesFromApplicationBaseDirectory(a => a.GetName().Name.StartsWith("SFA.DAS.ReferenceData"));
                   scan.RegisterConcreteTypesAgainstTheFirstInterface();
               });

            For<ICache>().Use<InMemoryCache>();
            For<IPublicOrgsUpdater>().Use<PublicOrgsUpdater>();
            For<INhsDataUpdater>().Use<NhsDataUpdater>();
            For<IFileSystem>().Use<FileSystem>();
            For<IJsonManager>().Use<JsonManager>();
            For<IPoliceDataLookupService>().Use<PoliceDataLookupService>();
            For<IPublicSectorOrganisationDatabaseUpdater>().Use<PublicSectorOrganisationDatabaseUpdater>();

            RegisterLogger();
        }

        private void RegisterLogger()
        {
            For<ILog>().Use(x => new NLogLogger(
                x.ParentType,
                null,
                null)).AlwaysUnique();
        }
    }
}
