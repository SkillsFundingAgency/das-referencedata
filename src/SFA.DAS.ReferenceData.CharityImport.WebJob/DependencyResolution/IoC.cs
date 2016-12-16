using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StructureMap;

namespace SFA.DAS.ReferenceData.CharityImport.WebJob.DependencyResolution
{
    public static class IoC
    {
        private const string ServiceName = "SFA.DAS.EmployerApprenticeshipsService";

        public static IContainer Initialize()
        {
            return new Container(c =>
            {
                //c.Policies.Add(new ConfigurationPolicy<LevyDeclarationProviderConfiguration>("SFA.DAS.LevyAggregationProvider"));
                //c.Policies.Add(new ConfigurationPolicy<EmployerApprenticeshipsServiceConfiguration>(ServiceName));
                //c.Policies.Add(new ConfigurationPolicy<PaymentsApiClientConfiguration>("SFA.DAS.PaymentsAPI"));
                //c.Policies.Add<LoggingPolicy>();
                //c.Policies.Add(new MessagePolicy<EmployerApprenticeshipsServiceConfiguration>(ServiceName));
                c.AddRegistry<DefaultRegistry>();
            });
        }
    }
}
