using System;
using System.Net;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.NLog.Logger;
using SFA.DAS.ReferenceData.Application.Services.OrganisationSearch;
using SFA.DAS.ReferenceData.Domain.Interfaces.Services;
using SFA.DAS.ReferenceData.Domain.Models.Company;
using SFA.DAS.ReferenceData.Types;

namespace SFA.DAS.ReferenceData.Application.UnitTests.Services.CompanySearchServiceTests
{
    public class WhenIGetACompanyByReference
    {
        private Mock<ILog> _logger;
        private Mock<ICompaniesHouseEmployerVerificationService> _verificationService;
        private CompanySearchService _searchService;

        [SetUp]
        public void Arrange()
        {
            _logger = new Mock<ILog>();
            _verificationService = new Mock<ICompaniesHouseEmployerVerificationService>();

            _searchService = new CompanySearchService(_verificationService.Object, _logger.Object);
        }

        [Test]
        public async Task ShouldSearchForCompanyUsingCompanyHouseService()
        {
            //Arrange
            var reference = "12345678";
            var info = new CompanyInformation();
          
            _verificationService.Setup(x => x.GetInformation(It.IsAny<string>())).ReturnsAsync(info);

            //Act
            await _searchService.Search(reference);

            //Assert
            _verificationService.Verify(x => x.GetInformation(reference), Times.Once);
        }

        [Test]
        public async Task ShouldReturnFoundCompany()
        {
            //Arrange
            const string reference = "12345678";
            var info = new CompanyInformation
            {
                CompanyName = "Test Corp",
                RegisteredAddress = new Domain.Models.Company.Address
                {
                    CompaniesHouseLine1 = "12 Test Street",
                    Line2 = "Test Park",
                    TownOrCity = "Test Town",
                    County = "Testville",
                    PostCode = "TE12 3ST"
                },
                DateOfIncorporation = DateTime.Now,
                CompanyNumber = "12345678"
            };
            _verificationService.Setup(x => x.GetInformation(It.IsAny<string>())).ReturnsAsync(info);

            //Act
            var result = await _searchService.Search(reference);

            //Assert
            Assert.AreEqual(info.CompanyName, result.Name);

            Assert.AreEqual(info.RegisteredAddress.Line1, result.Address.Line1);
            Assert.AreEqual(info.RegisteredAddress.Line2, result.Address.Line2);
            Assert.AreEqual(info.RegisteredAddress.TownOrCity, result.Address.Line4);
            Assert.AreEqual(info.RegisteredAddress.County, result.Address.Line5);
            Assert.AreEqual(info.RegisteredAddress.PostCode, result.Address.Postcode);

            Assert.AreEqual(info.CompanyNumber, result.Code);
            Assert.AreEqual(info.DateOfIncorporation, result.RegistrationDate);
            Assert.AreEqual(OrganisationType.Company, result.Type);
            Assert.AreEqual(OrganisationSubType.None, result.SubType);
        }

        [Test]
        public async Task ShouldReturnNullIfExceptionIsThrowAndLogTheError()
        {
            //Arrange
            var exception = new WebException();
            _verificationService.Setup(x => x.GetInformation(It.IsAny<string>()))
                .Throws(exception);

            //Act
            var result = await _searchService.Search("12345678");

            //Assert
            _logger.Verify(x => x.Error(exception, It.IsAny<string>()));
            Assert.IsNull(result);
        }

        [Test]
        public async Task ShouldReturnNullIfNoCompanyFound()
        {
            //Arrange
            _verificationService.Setup(x => x.GetInformation(It.IsAny<string>())).ReturnsAsync((CompanyInformation)null);

            //Act
            var result = await _searchService.Search("12345678");

            //Assert
            Assert.IsNull(result);
        }
    }
}
