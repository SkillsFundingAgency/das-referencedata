using System;
using SFA.DAS.ReferenceData.Api.Client.Dto.Adapters;

namespace SFA.DAS.ReferenceData.Api.Client.Dto
{
    /// <summary>
    ///     A generic organisation details used to de-serialise an <see cref="IOrganisationDetails"/> into.
    /// </summary>
    public class GenericOrganisationDetails : IOrganisationDetails
    {
        public OrganisationType OrganisationType { get; set; }
        public string Identifier { get; set; }
        public string Name { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string AddressLine4 { get; set; }
        public string AddressLine5 { get; set; }
        public string PostCode { get; set; }
    }
}