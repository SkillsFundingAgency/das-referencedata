using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.ReferenceData.PublicSectorOrgs.WebJob.Models
{
    public class OrganisationList
    {
        public OrganisationList()
        {
            Organisations = new List<Organisation>();
        }

        public List<Organisation> Organisations { get; set; }
    }
}
