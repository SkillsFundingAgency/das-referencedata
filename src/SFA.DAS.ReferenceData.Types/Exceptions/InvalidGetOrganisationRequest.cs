using SFA.DAS.ReferenceData.Types.DTO;

namespace SFA.DAS.ReferenceData.Types.Exceptions
{
    public class InvalidGetOrganisationRequest : ReferenceDataException
    {
        public InvalidGetOrganisationRequest()
        {
            // just call base    
        }

        public InvalidGetOrganisationRequest(string message) : base(message)
        {
            // just call base
        }
    }
}