using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using SFA.DAS.ReferenceData.Domain.Interfaces.Data;

namespace SFA.DAS.ReferenceData.CharityImport.WebJob.Updater
{
    public class CharityImporter : ICharityImporter
    {
        private readonly ICharityRepository _charityRepository;
        private readonly ILogger _logger;

        public CharityImporter(ICharityRepository charityRepository, ILogger logger)
        {
            _charityRepository = charityRepository;
            _logger = logger;
        }

        public async Task RunUpdate()
        {
            //determine what file (date) to try for
            //download files
            //truncate load tables
            //bcp data in
            //transfer data from load tables to data tables
            //update next file (date) to try for

            await _charityRepository.TruncateLoadTables();

            throw new NotImplementedException("CharityImporter not implemented");
        }
    }
}
