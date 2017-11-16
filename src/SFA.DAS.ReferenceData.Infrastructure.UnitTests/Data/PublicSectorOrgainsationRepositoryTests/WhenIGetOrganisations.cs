using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NLog;
using NUnit.Framework;
using SFA.DAS.NLog.Logger;
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
        private Mock<ILog> _logger;

        [SetUp]
        public void Arrange()
        {
            _lookup = new PublicSectorOrganisationLookUp
            {
                Organisations = new List<PublicSectorOrganisation>
                {
                    new PublicSectorOrganisation {Name = "Test 1", Source = DataSource.Ons},
                    new PublicSectorOrganisation {Name = "Test 2", Source = DataSource.Ons},
                    new PublicSectorOrganisation {Name = "Test 3", Source = DataSource.Ons},
                    new PublicSectorOrganisation {Name = "Example 1", Source = DataSource.Ons},
                    new PublicSectorOrganisation {Name = "Example 2", Source = DataSource.Ons},
                    new PublicSectorOrganisation {Name = "Example 3", Source = DataSource.Ons}
                }
            };

            _azureService = new Mock<IAzureService>();

            _azureService.Setup(x => x.GetModelFromBlobStorage<PublicSectorOrganisationLookUp>(
                It.IsAny<string>(), 
                It.IsAny<string>()))
                         .ReturnsAsync(_lookup);

            _cacheProvider = new Mock<ICacheProvider>();

            _logger = new Mock<ILog>();

            _repository = new PublicSectorOrganisationRepository(_cacheProvider.Object, _azureService.Object, _logger.Object);
        }

        [Test]
        public async Task ThenIShouldGetOrganisationFromAzureIfNotInCache()
        {
            //Arrange
            _cacheProvider.SetupSequence(x => x.GetAsync<PublicSectorOrganisationLookUp>(It.IsAny<string>()))
                          .ReturnsAsync(null)
                          .ReturnsAsync(_lookup);

            //Act
            var result = await _repository.FindOrganisations("", 1000, 0);

            //Assert
            Assert.IsNotEmpty(result.Data);

            _cacheProvider.Verify(x => x.GetAsync<PublicSectorOrganisationLookUp>(
               nameof(PublicSectorOrganisationLookUp)), Times.Exactly(2));

            _azureService.Verify(x => x.GetModelFromBlobStorage<PublicSectorOrganisationLookUp>(
                "sfa-das-reference-data",
                "PublicOrganisationNames.json"), Times.Once);
        }

        [Test]
        public async Task ThenIShouldGetOrganisationFromCacheIfAvailable()
        {
            //Arrange
            _cacheProvider.Setup(x => x.GetAsync<PublicSectorOrganisationLookUp>(It.IsAny<string>()))
                          .ReturnsAsync(_lookup);

            //Act
            var result = await _repository.FindOrganisations("", 1000, 0);

            //Assert
            Assert.IsNotEmpty(result.Data);

            _cacheProvider.Verify(x => x.GetAsync<PublicSectorOrganisationLookUp>(
               nameof(PublicSectorOrganisationLookUp)), Times.Once);

            _azureService.Verify(x => x.GetModelFromBlobStorage<PublicSectorOrganisationLookUp>(
                It.IsAny<string>(),
                 It.IsAny<string>()), Times.Never);
        }

        [Test]
        public async Task ThenIfNullIsReturnedFromTheAzureServiceAnEmptyCollectionShouldBeReturned()
        {
            //Arrange
            _cacheProvider.Setup(x => x.GetAsync<PublicSectorOrganisationLookUp>(It.IsAny<string>()))
                          .ReturnsAsync((PublicSectorOrganisationLookUp)null);

            _azureService.Setup(x => x.GetModelFromBlobStorage<PublicSectorOrganisationLookUp>(
               It.IsAny<string>(),
               It.IsAny<string>()))
                        .ReturnsAsync((PublicSectorOrganisationLookUp)null);

            //Act
            var result = await _repository.FindOrganisations("", 1000, 0);

            //Assert
            Assert.IsEmpty(result.Data);
        }

        [Test]
        public async Task ThenShouldReturnOnlyResultsThatMatchSearchTerm()
        {
            //Arrange
            _cacheProvider.Setup(x => x.GetAsync<PublicSectorOrganisationLookUp>(It.IsAny<string>()))
                          .ReturnsAsync(_lookup);

            //Act
            var result = await _repository.FindOrganisations("test", 10, 0);

            //Assert
            Assert.AreEqual(3, result.Data.Count);

            Assert.IsTrue(result.Data.Any(x => x.Name.Equals("Test 1")));
            Assert.IsTrue(result.Data.Any(x => x.Name.Equals("Test 2")));
            Assert.IsTrue(result.Data.Any(x => x.Name.Equals("Test 3")));
        }

        [Test]
        public async Task ThenShouldReturnOnlyTheNumberOfResultsSentAsThePageSize()
        {
            //Arrange
            _cacheProvider.Setup(x => x.GetAsync<PublicSectorOrganisationLookUp>(It.IsAny<string>()))
                          .ReturnsAsync(_lookup);

            //Act
            var result = await _repository.FindOrganisations("test", 2, 0);

            //Assert
            Assert.AreEqual(2, result.Data.Count);

            Assert.IsTrue(result.Data.Any(x => x.Name.Equals("Test 1")));
            Assert.IsTrue(result.Data.Any(x => x.Name.Equals("Test 2")));
        }

        [Test]
        public async Task ThenShouldReturnTheAFullPageOfTheLastResultsIfTheLastPageIsSelected()
        {
            //Arrange
            _cacheProvider.Setup(x => x.GetAsync<PublicSectorOrganisationLookUp>(It.IsAny<string>()))
                          .ReturnsAsync(_lookup);

            //Act
            var result = await _repository.FindOrganisations("test", 2, 2);

            //Assert
            Assert.AreEqual(2, result.Data.Count);

            Assert.IsTrue(result.Data.Any(x => x.Name.Equals("Test 2")));
            Assert.IsTrue(result.Data.Any(x => x.Name.Equals("Test 3")));
        }

        [Test]
        public async Task ThenShouldReturnTheFirstPageIfThePageNumberIsLessThanOne()
        {
            //Arrange
            _cacheProvider.Setup(x => x.GetAsync<PublicSectorOrganisationLookUp>(It.IsAny<string>()))
                          .ReturnsAsync(_lookup);

            //Act
            var result = await _repository.FindOrganisations("test", 2, -2);

            //Assert
            Assert.AreEqual(2, result.Data.Count);

            Assert.IsTrue(result.Data.Any(x => x.Name.Equals("Test 1")));
            Assert.IsTrue(result.Data.Any(x => x.Name.Equals("Test 2")));
        }

        [Test]
        public async Task ThenIfThePageSizeIsLessThanOneThePageSizeShouldBeSetToOne()
        {
            //Arrange
            _cacheProvider.Setup(x => x.GetAsync<PublicSectorOrganisationLookUp>(It.IsAny<string>()))
                          .ReturnsAsync(_lookup);

            //Act
            var result = await _repository.FindOrganisations("test", 0, -2);

            //Assert
            Assert.AreEqual(1, result.Data.Count);

            Assert.IsTrue(result.Data.Any(x => x.Name.Equals("Test 1")));
        }
    }
}
