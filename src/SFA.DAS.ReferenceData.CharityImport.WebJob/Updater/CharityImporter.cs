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
        private readonly ILogger _logger;

        public CharityImporter(ICharityRepository charityRepository)//, ILogger logger) //todo: put this back
        {
            _charityRepository = charityRepository;
            //_bcpService = bcpService;
            _bcpService = new BcpService(); //todo: remove this
            //_logger = logger;
        }

        public async Task RunUpdate()
        {
            //determine what file (date) to try for
            //download files
            //truncate load tables
            //bcp data in
            //transfer data from load tables to data tables
            //update next file (date) to try for

            //await _charityRepository.TruncateLoadTables();



            var bcp = new BcpRequest
            {
                ServerName = @"(localdb)\MSSQLLocalDB",
                UseTrustedConnection = true,
                Username = "",
                Password = "",
                TargetDb = "AngularSpa",
                TargetSchema = "import",
                TargetTable = "extract_aoo_ref",
                RowTerminator = "*@@*",
                FieldTerminator = "@**@",
                SourceFile = @"c:\temp\extract_aoo_ref.bcp"
            };
            //todo: get from config/azure storage

            await _bcpService.ExecuteBcp(bcp);

            throw new NotImplementedException("CharityImporter not implemented");
        }
    }
}
