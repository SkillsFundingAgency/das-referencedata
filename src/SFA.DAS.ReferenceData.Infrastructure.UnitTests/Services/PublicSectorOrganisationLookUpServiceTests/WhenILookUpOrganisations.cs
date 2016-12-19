using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.ReferenceData.Domain.Models;
using SFA.DAS.ReferenceData.Infrastructure.Caching;
using SFA.DAS.ReferenceData.Infrastructure.Services;

namespace SFA.DAS.ReferenceData.Infrastructure.UnitTests.Services.PublicSectorOrganisationLookUpServiceTests
{
    class WhenILookUpOrganisations
    {
        private Mock<ICacheProvider> _cacheProvider;
        private Mock<PublicSectorOrganisationLookUpService> _serviceMock;
        private PublicSectorOrganisationLookUpService _service;

        [SetUp]
        public void Arrange()
        {
            _cacheProvider = new Mock<ICacheProvider>();

            _cacheProvider.Setup(
                x => x.Set(
                    It.IsAny<string>(), 
                    It.IsAny<PublicSectorOrganisationLookUp>(), 
                    It.IsAny<TimeSpan>()));

            _serviceMock = new Mock<PublicSectorOrganisationLookUpService>(_cacheProvider.Object, "");

            _serviceMock.Setup(x => x.GetDataFromBlobStorage(It.IsAny<string>(), It.IsAny<string>())).Returns(new PublicSectorOrganisationLookUp
            {
                OrganisationNames = new List<string>()
            });

            _service = _serviceMock.Object;
        }

        [Test]
        public async Task ThenShouldGetOrganisationsFromTheCacheIfTheyAreAvailable()
        {
            //Arrange
            _cacheProvider.Setup(x => x.Get<PublicSectorOrganisationLookUp>(nameof(PublicSectorOrganisationLookUp)))
                .Returns(new PublicSectorOrganisationLookUp
                {
                    OrganisationNames = new List<string>()
                });

            //Act
            await _service.GetOrganisations();

            //Assert

            _cacheProvider.Verify(
                x => x.Set(
                    It.IsAny<string>(),
                    It.IsAny<PublicSectorOrganisationLookUp>(),
                    It.IsAny<TimeSpan>()), Times.Never);
        }


        [Test]
        public async Task ThenUpdateCachedOrganisationIfTheyAreNotAvailableInTheCache()
        {
            //Arrange
            _cacheProvider.Setup(x => x.Get<PublicSectorOrganisationLookUp>(nameof(PublicSectorOrganisationLookUp)))
                .Returns((PublicSectorOrganisationLookUp) null);

            //Act
            await _service.GetOrganisations();

            //Assert
            _cacheProvider.Verify(
                x =>
                    x.Set(
                        nameof(PublicSectorOrganisationLookUp),
                        It.IsAny<PublicSectorOrganisationLookUp>(),
                        TimeSpan.FromDays(14)), Times.Once);

            _serviceMock.Verify(x => x.GetDataFromBlobStorage(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }
    }
}
