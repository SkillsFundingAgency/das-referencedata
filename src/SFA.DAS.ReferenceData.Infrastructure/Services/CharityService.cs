using System;
using System.Diagnostics;
using System.IO;
using SFA.DAS.NLog.Logger;
using SFA.DAS.ReferenceData.Domain.Interfaces.Services;
using System.Threading.Tasks;
using SFA.DAS.ReferenceData.Domain.Models.Charity;
using SFA.DAS.ReferenceData.Domain.Interfaces.Data;

namespace SFA.DAS.ReferenceData.Infrastructure.Services
{
    public class CharityService : ICharityService
    {
        private readonly ILog _logger;
        private readonly ICharityImportRepository _charityImportRepository;
        private readonly IZipArchiveHelper _zipArchiveHelper;

        public CharityService(ILog logger, ICharityImportRepository charityImportRepository, IZipArchiveHelper zipArchiveHelper)
        {
            _logger = logger;
            _charityImportRepository = charityImportRepository;
            _zipArchiveHelper = zipArchiveHelper;
        }
        
        public async Task ExecuteCharityImport(Stream content)
        {
            var totalStopwatch = Stopwatch.StartNew();
            try
            {
                var charityJson = _zipArchiveHelper.ExtractModelFromJsonFileZipStream<CharityImport>(content, "publicextract.charity.json");              
                await _charityImportRepository.ImportToStagingTable(charityJson);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Charity Json Import Error");
            }

            totalStopwatch.Stop();
            _logger.Info($"Charity Json import complete for all files: {totalStopwatch.Elapsed} elapsed");
        }
    }   
}
