﻿using System.Linq;
using SFA.DAS.ReferenceData.Domain.Configuration;
using System;
using System.Threading.Tasks;
using SFA.DAS.ReferenceData.Domain.Interfaces.Services;
using System.IO;
using SFA.DAS.NLog.Logger;
using SFA.DAS.ReferenceData.Domain.Models;

namespace SFA.DAS.ReferenceData.PublicSectorOrgs.WebJob.Updater
{
    public class PublicOrgsUpdater : IPublicOrgsUpdater
    {
        private readonly IArchiveDownloadService _archiveDownloadService;
        private readonly INhsDataUpdater _nhsDataUpdater;
        private readonly IPublicSectorOrganisationDatabaseUpdater _publicSectorOrganisationDatabaseUpdater;
        private readonly IPoliceDataLookupService _policeDataLookupService;
        private readonly IJsonManager _jsonManager;
        private readonly ILog _logger;
        private readonly ReferenceDataApiConfiguration _configuration;
        private readonly string _workingFolder;
        private readonly string _fileName = "publicsectorclassificationguidelatest";
        private readonly string _jsonFileName = "PublicOrganisationNames.json";

        public PublicOrgsUpdater(ILog logger, ReferenceDataApiConfiguration configuration, 
            IArchiveDownloadService archiveDownloadService, INhsDataUpdater nhsDataUpdater, 
            IPublicSectorOrganisationDatabaseUpdater publicSectorOrganisationDatabaseUpdater,
            IPoliceDataLookupService policeDataLookupService, IJsonManager jsonManager)
        {
            _archiveDownloadService = archiveDownloadService;
            _nhsDataUpdater = nhsDataUpdater;
            _publicSectorOrganisationDatabaseUpdater = publicSectorOrganisationDatabaseUpdater;
            _policeDataLookupService = policeDataLookupService;
            _jsonManager = jsonManager;
            _logger = logger;
            _configuration = configuration;
            _workingFolder = Path.GetTempPath();
            _logger.Info($"Using temporary folder: {_workingFolder}");
        }

        public async Task RunUpdate()
        {
            try
            {
                _logger.Info("Running Public Organisations updater");

                if(!_configuration.NhsTrustsUrls.Any() || 
                    string.IsNullOrWhiteSpace(_configuration.PoliceForcesUrl) || 
                    string.IsNullOrWhiteSpace(_configuration.ONSUrl) ||
                    string.IsNullOrWhiteSpace(_configuration.OnsUrlDateFormat))
                {
                    const string errorMessage = "Missing configuration, check table storage configuration for NhsTrustsUrls, PoliceForcesUrl, ONSUrl and ONSUrlDateFormat";
                    _logger.Error(new Exception(errorMessage), errorMessage);
                    throw new Exception("Missing configuration, check table storage configuration for NhsTrustsUrls, PoliceForcesUrl, ONSUrl and ONSUrlDateFormat");
                }

                var onsOrgs = await GetOnsOrganisations();
                var nhsOrgs = await GetNhsOrganisations();
                var policeOrgs = await GetPoliceOrganisations();

                var orgs = new PublicSectorOrganisationLookUp
                {
                    Organisations = onsOrgs.Organisations
                                        .Concat(nhsOrgs.Organisations)
                                        .Concat(policeOrgs.Organisations)
                                        .ToList()
                };

                var jsonFilePath = Path.Combine(_workingFolder, _jsonFileName);
                _jsonManager.ExportFile(jsonFilePath, orgs);
                _jsonManager.UploadJsonToStorage(jsonFilePath);
            }
            catch(Exception ex)
            {
                _logger.Fatal(ex, $"The {_jsonFileName} has not been updated");
                throw;
            }
        }

        private async Task<PublicSectorOrganisationLookUp> GetOnsOrganisations()
        {
            var maxHistoricFileAttempts = 12; // change to go back 1 year. Last file without prior to change was June 2023
            var attempt = 0;
            var downloadSuccess = false;

            while(attempt < maxHistoricFileAttempts)
            {
                var url = GetDownloadUrlForMonthYear(attempt);
                _logger.Info($"Downloading ONS from {url}");

                downloadSuccess = await _archiveDownloadService.DownloadFile(url, _workingFolder, _fileName);

                if (downloadSuccess) break;
                attempt++;
            }

            if (!downloadSuccess)
            {
                const string errorMessage = "Failed to download ONS from current and previous month, potential URL format change";
                _logger.Error(new Exception(errorMessage), errorMessage);
                throw new Exception("Failed to download ONS from current and previous month, potential URL format change");
            }

            var excelFile = Path.Combine(_workingFolder, _fileName);

            _logger.Info($"Reading ONS from {excelFile}");
            var ol = _publicSectorOrganisationDatabaseUpdater.UpdateDatabase(excelFile);
            return ol;
        }

        private async Task<PublicSectorOrganisationLookUp> GetPoliceOrganisations()
        {
            _logger.Info($"Getting Police Organisations");
            var ol = await _policeDataLookupService.GetGbPoliceForces();

            return ol;
        }

        private async Task<PublicSectorOrganisationLookUp> GetNhsOrganisations()
        {
            var data = await _nhsDataUpdater.GetData();
            return data;
        }

        private string GetDownloadUrlForMonthYear(int minusMonths)
        {
            var urlpattern = _configuration.ONSUrl;
            var datePattern = _configuration.OnsUrlDateFormat;

            var now = DateTime.UtcNow.AddMonths(-minusMonths);
           
            var url = string.Format(urlpattern, now.ToString(datePattern).ToLower());
            return url;
        }
    }
}
