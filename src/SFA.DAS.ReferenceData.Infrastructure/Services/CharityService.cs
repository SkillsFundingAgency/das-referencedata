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
using SFA.DAS.ReferenceData.Infrastructure.Data;

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

            IEnumerable<CharityImport> charityImport = new List<CharityImport>();
            var charityData = new List<ExistingCharity>();
            var charityRegistrations = new List<ExistingCharityRegistration>();
            var mainCharity = new List<ExistingMainCharity>();

            var totalStopwatch = Stopwatch.StartNew();

            try
            {   
                using (StreamReader r = new StreamReader(files[0]))
                {
                    var stopwatch = Stopwatch.StartNew();
                    _logger.Info($"Beginning Json import for {sourceDirectory}");
                    string json = r.ReadToEnd();
                    charityImport = JsonConvert.DeserializeObject<IEnumerable<CharityImport>>(json); // TODO : change to response CharityExtractResponse
                    stopwatch.Stop();
                    _logger.Info($"Complete Json import  for {sourceDirectory}: {stopwatch.Elapsed} elapsed");
                }
                /*foreach (var item in charityImport)
                {
                    charityData.Add(new ExistingCharity
                    {
                        add1 = item.CharityContactAddress1,
                        add2 = item.CharityContactAddress2,
                        add3 = item.CharityContactAddress3,
                        add4 = item.CharityContactAddress4,
                        add5 = item.CharityContactAddress5,
                        name = item.CharityName,
                        phone = item.CharityContactPhone,
                        postcode = item.CharityContactPostcode,
                        regno = item.RegisteredCharityNumber,
                        subno = item.LinkedCharityNumber,
                        orgtype = item.CharityType,
                        ha_no = 0,
                        aob_defined = 0,
                        fax = 0,
                        aob = string.Empty,// need to find
                        corr = string.Empty,// need to find
                        gd = string.Empty,// need to find
                        nhs = string.Empty,// need to find                       
                    });

                    charityRegistrations.Add(new ExistingCharityRegistration
                    {
                        regno = item.RegisteredCharityNumber,
                        //subno = 0, // TODO : need to find
                        regdate = item.DateOfRegistration,
                        remdate = item.DateOfRemoval,
                        //remcode  // TODO : need to find
                    });

                    mainCharity.Add(new ExistingMainCharity
                    {
                        regno = item.RegisteredCharityNumber,
                        coyno = item.CharityCompanyRegistrationNumber,
                        fyend = item.LatestAccFinPeriodEndDate?.ToString("ddMM"),
                        email = item.CharityContactEmail,
                        web = item.CharityContactWeb,
                        income = item.LatestIncome,
                        incomedate = item.LatestAccFinPeriodEndDate,
                        welsh = "F",//TODO : need to find
                        trustees = "F",
                        //grouptype  //TODO : need to find
                    });
                }*/
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Charity Json Import Error");
            }
            
            await _charityImportRepository.ImportToStagingTable(charityData, charityRegistrations, mainCharity, charityImport);

            totalStopwatch.Stop();
            _logger.Info($"Charity Json import complete for all files: {totalStopwatch.Elapsed} elapsed");
        }

    }
   
}
