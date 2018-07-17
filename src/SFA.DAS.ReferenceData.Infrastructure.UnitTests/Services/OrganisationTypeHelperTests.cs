using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using SFA.DAS.ReferenceData.Domain.Interfaces.Services;
using SFA.DAS.ReferenceData.Infrastructure.Services;
using SFA.DAS.ReferenceData.Types.DTO;
using SFA.DAS.ReferenceData.Types.Exceptions;

namespace SFA.DAS.ReferenceData.Infrastructure.UnitTests.Services
{
    [TestFixture]
    public class OrganisationTypeHelperTests
    {
        [TestCase()]
        [TestCase(OrganisationType.Company)]
        [TestCase(OrganisationType.Company, OrganisationType.Charity)]
        public void Constructor_NoSearchServices_ShouldNotThrowException(params OrganisationType[] organisationTypes)
        {
            OrganisationTypeHelper oth = new OrganisationTypeHelper(Create(organisationTypes));
        }

        [Test]
        public void Constructor_Duplicates_ShouldThrowExpctedException()
        {
            Assert.Throws<ReferenceDataException>(() =>
            {
                new OrganisationTypeHelper(Create(OrganisationType.Charity, OrganisationType.Charity));
            });
        }

        [TestCase(OrganisationType.Company, false)]
        [TestCase(OrganisationType.Company, false, OrganisationType.Charity)]
        [TestCase(OrganisationType.Company, false, OrganisationType.Charity, OrganisationType.PublicSector)]
        [TestCase(OrganisationType.Company, true, OrganisationType.Company)]
        [TestCase(OrganisationType.Company, true, OrganisationType.Charity, OrganisationType.Company)]
        [TestCase(OrganisationType.Company, true, OrganisationType.Charity, OrganisationType.Company, OrganisationType.PublicSector)]
        public void TryGetReferenceSearcher_SpecifiedOrganisationType_ShouldFindIfReferenceSearchDefined(OrganisationType lookForOrganisationType, bool expectToFind, params OrganisationType[] createOrganisationTypes)
        {
            OrganisationTypeHelper oth = new OrganisationTypeHelper(Create(createOrganisationTypes));

            var actualResult = oth.TryGetReferenceSearcher(lookForOrganisationType, out IOrganisationReferenceSearchService foundReferenceSearchService);

            Assert.AreEqual(expectToFind, actualResult);

            if (expectToFind)
            {
                Assert.AreEqual(lookForOrganisationType, foundReferenceSearchService.OrganisationType);
            }
        }

        private IEnumerable<IOrganisationReferenceSearchService> Create(params OrganisationType[] organisationTypes)
        {
            foreach (var organisationType in organisationTypes)
            {
                var mock = new Mock<IOrganisationReferenceSearchService>();
                mock.Setup(orss => orss.OrganisationType)
                    .Returns(organisationType);

                yield return mock.Object;
            }
        }
    }
}
