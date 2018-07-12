using System.Collections.Generic;
using System.IO;
using CsvHelper;
using SFA.DAS.ReferenceData.Domain.Interfaces.Services;
using SFA.DAS.ReferenceData.Domain.Models;
using System.IO.Abstractions;
using SFA.DAS.ReferenceData.Api.Client.Dto;

namespace SFA.DAS.ReferenceData.Infrastructure.Services
{
    public class NhsCsvReaderHelper : INhsCsvReaderHelper
    {
        private readonly IFileSystem _fileSystem;

        public NhsCsvReaderHelper(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        public List<PublicSectorOrganisation> ReadNhsFile(string fileName)
        {
            var publicSectorOrganisationList = new List<PublicSectorOrganisation>();

            using (var stream = _fileSystem.FileInfo.FromFileName(fileName).Open(FileMode.Open, FileAccess.Read))
            {
                using (var reader = new StreamReader(stream))
                {
                    using (var csvReader = new CsvReader(reader))
                    {
                        csvReader.Configuration.HasHeaderRecord = false;
                        while (csvReader.Read())
                        {
                            if (CheckForEndOfFile(csvReader))
                            {
                                var organisation = new PublicSectorOrganisation
                                {
                                    Name = csvReader.GetField<string>(1),
                                    //OrganisationCode = code plus name - see AML-1590
                                    OrganisationCode = csvReader.GetField<string>(2) + csvReader.GetField<string>(1),
                                    AddressLine1 = csvReader.GetField<string>(4),
                                    AddressLine2 = csvReader.GetField<string>(5),
                                    AddressLine3 = csvReader.GetField<string>(6),
                                    AddressLine4 = csvReader.GetField<string>(7),
                                    AddressLine5 = csvReader.GetField<string>(8),
                                    PostCode = csvReader.GetField<string>(9),
                                    Source = DataSource.Nhs
                                };

                                publicSectorOrganisationList.Add(organisation);
                            }
                        }
                    }
                }
            }
                
            return publicSectorOrganisationList;
        }


        private bool CheckForEndOfFile(CsvReader csvReader)
        {
            //field 11 is apparently used to determine the end of the file. This is inferred from the existing code rather than directly from a requirement.
            //the name of this private method is therefore an assumption
            return string.IsNullOrEmpty(csvReader.GetField<string>(11));
        }
    }
}
