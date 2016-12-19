using System;
using System.Threading.Tasks;
using Moq;
using NLog;
using SFA.DAS.ReferenceData.CharityImport.WebJob.Updater;
using NUnit.Framework;
using SFA.DAS.ReferenceData.Domain.Interfaces.Data;
using SFA.DAS.ReferenceData.Domain.Interfaces.Services;
using SFA.DAS.ReferenceData.Domain.Models.Bcp;
using SFA.DAS.ReferenceData.Domain.Models.Charity;

namespace SFA.DAS.ReferenceData.CharityImport.WebJob.UnitTests.CharityImporterTests
{
    public class WhenIImportCharityData
    {
        private CharityImporter _importer;
        private Mock<ICharityRepository> _charityRepository;
        private Mock<IBcpService> _bcpService;
        private Mock<IArchiveDownloadService> _archiveDownloadService;
        private Mock<ILogger> _logger;

        [SetUp]
        public void Arrange()
        {
            _charityRepository = new Mock<ICharityRepository>();
            _bcpService = new Mock<IBcpService>();
            _archiveDownloadService = new Mock<IArchiveDownloadService>();
            _logger = new Mock<ILogger>();

            _charityRepository.Setup(x => x.GetLastCharityDataImport())
                .ReturnsAsync(() => new CharityDataImport {ImportDate = new DateTime(2016,5,4), Month = 5, Year=2016});

            _archiveDownloadService.Setup(
                x => x.DownloadFile(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(() => true);

            _archiveDownloadService.Setup(
                x => x.UnzipFile(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(() => true);

            _importer = new CharityImporter(_charityRepository.Object, _archiveDownloadService.Object, _bcpService.Object);
        }

        [Test]
        public async Task ThenThePreviousImportDateShouldBeRetrieved()
        {
            //Act
            await _importer.RunUpdate();

            //Assert
            _charityRepository.Verify(x=> x.GetLastCharityDataImport(), Times.Once);
        }

        [Test]
        public async Task ThenIfAPreviousImportExistsThenSubsequenttMonthIsUsedAsDefault()
        {
            //Setup
            _charityRepository.Setup(x => x.GetLastCharityDataImport())
                .ReturnsAsync(() => new CharityDataImport { ImportDate = new DateTime(2017, 12, 1), Month = 12, Year = 2016 });

            //Act
            await _importer.RunUpdate();

            //Assert
            var expectedFile = $"January_2017";

            _archiveDownloadService.Verify(x => x.DownloadFile(It.IsAny<string>(), It.IsAny<string>(), It.IsRegex(expectedFile)), Times.Once);
        }

        [Test]
        public async Task ThenIfNoPreviousImportExistsThenCurrentMonthIsUsedAsDefault()
        {
            //Setup
            _charityRepository.Setup(x => x.GetLastCharityDataImport())
                .ReturnsAsync(() => null);

            //Act
            await _importer.RunUpdate();

            //Assert
            var expectedFile = $"{DateTime.Now.ToString("MMMM")}_{DateTime.Now.Year}";

            _archiveDownloadService.Verify(x=> x.DownloadFile(It.IsAny<string>(), It.IsAny<string>(), It.IsRegex(expectedFile)), Times.Once);
        }

        [Test]
        public async Task ThenAZipFileIsExtracted()
        {
            //Act
            await _importer.RunUpdate();

            //Assert
            _archiveDownloadService.Verify(x => x.UnzipFile(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task ThenABcpCommandIsIssued()
        {
            //Act
            await _importer.RunUpdate();

            //Assert
            _bcpService.Verify(x => x.ExecuteBcp(It.IsAny<BcpRequest>()), Times.Once);
        }

        [Test]
        public async Task ThenTheImportIsRecorded()
        {
            //Act
            await _importer.RunUpdate();

            //Assert
            _charityRepository.Verify(x => x.CreateCharityDataImport(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }
    }
}
