using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using SFA.DAS.NLog.Logger;
using SFA.DAS.ReferenceData.Domain.Configuration;
using SFA.DAS.ReferenceData.Domain.Interfaces.Data;
using SFA.DAS.ReferenceData.Domain.Interfaces.Services;
using SFA.DAS.ReferenceData.Domain.Models.Bcp;

namespace SFA.DAS.ReferenceData.CharityImport.WebJob.Updater
{
    public class CharityImporter : ICharityImporter
    {
        private readonly ReferenceDataApiConfiguration _configuration;
        private readonly ICharityRepository _charityRepository;
        private readonly IBcpService _bcpService;
        private readonly IArchiveDownloadService _archiveDownloadService;
        private readonly ILog _logger;
        private readonly string _workingFolder;

        public CharityImporter(ReferenceDataApiConfiguration configuration, ICharityRepository charityRepository, IBcpService bcpService, IArchiveDownloadService archiveDownloadService, ILog logger)
        {
            _configuration = configuration;
            _charityRepository = charityRepository;
            _bcpService = bcpService;
            _archiveDownloadService = archiveDownloadService;
            _logger = logger;
            _workingFolder = Path.GetTempPath();
            _logger.Info($"Using temporary folder: {_workingFolder}");
        }

        public async Task RunUpdate()
        {
            _logger.Info("Executing CharityImporter");

            var lastImport = await _charityRepository.GetLastCharityDataImport();
            
            // start at May 2017 which will import June 2017 when no previous import exists
            var importInfo = await SearchForDownload(lastImport != null 
                ? new DateTime(lastImport.Year, lastImport.Month, 1) 
                : new DateTime(2017, 5, 1));
            
            var filename = importInfo.Item1;
            var importDate = importInfo.Item2;
            
            if (string.IsNullOrEmpty(filename))
                return;

            await _charityRepository.TruncateLoadTables();

            var zipFile = Path.Combine(_workingFolder, filename);
            var extractPath = Path.Combine(_workingFolder, Path.GetFileNameWithoutExtension(filename));

            _archiveDownloadService.UnzipFile(zipFile, extractPath);

            _bcpService.ExecuteBcp(new BcpRequest
            {
                ServerName = _configuration.CharityBcpServerName,
                UseTrustedConnection = _configuration.CharityBcpTrustedConnection,
                Username = _configuration.CharityBcpUsername,
                Password = _configuration.CharityBcpPassword,
                TargetDb = _configuration.CharityBcpTargetDb,
                TargetSchema = _configuration.CharityBcpTargetSchema,
                RowTerminator = _configuration.CharityBcpRowTerminator,
                FieldTerminator = _configuration.CharityBcpFieldTerminator,
                SourceDirectory = _workingFolder + Path.GetFileNameWithoutExtension(filename)
            });

            // transfer data into data tables
            _logger.Info("Transferring data from load tables");
            await _charityRepository.ImportDataFromLoadTables();

            // record import in db
            _logger.Info("Recording successful import in database");
            await _charityRepository.CreateCharityDataImport(importDate.Month, importDate.Year);
        }

        private async Task<(string, DateTime)> SearchForDownload(DateTime lastImportDate)
        {
            // search for next months file
            var nextImportDate = lastImportDate.AddMonths(1);
            if (nextImportDate > DateTime.Now)
                return (null, nextImportDate);
            
            var url = GetExtractUrlForMonthYear(nextImportDate.Month, nextImportDate.Year);
            var filename = GetFilenameForMonthYear(nextImportDate.Month, nextImportDate.Year);

            if (!await _archiveDownloadService.DownloadFile(url, _workingFolder, filename))
            {
                _logger.Error(new Exception($"Failed to download data from {url}"), $"Failed to download data from {url}");
                return await SearchForDownload(nextImportDate);
            }

            return (filename, nextImportDate);
        }

        private string GetExtractUrlForMonthYear(int month, int year)
        {
            var urlpattern = _configuration.CharityDataSourceUrlPattern;

            var monthyear = $"{CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month)}_{year}";

            var url = string.Format(urlpattern, monthyear);
            return url;
        }

        private static string GetFilenameForMonthYear(int month, int year)
        {
            return $"RegPlusExtract_{CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month)}_{year}.zip";
        }
    }
}
