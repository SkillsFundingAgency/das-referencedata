using SFA.DAS.ReferenceData.Types;

namespace SFA.DAS.ReferenceData.Api.Client.Exceptions
{
    public class OrganisationNotFoundExeption : ReferenceDataException
    {
        public OrganisationNotFoundExeption(OrganisationType organisationType, string identifier) :
            base($"Did not find an organisation type {organisationType} with identifier {identifier}")
        {
            OrganisationType = organisationType;
            Identifier = identifier;
        }
        public OrganisationType OrganisationType { get; }
        public string Identifier { get; }
    }
}