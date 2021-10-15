using Moq;
using NUnit.Framework;
using SFA.DAS.NLog.Logger;
using SFA.DAS.ReferenceData.Domain.Interfaces.Data;
using SFA.DAS.ReferenceData.Domain.Interfaces.Services;
using SFA.DAS.ReferenceData.Domain.Models.Charity;
using SFA.DAS.ReferenceData.Infrastructure.Services;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SFA.DAS.ReferenceData.Infrastructure.UnitTests.Services.CharityServiceTests
{
    public class WhenIGetCharityContent
    {
        private CharityService _charityService;        
        private Mock<ICharityImportRepository> _charityImportRepository;        
        private Mock<IZipArchiveHelper> _zipArchiveHelper;
        private Mock<ILog> _logger;
        private const string CharityImportFileName = "publicextract.charity.json";

        [SetUp]
        public void Arrange()
        {
            _charityImportRepository = new Mock<ICharityImportRepository>();
            _zipArchiveHelper = new Mock<IZipArchiveHelper>();
            _logger = new Mock<ILog>();

            _zipArchiveHelper.Setup(x => x.ExtractModelFromJsonFileZipStream<CharityImport>(It.IsAny<Stream>(), CharityImportFileName))
                            .Returns(It.IsAny<IEnumerable<CharityImport>>());            

            _charityService = new CharityService(_logger.Object, _charityImportRepository.Object, _zipArchiveHelper.Object);
        }

        [Test]
        public async Task ThenExtractModelFromJsonFileZipStream()
        {
            //Act
            await _charityService.ExecuteCharityImport(It.IsAny<Stream>());

            //Assert            
            _zipArchiveHelper.Verify(x => x.ExtractModelFromJsonFileZipStream<CharityImport>(It.IsAny<Stream>(), CharityImportFileName), Times.Once);
        }

        [Test]
        public async Task ThenImportToStagingTable()
        {
            //Act
            await _charityService.ExecuteCharityImport(It.IsAny<Stream>());

            //Assert            
            _charityImportRepository.Verify(x => x.ImportToStagingTable(It.IsAny<IEnumerable<CharityImport>>()), Times.Once);
        }
    }
}
