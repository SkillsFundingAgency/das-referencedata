namespace SFA.DAS.ReferenceData.Types.Exceptions
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