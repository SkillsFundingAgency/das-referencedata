using SFA.DAS.ReferenceData.Api.Client.Dto;

namespace SFA.DAS.ReferenceData.Api.Client.Exceptions
{
    public class OperationNotSupportedForOrganisationType: ReferenceDataException
    {
        public OperationNotSupportedForOrganisationType(OrganisationType organisationType, string operation) :
            base($"Organisation type {organisationType} does not support {operation}")
        {
            OrganisationType = organisationType;
            Operation = operation;
        }

        public OrganisationType OrganisationType { get; }
        public string Operation { get; set; }
    }
}