using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using SFA.DAS.ReferenceData.Domain.Interfaces.Data;
using SFA.DAS.ReferenceData.Domain.Interfaces.Services;
using SFA.DAS.ReferenceData.Domain.Models.Bcp;
using SFA.DAS.ReferenceData.Infrastructure.Data;
using SFA.DAS.ReferenceData.Infrastructure.Services;

namespace SFA.DAS.ReferenceData.CharityImport.WebJob.Updater
{
    public class CharityImporter : ICharityImporter
    {
        private readonly ICharityRepository _charityRepository;
        private readonly IBcpService _bcpService;
        private readonly IArchiveDownloadService _archiveDownloadService;
        private readonly ILogger _logger;

        //public CharityImporter(ICharityRepository charityRepository, IArchiveDownloadService archiveDownloadService, IBcpService bcpService)
        //{
        //    _charityRepository = charityRepository;
        //    _bcpService = bcpService;
        //    _archiveDownloadService = archiveDownloadService;
        //    //_logger = logger;
        //}

        public CharityImporter(ICharityRepository charityRepository, IBcpService bcpService, IArchiveDownloadService archiveDownloadService)
        {
            _charityRepository = charityRepository;
            _bcpService = bcpService;
            _archiveDownloadService = archiveDownloadService;
            //_logger = logger;
        }

        public async Task RunUpdate()
        {
            var importMonth = DateTime.Now.Month;
            var importYear = DateTime.Now.Year;

            var lastImport = await _charityRepository.GetLastCharityDataImport();

            if (lastImport != null)
            {
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

            var downloadResult = await _archiveDownloadService.DownloadFile(
                url,
                @"c:\temp",
                filename);

            if (!downloadResult)
            {
                //log error
                return;
            }


            var extractResult = _archiveDownloadService.UnzipFile($"c:\\temp\\{filename}", @"c:\temp\");

            if (!extractResult)
            {
                //log
                return;
            }


            var bcp = new BcpRequest
            {
                ServerName = @"(localdb)\MSSQLLocalDB",
                UseTrustedConnection = true,
                Username = "",
                Password = "",
                TargetDb = "AngularSpa", //todo: read from config
                TargetSchema = "import",
                RowTerminator = "*@@*",
                FieldTerminator = "@**@",
                SourceDirectory = @"c:\temp\" + Path.GetFileNameWithoutExtension(filename)
            };
            //todo: get from config/azure storage

            var bcpResult = _bcpService.ExecuteBcp(bcp);

            //record import in db
            //...
            await _charityRepository.CreateCharityDataImport(importMonth, importYear);

        }

        private string GetExtractUrlForMonthYear(int month, int year)
        {
            var rootUrl = @"http://apps.charitycommission.gov.uk/data";
            var dateNumericString = $"{year}{month}";
            var monthyear = $"{CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month)}_{year}"; 
                     
            return $"{rootUrl}/{dateNumericString}_2/extract1/RegPlusExtract_{monthyear}.zip";
        }

        private string GetFilenameForMonthYear(int month, int year)
        {
            return $"RegPlusExtract_{CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month)}_{year}.zip";
        }
    }
}
