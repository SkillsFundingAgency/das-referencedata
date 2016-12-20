using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
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
                }
            };

            _azureService = new Mock<IAzureService>();

            _azureService.Setup(x => x.GetModelFromBlobStorage<PublicSectorOrganisationLookUp>(
                It.IsAny<string>(), 
                It.IsAny<string>()))
                         .ReturnsAsync(_lookup);

            _cacheProvider = new Mock<ICacheProvider>();

            _repository = new PublicSectorOrganisationRepository(_cacheProvider.Object, _azureService.Object);
        }

        [Test]
        public async Task ThenIShouldGetOrganisationFromAzureIfNotInCache()
        {
            //Arrange
            _cacheProvider.Setup(x => x.Get<PublicSectorOrganisationLookUp>(It.IsAny<string>()))
                          .Returns((PublicSectorOrganisationLookUp) null);

            //Act
            var result = await _repository.GetOrganisations();

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
            var result = await _repository.GetOrganisations();

            //Assert
            Assert.IsNotEmpty(result);

            _cacheProvider.Verify(x => x.Get<PublicSectorOrganisationLookUp>(
               nameof(PublicSectorOrganisationLookUp)), Times.Once);

            _azureService.Verify(x => x.GetModelFromBlobStorage<PublicSectorOrganisationLookUp>(
                It.IsAny<string>(),
                 It.IsAny<string>()), Times.Never);
        }
    }
}
