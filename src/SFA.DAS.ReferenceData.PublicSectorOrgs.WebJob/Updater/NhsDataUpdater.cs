using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using SFA.DAS.ReferenceData.Domain.Configuration;
using SFA.DAS.ReferenceData.Domain.Interfaces.Data;
using SFA.DAS.ReferenceData.Domain.Interfaces.Services;
using SFA.DAS.ReferenceData.Domain.Models;
using SFA.DAS.ReferenceData.Types.DTO;

namespace SFA.DAS.ReferenceData.PublicSectorOrgs.WebJob.Updater
{
    public class NhsDataUpdater : INhsDataUpdater
    {
        private readonly IArchiveDownloadService _archiveDownloadService;
        private readonly ReferenceDataApiConfiguration _configuration;
        private readonly INhsCsvReaderHelper _nhsCsvReaderHelper;
        private readonly IFileSystemRepository _fileSystemRepository;

        public NhsDataUpdater(IArchiveDownloadService archiveDownloadService, ReferenceDataApiConfiguration configuration, INhsCsvReaderHelper nhsCsvReaderHelper, IFileSystemRepository fileSystemRepository)
        {
            _archiveDownloadService = archiveDownloadService;
            _configuration = configuration;
            _nhsCsvReaderHelper = nhsCsvReaderHelper;
            _fileSystemRepository = fileSystemRepository;
        }

        public async Task<PublicSectorOrganisationLookUp> GetData()
        {
            var organisations = new List<PublicSectorOrganisation>();
            foreach (var nhsLink in _configuration.NhsTrustsUrls)
            {
                var fileDownloadSuccessful = await _archiveDownloadService.DownloadFile(nhsLink, Path.GetTempPath(), "nhsfile.zip");
                if (fileDownloadSuccessful)
                {
                    var nhsZipFolderPath = Path.Combine(Path.GetTempPath(), "NhsExtract");
                    _archiveDownloadService.UnzipFile(Path.Combine(Path.GetTempPath(), "nhsFile.zip"), nhsZipFolderPath);

                    var fileName = _fileSystemRepository.GetDataFile(nhsZipFolderPath);
                    if (!string.IsNullOrEmpty(fileName))
                    {
                        organisations.AddRange(_nhsCsvReaderHelper.ReadNhsFile(fileName));
                    }
                }
            }

            return new PublicSectorOrganisationLookUp { Organisations = organisations };
        }
    }
}
