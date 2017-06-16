using Moq;
using NUnit.Framework;
using SFA.DAS.NLog.Logger;
using SFA.DAS.ReferenceData.Application.Services.OrganisationSearch;
using SFA.DAS.ReferenceData.Domain.Interfaces.Services;

namespace SFA.DAS.ReferenceData.Application.UnitTests.Services.CompanySearchServiceTests
{
    public class WhenICheckIfSearchTermIsAReference
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

        [TestCase("12345678", true)]
        [TestCase("A2345678", true)]
        [TestCase("AA345678", true)]
        [TestCase("AAA45678", false)]
        [TestCase("AAAAAAAA", false)]
        [TestCase("A23456A8", false)]
        [TestCase("1234567", false)]
        [TestCase("123456789", false)]
        [TestCase("", false)]
        public void ShouldReturnIfTermIsAReference(string reference, bool isTerm)
        {
            //Act
            var result = _searchService.IsSearchTermAReference(reference);

            //Assert
            Assert.AreEqual(isTerm, result);
        }
    }
}
