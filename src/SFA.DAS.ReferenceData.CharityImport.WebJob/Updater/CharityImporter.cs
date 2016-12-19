using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using SFA.DAS.ReferenceData.Domain.Interfaces.Data;
using SFA.DAS.ReferenceData.Domain.Interfaces.Services;
using SFA.DAS.ReferenceData.Domain.Models.Bcp;
using SFA.DAS.ReferenceData.Infrastructure.Services;

namespace SFA.DAS.ReferenceData.CharityImport.WebJob.Updater
{
    public class CharityImporter : ICharityImporter
    {
        private readonly ICharityRepository _charityRepository;
        private readonly IBcpService _bcpService;
        private readonly IArchiveDownloadService _archiveDownloadService;
        private readonly ILogger _logger;

        public CharityImporter(ICharityRepository charityRepository, IArchiveDownloadService archiveDownloadService, IBcpService bcpService)//, ILogger logger) //todo: put this back
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
                importYear = lastImport.Year + 1;
                if (importMonth > 12)
                {
                    importMonth = 1;
                    importYear = importYear + 1;
                }
            }


            //determine what file (date) to try for
            //download files
            //truncate load tables
            //bcp data in
            //transfer data from load tables to data tables
            //update next file (date) to try for

            //await _charityRepository.TruncateLoadTables();

            var url = GetExtractUrlForMonthYear(importMonth, importYear);
            var filename = GetFilenameForMonthYear(importMonth, importYear);

            var downloadResult = await _archiveDownloadService.DownloadFile(
                url,
                @"c:\temp",
                filename);



            var extractResult = _archiveDownloadService.UnzipFile($"c:\\temp\\{filename}", @"c:\temp\");


            var bcp = new BcpRequest
            {
                ServerName = @"(localdb)\MSSQLLocalDB",
                UseTrustedConnection = true,
                Username = "",
                Password = "",
                TargetDb = "AngularSpa", //todo: read from config
                TargetSchema = "import",
                TargetTable = "extract_aoo_ref",
                RowTerminator = "*@@*",
                FieldTerminator = "@**@",
                SourceFile = @"c:\temp\extract_aoo_ref.bcp"
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
            var monthyear = $"{DateTime.Now.ToString("MMMM")}_{DateTime.Now.Year}"; 
                     
            return $"{rootUrl}/{dateNumericString}_2/extract1/RegPlusExtract_{monthyear}.zip";
        }

        private string GetFilenameForMonthYear(int month, int year)
        {
            return $"RegPlusExtract_{DateTime.Now.ToString("MMMM")}_{DateTime.Now.Year}.zip";
        }
    }
}
