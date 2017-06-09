using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using NLog;
using SFA.DAS.ReferenceData.Domain.Models.Education;

namespace SFA.DAS.ReferenceData.EducationOrgsImporter.WebJob.Serializer
{
    public class EducationalOrgainsationSerialiser : IEducationalOrgainsationSerialiser
    {
        private readonly ILogger _logger;

        public EducationalOrgainsationSerialiser(ILogger logger)
        {
            _logger = logger;
        }

        public byte[] SerialiseToJson(IEnumerable<EducationOrganisation> organisations)
        {
            try
            {
                using (var ms = new MemoryStream())
                {
                    using (var sw = new StreamWriter(ms))
                    {
                        using (var jw = new JsonTextWriter(sw))
                        {
                            jw.Formatting = Formatting.Indented;

                            var serializer = new JsonSerializer();
                            serializer.Serialize(jw, organisations);
                        }
                    }
                    return ms.ToArray();
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"An error occurred generating education orgainsation json data: {ex.Message}");
                throw new Exception($"An error occurred generating education orgainsation json data: {ex.Message}");
            }
        }
    }
}
