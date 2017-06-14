using System;
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

        public byte[] SerialiseToJson(EducationalOrganisationLookUp organisations)
        {
            try
            {
                using (var stream = new MemoryStream())
                {
                    using (var writer = new StreamWriter(stream))
                    {
                        using (var jsonWriter = new JsonTextWriter(writer))
                        {
                            jsonWriter.Formatting = Formatting.Indented;

                            var serializer = new JsonSerializer();
                            serializer.Serialize(jsonWriter, organisations);
                        }
                    }
                    return stream.ToArray();
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
