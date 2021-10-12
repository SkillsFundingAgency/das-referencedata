using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
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

        public CharityService(ILog logger, ICharityImportRepository charityImportRepository)
        {
            _logger = logger;
            _charityImportRepository = charityImportRepository;
        }

        public async Task ExecuteCharityImport(string sourceDirectory)
        {
            var files = Directory.EnumerateFiles(sourceDirectory, "*.json", SearchOption.AllDirectories).ToList();

            _logger.Info($"{files.Count} files found for import in {sourceDirectory}");

            if (!files.Any())
            {
                throw new InvalidOperationException("Import aborted - no files found in directory");
            }

            IEnumerable<CharityImport> charityImport;
            var totalStopwatch = Stopwatch.StartNew();
            try
            {   
                using (StreamReader r = new StreamReader(files[0]))
                {
                    var stopwatch = Stopwatch.StartNew();
                    _logger.Info($"Beginning Json import for {sourceDirectory}");
                    string json = r.ReadToEnd();
                    charityImport = JsonConvert.DeserializeObject<IEnumerable<CharityImport>>(json);
                    stopwatch.Stop();
                    _logger.Info($"Complete Json import for {sourceDirectory}: {stopwatch.Elapsed} elapsed");
                }

                await _charityImportRepository.ImportToStagingTable(charityImport);
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
