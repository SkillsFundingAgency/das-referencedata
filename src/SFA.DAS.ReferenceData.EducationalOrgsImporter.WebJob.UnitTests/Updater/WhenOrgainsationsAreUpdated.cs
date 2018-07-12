using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.NLog.Logger;
using SFA.DAS.ReferenceData.Domain.Interfaces.Services;
using SFA.DAS.ReferenceData.Domain.Models.Education;
using SFA.DAS.ReferenceData.EducationOrgsImporter.WebJob.Azure;
using SFA.DAS.ReferenceData.EducationOrgsImporter.WebJob.Serializer;
using SFA.DAS.ReferenceData.EducationOrgsImporter.WebJob.Updater;
using SFA.DAS.ReferenceData.Types;

namespace SFA.DAS.ReferenceData.EducationalOrgsImporter.WebJob.UnitTests.Updater
{
    public class WhenOrgainsationsAreUpdated
    {
        private EducationalOrgsUpdater _updater;
        private Mock<IEdubaseService> _edubaseService;
        private Mock<IEducationalOrgainsationSerialiser> _serialiser;
        private Mock<IAzureStorageUploader> _uploader;
        private Mock<ILog> _logger;


        [SetUp]
        public void Arrange()
        {
            _edubaseService = new Mock<IEdubaseService>();
            _serialiser = new Mock<IEducationalOrgainsationSerialiser>();
            _uploader = new Mock<IAzureStorageUploader>();
            _logger = new Mock<ILog>();

            _updater = new EducationalOrgsUpdater(
                _edubaseService.Object,
                _serialiser.Object,
                _uploader.Object,
                _logger.Object);
        }

        [Test]
        public async Task ThenShouldUploadCollectedOrgansiations()
        {
            //Arrange
            var organisations = new List<EducationOrganisation> {new EducationOrganisation()};
            var jsonData = new byte[10];

            _edubaseService.Setup(x => x.GetOrganisations())
                           .ReturnsAsync(organisations);

            _serialiser.Setup(x => x.SerialiseToJson(It.IsAny<EducationalOrganisationLookUp>()))
                       .Returns(jsonData);

            //Act
            await _updater.RunUpdate();

            //Assert
            _edubaseService.Verify(x => x.GetOrganisations(), Times.Once);
            _serialiser.Verify(x => x.SerialiseToJson(It.Is< EducationalOrganisationLookUp>(org => org.Organisations.Equals(organisations))), Times.Once);
            _uploader.Verify(x => x.UploadDataToStorage(jsonData), Times.Once);
        }

        [Test]
        public async Task ThenShouldNotUploadOrgainsationsIfNoneFound()
        {
            //Arrange
            _edubaseService.Setup(x => x.GetOrganisations())
                           .ReturnsAsync(new List<EducationOrganisation>());

            //Act
            await _updater.RunUpdate();

            //Assert
            _edubaseService.Verify(x => x.GetOrganisations(), Times.Once);
            _serialiser.Verify(x => x.SerialiseToJson(It.IsAny<EducationalOrganisationLookUp>()), Times.Never);
            _uploader.Verify(x => x.UploadDataToStorage(It.IsAny<byte[]>()), Times.Never);
        }

        [Test]
        public async Task ThenShouldLogErrorIfEdubaseThrowsOne()
        {
            //Arrange
            var exception = new WebException();

            _edubaseService.Setup(x => x.GetOrganisations()).Throws(exception);

            //Act
            await _updater.RunUpdate();

            //Assert
            _logger.Verify(x => x.Error(exception, It.IsAny<string>()), Times.Once);
            _serialiser.Verify(x => x.SerialiseToJson(It.IsAny<EducationalOrganisationLookUp>()), Times.Never);
        }

        [Test]
        public async Task ThenShouldLogErrorIfSerialiserThrowsOne()
        {
            //Arrange
            var exception = new SerializationException();
          

            _edubaseService.Setup(x => x.GetOrganisations())
                           .ReturnsAsync(new List<EducationOrganisation> { new EducationOrganisation() });

            _serialiser.Setup(x => x.SerialiseToJson(It.IsAny<EducationalOrganisationLookUp>()))
                       .Throws(exception);

            //Act
            await _updater.RunUpdate();

            //Assert
            _logger.Verify(x => x.Error(exception, It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task ThenShouldLogErrorIfAzureUploaderThrowsOne()
        {
            //Arrange
            var exception = new WebException();

            _edubaseService.Setup(x => x.GetOrganisations())
                           .ReturnsAsync(new List<EducationOrganisation> { new EducationOrganisation() });

            _serialiser.Setup(x => x.SerialiseToJson(It.IsAny<EducationalOrganisationLookUp>()))
                       .Returns(new byte[10]);

            _uploader.Setup(x => x.UploadDataToStorage(It.IsAny<byte[]>())).Throws(exception);

            //Act
            await _updater.RunUpdate();

            //Assert
            _logger.Verify(x => x.Error(exception, It.IsAny<string>()), Times.Once);
        }
    }
}
