using System.IO;
using SFA.DAS.ReferenceData.Domain.Interfaces.Services;
using System.IO.Compression;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using SFA.DAS.ReferenceData.Domain.Models.Charity;

namespace SFA.DAS.ReferenceData.Infrastructure.Services
{
    public class ZipArchiveHelper : IZipArchiveHelper
    {
        public IEnumerable<CharityImport> ExtractModelFromJsonFileZipStream<T>(Stream stream, string filePath)
        {

            using (var zip = new ZipArchive(stream, ZipArchiveMode.Read, true))
            {
                var entry = zip.Entries.FirstOrDefault(m => m.FullName.EndsWith(filePath));

                if (entry == null)
                {
                    return null;
                }

                JsonSerializer serializer = new JsonSerializer();
                using (var sr = new StreamReader(entry.Open()))
                using (JsonReader reader = new JsonTextReader(sr))
                {
                    return serializer.Deserialize<IEnumerable<CharityImport>>(reader);
                }
            }
        }
    }
}
