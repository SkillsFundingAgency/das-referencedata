using Moq;
using NUnit.Framework;
using SFA.DAS.ReferenceData.Application.Services.OrganisationSearch;
using SFA.DAS.ReferenceData.Domain.Interfaces.Data;

namespace SFA.DAS.ReferenceData.Application.UnitTests.Services.CharitiesSearchServiceTests
{
    public abstract class CharitiesSearchServiceTestsBase
    {
        protected Mock<ICharityRepository> Repository;
        protected CharitiesSearchService Service;

        [SetUp]
        public void Arrange()
        {
            Repository = new Mock<ICharityRepository>();
            Service = new CharitiesSearchService(Repository.Object);
        }
    }
}
