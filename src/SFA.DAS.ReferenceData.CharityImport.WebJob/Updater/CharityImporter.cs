using System;
using System.Threading.Tasks;
using SFA.DAS.NLog.Logger;
using SFA.DAS.ReferenceData.Domain.Configuration;
using SFA.DAS.ReferenceData.Domain.Interfaces.Data;
using SFA.DAS.ReferenceData.Domain.Interfaces.Services;

namespace SFA.DAS.ReferenceData.CharityImport.WebJob.Updater
{
    public class CharityImporter : ICharityImporter
    {
        private readonly ReferenceDataApiConfiguration _configuration;
        private readonly ICharityRepository _charityRepository;
        private readonly ICharityService _charityService;        
        private readonly IDataDownloadService _dataDownloadService;
        private readonly ILog _logger;        

        public CharityImporter(ReferenceDataApiConfiguration configuration, 
            ICharityRepository charityRepository,
            ICharityService charityService,            
            IDataDownloadService dataDownloadService,
            ILog logger)
        {
            _configuration = configuration;
            _charityRepository = charityRepository; 
            _charityService = charityService;            
            _dataDownloadService = dataDownloadService;
            _logger = logger;
        }

        public async Task RunUpdate()
        {
            try
            {
                _logger.Info("Executing CharityImporter");

                await _charityRepository.TruncateLoadTables();

                var content = await _dataDownloadService.GetFileStream(_configuration.CharityDataSourceUrl);
                await _charityService.ExecuteCharityImport(content);

                _logger.Info("Transferring data from public extract charity table into staging tables");
                await _charityRepository.ImportFromPublicExtractCharityToStagingTable();

                // transfer data into data tables
                _logger.Info("Transferring data from load tables");
                await _charityRepository.ImportDataFromLoadTables();

                // record import in db
                _logger.Info("Recording successful import in database");
                await _charityRepository.CreateCharityDataImport(DateTime.UtcNow.Month, DateTime.UtcNow.Year);
            }
            catch(Exception ex)
            {
                _logger.Error(ex, $"charity import - an error occurred while trying to import charity data {_configuration.CharityDataSourceUrl}");
                throw;
            }
        }        
    }
}
