using System.Collections.Generic;
using SFA.DAS.ReferenceData.Domain.Models;
using SFA.DAS.ReferenceData.Types;

namespace SFA.DAS.ReferenceData.Domain.Interfaces.Services
{
    public interface INhsCsvReaderHelper
    {
        List<PublicSectorOrganisation> ReadNhsFile(string fileName);
    }
}