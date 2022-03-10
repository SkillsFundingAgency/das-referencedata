﻿using System;
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
using SFA.DAS.ReferenceData.Types.DTO;
using Address = SFA.DAS.ReferenceData.Types.DTO.Address;

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
                CompanyName = "Test Company",
                Address = new Domain.Models.Company.Address
                {
                    Premises = "12",
                    CompaniesHouseLine1 = "Test Street",
                    CompaniesHouseLine2 = "Test Park",
                    TownOrCity = "Test Town",
                    County = "Testshire",
                    PostCode = "TE51 3TS"
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

            //Assert
            Assert.IsNotNull(results.FirstOrDefault());
        }

        [Test]
        [TestCase("", OrganisationStatus.None)]
        [TestCase(null, OrganisationStatus.None)]
        [TestCase("active", OrganisationStatus.Active)]
        [TestCase("dissolved", OrganisationStatus.Dissolved)]
        [TestCase("liquidation", OrganisationStatus.Liquidation)]
        [TestCase("receivership", OrganisationStatus.Receivership)]
        [TestCase("administration", OrganisationStatus.Administration)]
        [TestCase("voluntary-arrangement", OrganisationStatus.VoluntaryArrangement)]
        [TestCase("converted-closed", OrganisationStatus.ConvertedClosed)]
        [TestCase("insolvency-proceedings", OrganisationStatus.InsolvencyProceedings)]
        public async Task ShouldSetOrganisationStatus(string companiesHouseStatus, OrganisationStatus expectedMappedStatus)
        {
            //Arrange
            const string searchTerm = "test";
            var resultItem = new CompanySearchResultsItem
            {
                CompanyName = "Test Company",
                Address = new Domain.Models.Company.Address
                {
                    Premises = "12",
                    CompaniesHouseLine1 = "Test Street",
                    CompaniesHouseLine2 = "Test Park",
                    TownOrCity = "Test Town",
                    County = "Testshire",
                    PostCode = "TE51 3TS"
                },
                DateOfIncorporation = DateTime.Now,
                CompanyNumber = "12345678",
                CompanyStatus = companiesHouseStatus
            };

            _verificationService.Setup(x => x.FindCompany(It.IsAny<string>(), 10)).ReturnsAsync(new CompanySearchResults
            {
                Companies = new List<CompanySearchResultsItem> { resultItem }
            });

            //Act
            var results = await _searchService.Search(searchTerm, 10);

            //Assert
            Assert.AreEqual(expectedMappedStatus, results.FirstOrDefault().OrganisationStatus);
        }

        [TestCase("12", "Test Street", "TestPark", "Test Town", "Testshire", "TE51 3TS")]
        [TestCase(null, "Test Street", "TestPark", "Test Town", "Testshire", "TE51 3TS")]
        [TestCase("12", "Test Street", null, "Test Town", "Testshire", "TE51 3TS")]
        [TestCase(null, "Test Street", null, "Test Town", "Testshire", "TE51 3TS")]
        public async Task ShouldFormatAddressCorrectlyForFoundCompanies(string premises, string companiesHouseLine1, string companiesHouseLine2, 
            string townOrCity, string county, string postcode)
        {
            //Arrange
            const string searchTerm = "Test";
            var resultItem = new CompanySearchResultsItem
            {
                CompanyName = "Test Company",
                Address = new Domain.Models.Company.Address
                {
                    Premises = premises,
                    CompaniesHouseLine1 = companiesHouseLine1,
                    CompaniesHouseLine2 = companiesHouseLine2,
                    TownOrCity = townOrCity,
                    County = county,
                    PostCode = postcode
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

            Assert.AreEqual(!string.IsNullOrEmpty(resultItem.Address.Premises) ? resultItem.Address.Premises : resultItem.Address.CompaniesHouseLine1, organisation.Address.Line1);
            Assert.AreEqual(!string.IsNullOrEmpty(resultItem.Address.Premises) ? resultItem.Address.CompaniesHouseLine1 : resultItem.Address.CompaniesHouseLine2, organisation.Address.Line2);
            Assert.AreEqual(!string.IsNullOrEmpty(resultItem.Address.Premises) ? resultItem.Address.CompaniesHouseLine2 : null, organisation.Address.Line3);
            Assert.AreEqual(resultItem.Address.TownOrCity, organisation.Address.Line4);
            Assert.AreEqual(resultItem.Address.County, organisation.Address.Line5);
            Assert.AreEqual(resultItem.Address.PostCode, organisation.Address.Postcode);

            Assert.AreEqual(resultItem.CompanyNumber, organisation.Code);
            Assert.AreEqual(resultItem.DateOfIncorporation, organisation.RegistrationDate);
            Assert.AreEqual(OrganisationType.Company, organisation.Type);
            Assert.AreEqual(OrganisationSubType.None, organisation.SubType);
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
