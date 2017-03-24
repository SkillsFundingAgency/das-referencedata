using System.Collections.Generic;
using System.IO;
using CsvHelper;
using SFA.DAS.ReferenceData.Domain.Interfaces.Services;
using SFA.DAS.ReferenceData.Domain.Models;

namespace SFA.DAS.ReferenceData.Infrastructure.Services
{
    public class NhsCsvReaderHelper : INhsCsvReaderHelper
    {
        public List<PublicSectorOrganisation> ReadNhsFile(string fileName)
        {
            var publicSectorOrganisationList = new List<PublicSectorOrganisation>();

            using (var stream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            using (var reader = new StreamReader(stream))
            using (var csvReader = new CsvReader(reader))
            {
                while (csvReader.Read())
                {
                    if (string.IsNullOrEmpty(csvReader.GetField<string>(11)))
                    {
                        publicSectorOrganisationList.Add(new PublicSectorOrganisation
                        {
                            Name = csvReader.GetField<string>(1),
                            OrganisationCode = csvReader.GetField<string>(2),
                            AddressLine1 = csvReader.GetField<string>(4),
                            AddressLine2 = csvReader.GetField<string>(5),
                            AddressLine3 = csvReader.GetField<string>(6),
                            AddressLine4 = csvReader.GetField<string>(7),
                            AddressLine5 = csvReader.GetField<string>(8),
                            PostCode = csvReader.GetField<string>(9),
                            Source = DataSource.Nhs
                        });
                    }
                }

            }

            return publicSectorOrganisationList;
        }
    }
}
