using System;
using System.Threading.Tasks;
using Moq;
using SFA.DAS.ReferenceData.CharityImport.WebJob.Updater;
using NUnit.Framework;
using SFA.DAS.NLog.Logger;
using SFA.DAS.ReferenceData.Domain.Configuration;
using SFA.DAS.ReferenceData.Domain.Interfaces.Data;
using SFA.DAS.ReferenceData.Domain.Interfaces.Services;
using SFA.DAS.ReferenceData.Domain.Models.Charity;
using System.IO;
using System.Text;

namespace SFA.DAS.ReferenceData.CharityImport.WebJob.UnitTests.CharityImporterTests
{
    public class WhenIImportCharityData
    {
        private CharityImporter _importer;
        private ReferenceDataApiConfiguration _configuration;
        private Mock<ICharityRepository> _charityRepository;        
        private Mock<ICharityService> _charityService;
        private Mock<IDataDownloadService> _dataDownloadService;
        private Mock<ILog> _logger;

        [SetUp]
        public void Arrange()
        {
            _charityRepository = new Mock<ICharityRepository>();            
            _charityService = new Mock<ICharityService>();
            _dataDownloadService = new Mock<IDataDownloadService>();
            _logger = new Mock<ILog>();

            _charityRepository.Setup(x => x.GetLastCharityDataImport())
                .ReturnsAsync(() => new CharityDataImport {ImportDate = new DateTime(2016,5,4), Month = 5, Year=2016});

            _dataDownloadService.Setup(
                x => x.GetFileStream(It.IsAny<string>()))
                 .ReturnsAsync(new MemoryStream(Encoding.UTF8.GetBytes("Lorem Ipsum")));

            _configuration = new ReferenceDataApiConfiguration {  DatabaseConnectionString="" };

            _importer = new CharityImporter(_configuration, _charityRepository.Object, _charityService.Object, _dataDownloadService.Object, _logger.Object);
        }      

        [Test]
        public async Task ThenIfAPreviousImportExistsThenSubsequenttMonthIsUsedAsDefault()
        {            
            //Act
            await _importer.RunUpdate();

            //Assert            
            _dataDownloadService.Verify(x => x.GetFileStream(It.IsAny<string>()), Times.Once);
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
        public async Task ThenExecuteCharityImportIsIssued()
        {
            //Act
            await _importer.RunUpdate();

            //Assert
            _charityService.Verify(x => x.ExecuteCharityImport(It.IsAny<Stream>()), Times.Once);
        }

        [Test]
        public async Task ThenDataIsTransferredFromPublicExtractCharityToStagingTable()
        {
            //Act
            await _importer.RunUpdate();

            //Assert
            _charityRepository.Verify(x => x.ImportFromPublicExtractCharityToStagingTable(), Times.Once);
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
    }
}
