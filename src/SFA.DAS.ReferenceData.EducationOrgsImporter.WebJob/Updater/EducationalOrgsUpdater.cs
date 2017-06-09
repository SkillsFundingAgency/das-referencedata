using System;
using System.Threading.Tasks;
using SFA.DAS.ReferenceData.Domain.Models;

namespace SFA.DAS.ReferenceData.EducationOrgsImporter.WebJob.Updater
{
    public class EducationalOrgsUpdater : IEducationalOrgsUpdater
    {
        public EducationalOrgsUpdater()
        {
           
        }

        public async Task<PublicSectorOrganisationLookUp> GetData()
        {
            throw new NotImplementedException();
        }
    }
}
