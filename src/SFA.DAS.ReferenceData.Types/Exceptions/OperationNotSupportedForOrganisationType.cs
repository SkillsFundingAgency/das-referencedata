using SFA.DAS.Common.Domain.Types;
using SFA.DAS.ReferenceData.Types.DTO;

namespace SFA.DAS.ReferenceData.Types.Exceptions
{
    public class OperationNotSupportedForOrganisationTypeException: InvalidGetOrganisationRequest
    {
        public OperationNotSupportedForOrganisationTypeException()
        {
            // just call base    
        }

        public OperationNotSupportedForOrganisationTypeException(string message) : base(message)
        {
            // just call base
        }

        public OperationNotSupportedForOrganisationTypeException(OrganisationType organisationType, string operation) :
            base($"Organisation type {organisationType} does not support {operation}")
        {
            OrganisationType = organisationType;
            Operation = operation;
        }

        public OrganisationType OrganisationType { get; }
        public string Operation { get; set; }
    }
}