using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.ReferenceData.Domain.Configuration;
using SFA.DAS.ReferenceData.Domain.Interfaces.Data;
using SFA.DAS.ReferenceData.Domain.Interfaces.Services;
using SFA.DAS.ReferenceData.Domain.Models;
using SFA.DAS.ReferenceData.PublicSectorOrgs.WebJob.Updater;
using SFA.DAS.ReferenceData.Types.DTO;

namespace SFA.DAS.ReferenceData.PublicSectorOrgs.WebJob.UnitTests.NhsDataImporterTests
{
    public class WhenIImportNhsData
    {
        private NhsDataUpdater _nhsDataUpdater;
        private Mock<IArchiveDownloadService> _archiveDownloadService;

        private ReferenceDataApiConfiguration _configuration;
        private Mock<INhsCsvReaderHelper> _nhsCsvReaderHelper;
        private Mock<IFileSystemRepository> _fileSystemRepository;
        private const string ExpectedNhsUrl1 = "http://someurl1";
        private const string ExpectedNhsUrl2 = "http://someurl2";
        private const string ExpectedNhsUrl3 = "http://someurl3";

        [SetUp]
        public void Arrange()
        {
            _configuration = new ReferenceDataApiConfiguration
            {
                NhsTrustsUrls = new List<string>
                {
                    ExpectedNhsUrl1,
                    ExpectedNhsUrl2,
                    ExpectedNhsUrl3
                }
            };

            _archiveDownloadService = new Mock<IArchiveDownloadService>();
            _archiveDownloadService.Setup(x => x.DownloadFile(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);

            _nhsCsvReaderHelper = new Mock<INhsCsvReaderHelper>();
            _nhsCsvReaderHelper.Setup(x => x.ReadNhsFile(It.IsAny<string>())).Returns(new List<PublicSectorOrganisation> {new PublicSectorOrganisation()});

            _fileSystemRepository = new Mock<IFileSystemRepository>();
            _fileSystemRepository.Setup(x => x.GetDataFile(It.IsAny<string>())).Returns("test");

            _nhsDataUpdater = new NhsDataUpdater(_archiveDownloadService.Object, _configuration, _nhsCsvReaderHelper.Object, _fileSystemRepository.Object);
        }

        [Test]
        public async Task ThenTheArchiveServiceIsCalledForEachFileInTheConfigurationObject()
        {
            //Act
            await _nhsDataUpdater.GetData();
            
            //Assert
            _archiveDownloadService.Verify(x=>x.DownloadFile(ExpectedNhsUrl1,It.IsAny<string>(), "nhsfile.zip"), Times.Once);
            _archiveDownloadService.Verify(x=>x.DownloadFile(ExpectedNhsUrl2,It.IsAny<string>(), "nhsfile.zip"), Times.Once);
            _archiveDownloadService.Verify(x=>x.DownloadFile(ExpectedNhsUrl3,It.IsAny<string>(), "nhsfile.zip"), Times.Once);
        }

        [Test]
        public async Task ThenEachFileDownloadedIsExtractedWhenTheFileIsSucessfullyDownloaded()
        {
            //Arrange
            _archiveDownloadService.SetupSequence(
                x => x.DownloadFile(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true)
                .ReturnsAsync(false)
                .ReturnsAsync(true);

            //Act
            await _nhsDataUpdater.GetData();

            //Assert
            _archiveDownloadService.Verify(x => x.UnzipFile(Path.Combine(Path.GetTempPath(), "nhsFile.zip"), Path.Combine(Path.GetTempPath(),"NhsExtract")),Times.Exactly(2));
        }

        [Test]
        public async Task ThenTheExtractedFileIsFoundForEachFileDownloadedByTheFileRepositoryHelper()
        {
            //Arrange
            _archiveDownloadService.SetupSequence(
                x => x.DownloadFile(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true)
                .ReturnsAsync(false)
                .ReturnsAsync(true);

            //Act
            await _nhsDataUpdater.GetData();

            //Assert
            _fileSystemRepository.Verify(x=>x.GetDataFile(Path.Combine(Path.GetTempPath(), "NhsExtract")), Times.Exactly(2));
        }

        [Test]
        public async Task ThenEachFileIsReadAfterItIsExtractedIfItExists()
        {
            //Arrange
            _archiveDownloadService.Setup(
                x => x.DownloadFile(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true);
            _fileSystemRepository.SetupSequence(x => x.GetDataFile(It.IsAny<string>()))
                .Returns("SomeValue")
                .Returns("")
                .Returns("AnotherValue");

            //Act
            await _nhsDataUpdater.GetData();

            //Assert
            _nhsCsvReaderHelper.Verify(x=>x.ReadNhsFile(It.IsAny<string>()),Times.Exactly(2));

        }
        
        [Test]
        public async Task ThenTheDataIsReturnedInTheResponse()
        {
           

            //Act
            var actual = await _nhsDataUpdater.GetData();

            //Assert
            Assert.IsAssignableFrom<PublicSectorOrganisationLookUp>(actual);
            Assert.IsNotNull(actual.Organisations);
            Assert.IsNotEmpty(actual.Organisations);
        }
    }
}
