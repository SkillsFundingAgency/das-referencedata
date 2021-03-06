﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.NLog.Logger;
using SFA.DAS.ReferenceData.Domain.Interfaces.Services;
using SFA.DAS.ReferenceData.Domain.Models.Education;
using SFA.DAS.ReferenceData.EducationOrgsImporter.WebJob.Azure;
using SFA.DAS.ReferenceData.EducationOrgsImporter.WebJob.Serializer;

namespace SFA.DAS.ReferenceData.EducationOrgsImporter.WebJob.Updater
{
    public class EducationalOrgsUpdater : IEducationalOrgsUpdater
    {
        private readonly IEdubaseService _edubaseService;
        private readonly IEducationalOrgainsationSerialiser _serialiser;
        private readonly IAzureStorageUploader _uploader;
        private readonly ILog _logger;

        public EducationalOrgsUpdater(IEdubaseService edubaseService,
            IEducationalOrgainsationSerialiser serialiser, 
            IAzureStorageUploader uploader,
            ILog logger)
        {
            _edubaseService = edubaseService;
            _serialiser = serialiser;
            _uploader = uploader;
            _logger = logger;
        }

        public async Task RunUpdate()
        {
            try
            {
                var orgainsations = await _edubaseService.GetOrganisations();

                _logger.Info("Retrieved educational organisations", new Dictionary<string, object> { {"TotalCount", orgainsations.Count}});

                if (orgainsations == null || !orgainsations.Any()) return;

                var lookup = new EducationalOrganisationLookUp
                {
                    Organisations = orgainsations
                };

                var serialisedJsonData = _serialiser.SerialiseToJson(lookup);

                await _uploader.UploadDataToStorage(serialisedJsonData);
            }
            catch (Exception e)
            {
                _logger.Error(e, "Could not update educational organisations due to an error.");
            }
        }
    }
}
