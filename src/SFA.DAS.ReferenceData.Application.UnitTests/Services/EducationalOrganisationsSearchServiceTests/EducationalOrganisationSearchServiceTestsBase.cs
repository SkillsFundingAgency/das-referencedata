using Moq;
using NUnit.Framework;
using SFA.DAS.ReferenceData.Application.Services.OrganisationSearch;
using SFA.DAS.ReferenceData.Domain.Interfaces.Data;

namespace SFA.DAS.ReferenceData.Application.UnitTests.Services.EducationalOrganisationsSearchServiceTests
{
    public abstract class EducationalOrganisationSearchServiceTestsBase
    {
        protected Mock<IEducationalOrganisationRepository> Repository;
        protected EducationalOrganisationSearchService Service;

        [SetUp]
        public void Arrange()
        {
            Repository = new Mock<IEducationalOrganisationRepository>();
            Service = new EducationalOrganisationSearchService(Repository.Object);
        }
    }
}
