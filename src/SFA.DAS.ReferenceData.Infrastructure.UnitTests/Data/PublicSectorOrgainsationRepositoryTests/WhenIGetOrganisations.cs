using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NLog;
using NUnit.Framework;
using SFA.DAS.ReferenceData.Domain.Interfaces.Services;
using SFA.DAS.ReferenceData.Domain.Models;
using SFA.DAS.ReferenceData.Infrastructure.Caching;
using SFA.DAS.ReferenceData.Infrastructure.Data;

namespace SFA.DAS.ReferenceData.Infrastructure.UnitTests.Data.PublicSectorOrgainsationRepositoryTests
{
    class WhenIGetOrganisations
    {
        private Mock<IAzureService> _azureService;
        private PublicSectorOrganisationRepository _repository;
        private Mock<ICacheProvider> _cacheProvider;
        private PublicSectorOrganisationLookUp _lookup;
        private Mock<ILogger> _logger;

        [SetUp]
        public void Arrange()
        {
            _lookup = new PublicSectorOrganisationLookUp
            {
                OrganisationNames = new List<string>
                {
                    "Test 1",
                    "Test 2",
                    "Test 3",
                    "Example 1",
                    "Example 2",
                    "Example 3"
                }
            };

            _azureService = new Mock<IAzureService>();

            _azureService.Setup(x => x.GetModelFromBlobStorage<PublicSectorOrganisationLookUp>(
                It.IsAny<string>(), 
                It.IsAny<string>()))
                         .ReturnsAsync(_lookup);

            _cacheProvider = new Mock<ICacheProvider>();

            _logger = new Mock<ILogger>();

            _repository = new PublicSectorOrganisationRepository(_cacheProvider.Object, _azureService.Object, _logger.Object);
        }

        [Test]
        public async Task ThenIShouldGetOrganisationFromAzureIfNotInCache()
        {
            //Arrange
            _cacheProvider.Setup(x => x.Get<PublicSectorOrganisationLookUp>(It.IsAny<string>()))
                          .Returns((PublicSectorOrganisationLookUp) null);

            //Act
            var result = await _repository.FindOrganisations("", 1000, 0);

            //Assert
            Assert.IsNotEmpty(result);

            _cacheProvider.Verify(x => x.Get<PublicSectorOrganisationLookUp>(
               nameof(PublicSectorOrganisationLookUp)), Times.Once);

            _azureService.Verify(x => x.GetModelFromBlobStorage<PublicSectorOrganisationLookUp>(
                "sfa-das-reference-data",
                "PublicOrganisationNames.json"), Times.Once);
        }

        [Test]
        public async Task ThenIShouldGetOrganisationFromCacheIfAvailable()
        {
            //Arrange
            _cacheProvider.Setup(x => x.Get<PublicSectorOrganisationLookUp>(It.IsAny<string>()))
                          .Returns(_lookup);

            //Act
            var result = await _repository.FindOrganisations("", 1000, 0);

            //Assert
            Assert.IsNotEmpty(result);

            _cacheProvider.Verify(x => x.Get<PublicSectorOrganisationLookUp>(
               nameof(PublicSectorOrganisationLookUp)), Times.Once);

            _azureService.Verify(x => x.GetModelFromBlobStorage<PublicSectorOrganisationLookUp>(
                It.IsAny<string>(),
                 It.IsAny<string>()), Times.Never);
        }

        [Test]
        public async Task ThenIfNullIsReturnedFromTheAzureServiceAnEmptyCollectionShouldBeReturned()
        {
            //Arrange
            _cacheProvider.Setup(x => x.Get<PublicSectorOrganisationLookUp>(It.IsAny<string>()))
                          .Returns((PublicSectorOrganisationLookUp)null);

            _azureService.Setup(x => x.GetModelFromBlobStorage<PublicSectorOrganisationLookUp>(
               It.IsAny<string>(),
               It.IsAny<string>()))
                        .ReturnsAsync((PublicSectorOrganisationLookUp)null);

            //Act
            var result = await _repository.FindOrganisations("", 1000, 0);

            //Assert
            Assert.IsEmpty(result);
        }

        [Test]
        public async Task ThenShouldReturnOnlyResultsThatMatchSearchTerm()
        {
            //Arrange
            _cacheProvider.Setup(x => x.Get<PublicSectorOrganisationLookUp>(It.IsAny<string>()))
                          .Returns(_lookup);

            //Act
            var result = await _repository.FindOrganisations("test", 10, 0);

            //Assert
            Assert.AreEqual(3, result.Count);

            Assert.IsTrue(result.Any(x => x.Name.Equals("Test 1")));
            Assert.IsTrue(result.Any(x => x.Name.Equals("Test 2")));
            Assert.IsTrue(result.Any(x => x.Name.Equals("Test 3")));
        }

        [Test]
        public async Task ThenShouldReturnOnlyTheNumberOfResultsSentAsThePageSize()
        {
            //Arrange
            _cacheProvider.Setup(x => x.Get<PublicSectorOrganisationLookUp>(It.IsAny<string>()))
                          .Returns(_lookup);

            //Act
            var result = await _repository.FindOrganisations("test", 2, 0);

            //Assert
            Assert.AreEqual(2, result.Count);

            Assert.IsTrue(result.Any(x => x.Name.Equals("Test 1")));
            Assert.IsTrue(result.Any(x => x.Name.Equals("Test 2")));
        }

        [Test]
        public async Task ThenShouldReturnTheAFullPageOfTheLastResultsIfTheLastPageIsSelected()
        {
            //Arrange
            _cacheProvider.Setup(x => x.Get<PublicSectorOrganisationLookUp>(It.IsAny<string>()))
                          .Returns(_lookup);

            //Act
            var result = await _repository.FindOrganisations("test", 2, 2);

            //Assert
            Assert.AreEqual(2, result.Count);

            Assert.IsTrue(result.Any(x => x.Name.Equals("Test 2")));
            Assert.IsTrue(result.Any(x => x.Name.Equals("Test 3")));
        }

        [Test]
        public async Task ThenShouldReturnTheFirstPageIfThePageNumberIsLessThanOne()
        {
            //Arrange
            _cacheProvider.Setup(x => x.Get<PublicSectorOrganisationLookUp>(It.IsAny<string>()))
                          .Returns(_lookup);

            //Act
            var result = await _repository.FindOrganisations("test", 2, -2);

            //Assert
            Assert.AreEqual(2, result.Count);

            Assert.IsTrue(result.Any(x => x.Name.Equals("Test 1")));
            Assert.IsTrue(result.Any(x => x.Name.Equals("Test 2")));
        }

        [Test]
        public async Task ThenIfThePageSizeIsLessThanOneThePageSizeShouldBeSetToOne()
        {
            //Arrange
            _cacheProvider.Setup(x => x.Get<PublicSectorOrganisationLookUp>(It.IsAny<string>()))
                          .Returns(_lookup);

            //Act
            var result = await _repository.FindOrganisations("test", 0, -2);

            //Assert
            Assert.AreEqual(1, result.Count);

            Assert.IsTrue(result.Any(x => x.Name.Equals("Test 1")));
        }
    }
}
