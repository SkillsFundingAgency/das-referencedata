using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.ReferenceData.Api.Client.Dto
{
    public class PublicSectorOrganisation
    {
        public string Name { get; set; }
        public DataSource Source { get; set; }
    }

    public enum DataSource
    {
        Ons = 1,
        Nhs = 2,
        Police = 3
    }
}
