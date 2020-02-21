using NUnit.Framework;

namespace SFA.DAS.ReferenceData.Application.UnitTests.Services.EducationalOrganisationsSearchServiceTests
{
    [TestFixture]
    public class WhenIProvideASearchTerm : EducationalOrganisationSearchServiceTestsBase
    {
        [Test]
        public void AndTheSearchTermIsAnIntegerThenItIsAReference()
        {
            var searchTerm = "123456";

            var result = Service.IsSearchTermAReference(searchTerm);

            Assert.IsTrue(result);
        }

        [Test]
        public void AndTheSearchTermIsNotAnIntegerThenItIsNotAReference()
        {
            var searchTerm = "123456a";

            var result = Service.IsSearchTermAReference(searchTerm);

            Assert.IsFalse(result);
        }
    }
}
