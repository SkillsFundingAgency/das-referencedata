using SFA.DAS.ReferenceData.Domain.Models.Charity;
using System.Collections.Generic;
using System.IO;

namespace SFA.DAS.ReferenceData.Domain.Interfaces.Services
{
    public interface IZipArchiveHelper
    {
        IEnumerable<CharityImport> ExtractModelFromJsonFileZipStream<T>(Stream stream, string filePath);
    }
}