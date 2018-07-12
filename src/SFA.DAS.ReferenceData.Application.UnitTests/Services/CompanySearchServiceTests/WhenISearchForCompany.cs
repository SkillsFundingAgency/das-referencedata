using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.NLog.Logger;
using SFA.DAS.ReferenceData.Application.Services.OrganisationSearch;
using SFA.DAS.ReferenceData.Domain.Interfaces.Services;
using SFA.DAS.ReferenceData.Domain.Models.Company;
using SFA.DAS.ReferenceData.Types;
using Address = SFA.DAS.ReferenceData.Types.Address;

namespace SFA.DAS.ReferenceData.Application.UnitTests.Services.CompanySearchServiceTests
{
    public class WhenISearchForACompany
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
        public async Task ShouldSearchForCompaniesUsingCompanyHouseService()
        {
            //Arrange
            var searchTerm = "test";
            var results = new CompanySearchResults();
          
            _verificationService.Setup(x => x.FindCompany(It.IsAny<string>(), 10)).ReturnsAsync(results);

            //Act
            await _searchService.Search(searchTerm, 10);

            //Assert
            _verificationService.Verify(x => x.FindCompany(searchTerm, 10), Times.Once);
        }

        [Test]
        public async Task ShouldReturnFoundCompany()
        {
            //Arrange
            const string searchTerm = "test";
            var resultItem = new CompanySearchResultsItem
            {
                CompanyName = "Test Corp",
                Address = new Domain.Models.Company.Address
                {
                    Premises = "12",
                    CompaniesHouseLine1 = "Test Street",
                    Line2 = "Test Park",
                    TownOrCity = "Test Town",
                    County = "Testville",
                    PostCode = "TE12 3ST"
                },
                DateOfIncorporation = DateTime.Now,
                CompanyNumber = "12345678"
            }; 
            
            _verificationService.Setup(x => x.FindCompany(It.IsAny<string>(), 10)).ReturnsAsync(new CompanySearchResults
            {
                Companies = new List<CompanySearchResultsItem> { resultItem }
            });

            //Act
            var results = await _searchService.Search(searchTerm, 10);

            var organisation = results.FirstOrDefault();

            //Assert
            Assert.IsNotNull(organisation);
            Assert.AreEqual(resultItem.CompanyName, organisation.Name);

            Assert.AreEqual(resultItem.Address.Premises + " " + resultItem.Address.CompaniesHouseLine1, organisation.Address.Line1);
            Assert.AreEqual(resultItem.Address.Line2, organisation.Address.Line2);
            Assert.AreEqual(resultItem.Address.TownOrCity, organisation.Address.Line4);
            Assert.AreEqual(resultItem.Address.County, organisation.Address.Line5);
            Assert.AreEqual(resultItem.Address.PostCode, organisation.Address.Postcode);

            Assert.AreEqual(resultItem.CompanyNumber, organisation.Code);
            Assert.AreEqual(resultItem.DateOfIncorporation, organisation.RegistrationDate);
            Assert.AreEqual(OrganisationType.Company, organisation.Type);
            Assert.AreEqual(OrganisationSubType.None, organisation.SubType);
        }

        [Test]
        public async Task AndTheCompanyNameDoesNotContainTheSearchTermThenTheCompanyIsNotInTheResults()
        {
            //Arrange
            const string searchTerm = "NHS 24";
            var resultItem = new CompanySearchResultsItem
            {
                CompanyName = "This is not the nhs you're looking for",
            };

            _verificationService.Setup(x => x.FindCompany(It.IsAny<string>(), 10)).ReturnsAsync(new CompanySearchResults
            {
                Companies = new List<CompanySearchResultsItem> { resultItem }
            });

            //Act
            var results = await _searchService.Search(searchTerm, 10);

            var organisation = results.FirstOrDefault();

            //Assert
            Assert.IsNull(organisation);
        }
		
		[Test]
        public async Task ThenAnEmptyAddressIsReturnedWhenNullIsReturnedFromTheApi()
        {
            //Arrange
            var resultItem = new CompanySearchResultsItem
            {
                CompanyName = "Test Corp",
                Address = null,
                DateOfIncorporation = DateTime.Now,
                CompanyNumber = "12345678"
            };
            _verificationService.Setup(x => x.FindCompany(It.IsAny<string>(), 10)).ReturnsAsync(new CompanySearchResults
            {
                Companies = new List<CompanySearchResultsItem> { resultItem }
            });

            //Act
            var actual = await _searchService.Search("test", 10);

            //Assert
            Assert.IsNotNull(actual);
            Assert.IsNotNull(actual.FirstOrDefault());
            Assert.IsAssignableFrom<Address>(actual.First().Address);
        }
		
        [Test]
        public async Task ShouldReturnNullIfExceptionIsThrowAndLogTheError()
        {
            //Arrange
            var exception = new WebException();
            _verificationService.Setup(x => x.FindCompany(It.IsAny<string>(), 10))
                .Throws(exception);

            //Act
            var result = await _searchService.Search("test", 10);

            //Assert
            _logger.Verify(x => x.Error(exception, It.IsAny<string>()));
            Assert.IsNull(result);
        }

        [Test]
        public async Task ShouldReturnNullIfNoCompaniesFound()
        {
            //Arrange
            _verificationService.Setup(x => x.FindCompany(It.IsAny<string>(), 10)).ReturnsAsync(new CompanySearchResults{Companies = new CompanySearchResultsItem[0]});

            //Act
            var result = await _searchService.Search("test", 10);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsEmpty(result);
        }
    }
}
