using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using NLog;
using SFA.DAS.ReferenceData.Domain.Configuration;
using SFA.DAS.ReferenceData.Domain.Interfaces.Configuration;
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
        private readonly ILogger _logger;
        private readonly string _workingFolder;
        
        public CharityImporter(ReferenceDataApiConfiguration configuration, ICharityRepository charityRepository, IBcpService bcpService, IArchiveDownloadService archiveDownloadService, ILogger logger)
        {
            _configuration = configuration;
            _charityRepository = charityRepository;
            _bcpService = bcpService;
            _archiveDownloadService = archiveDownloadService;
            _logger = logger;
            //_workingFolder = Environment.GetEnvironmentVariable("TEMP");
            _workingFolder = Path.GetTempPath();
            _logger.Info($"Using temporary folder: {_workingFolder}");
        }

        public async Task RunUpdate()
        {
            _logger.Info("Executing CharityImporter");

            //Default to Nov 2016
            var importMonth = 11;
            var importYear = 2016;

            var lastImport = await _charityRepository.GetLastCharityDataImport();

            if (lastImport != null)
            {
                //Target subsequent month's file
                importMonth = lastImport.Month + 1;
                importYear = lastImport.Year;
                if (importMonth > 12)
                {
                    importMonth = 1;
                    importYear = importYear + 1;
                }
            }

            await _charityRepository.TruncateLoadTables();

            var url = GetExtractUrlForMonthYear(importMonth, importYear);
            var filename = GetFilenameForMonthYear(importMonth, importYear);

            if (!await _archiveDownloadService.DownloadFile(url, _workingFolder, filename))
            {
                _logger.Error($"Failed to download data from {url}");
                return;
            }

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

            //transfer data into data tables
            _logger.Info("Transferring data from load tables");
            await _charityRepository.ImportDataFromLoadTables();

            //record import in db
            _logger.Info("Recording successful import in database");
            await _charityRepository.CreateCharityDataImport(importMonth, importYear);
        }

        private string GetExtractUrlForMonthYear(int month, int year)
        {
            var urlpattern = _configuration.CharityDataSourceUrlPattern;

            var dateNumericString = $"{year}{month:D2}";
            var monthyear = $"{CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month)}_{year}";

            var url = String.Format(urlpattern, dateNumericString, monthyear);
            return url;
        }

        private string GetFilenameForMonthYear(int month, int year)
        {
            return $"RegPlusExtract_{CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month)}_{year}.zip";
        }
    }
}
