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
        public string Sector { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string AddressLine4 { get; set; }
        public string AddressLine5 { get; set; }
        public string PostCode { get; set; }
        public string OrganisationCode { get; set; }
    }

    public enum DataSource
    {
        Ons = 1,
        Nhs = 2,
        Police = 3
    }
}
