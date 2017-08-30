using SFA.DAS.ReferenceData.Domain.Models;

namespace SFA.DAS.ReferenceData.Infrastructure.Extensions
{
    internal static class PublicSectorOrganisationExtensions
    {
        internal static void PopulateOrganisationCode(this PublicSectorOrganisation publicOrganisation)
        {
            publicOrganisation.OrganisationCode = publicOrganisation.Name;
        }
    }
}
 