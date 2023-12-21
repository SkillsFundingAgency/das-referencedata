using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.NLog.Logger;
using SFA.DAS.ReferenceData.Domain.Configuration;
using SFA.DAS.ReferenceData.Domain.Interfaces.Services;
using SFA.DAS.ReferenceData.Domain.Models;
using SFA.DAS.ReferenceData.PublicSectorOrgs.WebJob.Updater;
using SFA.DAS.ReferenceData.Types.DTO;

namespace SFA.DAS.ReferenceData.PublicSectorOrgs.WebJob.UnitTests.PublicOrgsUpdaterTests
{
    public class WhenIImportPublicOrgsData
    {
        private Mock<ILog> _logger;
        private ReferenceDataApiConfiguration _configuration;
        private Mock<IArchiveDownloadService> _archiveDownloadService;
        private Mock<INhsDataUpdater> _nhsDataUpdater;
        private PublicOrgsUpdater _updater;
        private Mock<IPublicSectorOrganisationDatabaseUpdater> _dbUpdater;
        private Mock<IPoliceDataLookupService> _policeDataLookUpService;
        private Mock<IJsonManager> _jsonManager;

        private string _onsUrl =
                "https://www.ons.gov.uk/file?uri=/methodology/classificationsandstandards/economicstatisticsclassifications/introductiontoeconomicstatisticsclassifications/publicsectorclassificationguide{0}.xls";
        private string _onsDateFormat = "MMMyy";

        [SetUp]
        public void Arrange()
        {
            _logger = new Mock<ILog>();
            _configuration = new ReferenceDataApiConfiguration
            {
                ONSUrl = _onsUrl,
                OnsUrlDateFormat = _onsDateFormat,
                NhsTrustsUrls = new List<string>
                {
                    { "http://aurl.com" }
                },
                PoliceForcesUrl = "http://aurl.com"
            };
            _archiveDownloadService = new Mock<IArchiveDownloadService>();
            _nhsDataUpdater = new Mock<INhsDataUpdater>();
            _dbUpdater = new Mock<IPublicSectorOrganisationDatabaseUpdater>();
            _policeDataLookUpService = new Mock<IPoliceDataLookupService>();
            _jsonManager = new Mock<IJsonManager>();
            _updater = new PublicOrgsUpdater(_logger.Object, _configuration, _archiveDownloadService.Object, _nhsDataUpdater.Object, _dbUpdater.Object, _policeDataLookUpService.Object, _jsonManager.Object);
        }

        [Test]
        public async Task ThenTheServiceUrlIsConstructedFromTheONSUrlAndONSDateFormatSettings()
        {
            var urlFormat = string.Format(_onsUrl, DateTime.UtcNow.ToString(_onsDateFormat).ToLower());

            _archiveDownloadService.Setup(o => o.DownloadFile(urlFormat, It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);
            SetupObjectsThatReturnAPublicSectorOrganisationLookupOrList();
            await _updater.RunUpdate();

            _archiveDownloadService.Verify(o => o.DownloadFile(urlFormat, It.IsAny<string>(), It.IsAny<string>()));
        }

        [Test]
        public async Task WhenTheOnsFileIsNotAvailableForTheCurrentMonthItDownloadsForThePreviousMonth()
        {
            _archiveDownloadService.SetupSequence(o => o.DownloadFile(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(false)).Returns(Task.FromResult(true));

            SetupObjectsThatReturnAPublicSectorOrganisationLookupOrList();
            await _updater.RunUpdate();

            _archiveDownloadService.Verify(o => o.DownloadFile(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(2));
            _dbUpdater.Verify(o => o.UpdateDatabase(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void WhenTheOnsFileIsNotAvailableForTheCurrentMonthAndAlsoForThePreviousMonthsUpdateDatabaseIsNotCalledExceptionIsThrown()
        {
            _archiveDownloadService.SetupSequence(o => o.DownloadFile(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(false)).Returns(Task.FromResult(false));

            SetupObjectsThatReturnAPublicSectorOrganisationLookupOrList();
            Assert.ThrowsAsync<Exception>(async () =>
            {
                await _updater.RunUpdate();
            });

            _archiveDownloadService.Verify(o => o.DownloadFile(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(5));
            _dbUpdater.Verify(o => o.UpdateDatabase(It.IsAny<string>()), Times.Never);
        }

        private void SetupObjectsThatReturnAPublicSectorOrganisationLookupOrList()
        {
            _dbUpdater.Setup(o => o.UpdateDatabase(It.IsAny<string>()))
                .Returns(new PublicSectorOrganisationLookUp
                {
                    Organisations = new List<PublicSectorOrganisation>()
                });
            _policeDataLookUpService.Setup(o => o.GetGbPoliceForces())
                .ReturnsAsync(new PublicSectorOrganisationLookUp
                {
                    Organisations = new List<PublicSectorOrganisation>()
                });
            _nhsDataUpdater.Setup(o => o.GetData()).ReturnsAsync(new PublicSectorOrganisationLookUp
            {
                Organisations = new List<PublicSectorOrganisation>()
            });
        }
    }
}
