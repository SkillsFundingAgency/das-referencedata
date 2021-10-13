using System;
using System.Threading.Tasks;
using Moq;
using SFA.DAS.ReferenceData.CharityImport.WebJob.Updater;
using NUnit.Framework;
using SFA.DAS.NLog.Logger;
using SFA.DAS.ReferenceData.Domain.Configuration;
using SFA.DAS.ReferenceData.Domain.Interfaces.Data;
using SFA.DAS.ReferenceData.Domain.Interfaces.Services;
using SFA.DAS.ReferenceData.Domain.Models.Bcp;
using SFA.DAS.ReferenceData.Domain.Models.Charity;

namespace SFA.DAS.ReferenceData.CharityImport.WebJob.UnitTests.CharityImporterTests
{
    public class WhenIImportCharityData
    {
        private CharityImporter _importer;
        private ReferenceDataApiConfiguration _configuration;
        private Mock<ICharityRepository> _charityRepository;        
        private Mock<ICharityService> _charityService;
        private Mock<IArchiveDownloadService> _archiveDownloadService;
        private Mock<ILog> _logger;

        [SetUp]
        public void Arrange()
        {
            _charityRepository = new Mock<ICharityRepository>();            
            _charityService = new Mock<ICharityService>();
            _archiveDownloadService = new Mock<IArchiveDownloadService>();
            _logger = new Mock<ILog>();

            _charityRepository.Setup(x => x.GetLastCharityDataImport())
                .ReturnsAsync(() => new CharityDataImport {ImportDate = new DateTime(2016,5,4), Month = 5, Year=2016});

            _archiveDownloadService.Setup(
                x => x.DownloadFile(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(() => true);

            _configuration = new ReferenceDataApiConfiguration {  DatabaseConnectionString="" };

            _importer = new CharityImporter(_configuration, _charityRepository.Object, _charityService.Object,  _archiveDownloadService.Object, _logger.Object);
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
                .ReturnsAsync(() => new CharityDataImport { ImportDate = new DateTime(2016, 12, 1), Month = 12, Year = 2016 });

            //Act
            await _importer.RunUpdate();

            //Assert
            var expectedFile = "publicextract.charity.zip";

            _archiveDownloadService.Verify(x => x.DownloadFile(It.IsAny<string>(), It.IsAny<string>(), It.IsRegex(expectedFile)), Times.Once);
        }

        [Test]
        public async Task ThenDontDownloadPublicCharityFileSinceLastImportLessThanAMonth()
        {
            //Arrange
            _charityRepository.Setup(x => x.GetLastCharityDataImport())
                .ReturnsAsync(() => new CharityDataImport { ImportDate = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1), Month = DateTime.UtcNow.Month, Year = DateTime.UtcNow.Year });
            var expectedFile = "publicextract.charity.zip";

            //Act
            await _importer.RunUpdate();

            //Assert
            _archiveDownloadService.Verify(x => x.DownloadFile(It.IsAny<string>(), It.IsAny<string>(), It.IsRegex(expectedFile)), Times.Never);
        }

        [Test]
        public async Task ThenIfNoPreviousImportExistsThenSearchStartsAtJune2017()
        {
            //Setup
            _charityRepository.Setup(x => x.GetLastCharityDataImport())
                .ReturnsAsync(() => null);

            //Act
            await _importer.RunUpdate();

            //Assert
            var expectedFile = "publicextract.charity.zip";

            _archiveDownloadService.Verify(x=> x.DownloadFile(It.IsAny<string>(), It.IsAny<string>(), It.IsRegex(expectedFile)), Times.Once);
        }


        [Test]
        public async Task ThenImportLoadTablesAreTruncated()
        {
            //Act
            await _importer.RunUpdate();

            //Assert
            _charityRepository.Verify(x => x.TruncateLoadTables(), Times.Once);
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
        public async Task ThenExecuteCharityImportIsIssued()
        {
            //Act
            await _importer.RunUpdate();

            //Assert
            _charityService.Verify(x => x.ExecuteCharityImport(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task ThenDataIsTransferredFromLoadTables()
        {
            //Act
            await _importer.RunUpdate();

            //Assert
            _charityRepository.Verify(x => x.ImportDataFromLoadTables(), Times.Once);
        }

        [Test]
        public async Task ThenTheImportIsRecorded()
        {
            //Act
            await _importer.RunUpdate();

            //Assert
            _charityRepository.Verify(x => x.CreateCharityDataImport(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }

        [Test]
        public async Task ThenIfDownloadFailsThenNoFurtherWorkIsDone()
        {
            //Setup
            _archiveDownloadService.Setup(x => x.DownloadFile(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(() => false);

            //Act
            await _importer.RunUpdate();

            //Assert
            _archiveDownloadService.Verify(x=> x.UnzipFile(It.IsAny<string>(), It.IsAny<string>()), Times.Never);            
            _charityRepository.Verify(x=> x.RecordCharityDataImport(It.IsAny<int>(), It.IsAny<int>()),Times.Never);
        }
    }
}
