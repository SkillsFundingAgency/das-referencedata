using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using SFA.DAS.ReferenceData.Domain.Models;

namespace SFA.DAS.ReferenceData.Infrastructure.Extensions
{
    internal static class PublicSectorOrganisationExtensions
    {
        internal static void PopulateOrganisationCode(this PublicSectorOrganisation publicOrganisation)
        {
            var sb = new StringBuilder();
            var concatenatedBits = sb
                .Append(publicOrganisation.Name)
                .Append(publicOrganisation.AddressLine1)
                .Append(publicOrganisation.AddressLine2)
                .Append(publicOrganisation.AddressLine3)
                .Append(publicOrganisation.AddressLine4)
                .Append(publicOrganisation.AddressLine5)
                .Append(publicOrganisation.PostCode)
                .ToString();
            publicOrganisation.OrganisationCode = CalculateMd5Hash(concatenatedBits);
        }

        private static string CalculateMd5Hash(string concatenatedNameBits)
        {
            var md5 = MD5.Create();

            var hash = md5.ComputeHash(Encoding.ASCII.GetBytes(concatenatedNameBits));

            var sb = new StringBuilder();

            foreach (byte t in hash)
            {
                sb.Append(t.ToString());
            }

            return sb.ToString();
        }
    }
}
 