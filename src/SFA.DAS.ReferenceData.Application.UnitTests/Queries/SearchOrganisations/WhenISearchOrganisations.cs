using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.ReferenceData.Api.Client.Dto;
using SFA.DAS.ReferenceData.Application.Queries.SearchOrganisations;
using SFA.DAS.ReferenceData.Domain.Interfaces.Services;
using SFA.DAS.ReferenceData.Domain.Models.Organisation;

namespace SFA.DAS.ReferenceData.Application.UnitTests.Queries.SearchOrganisations
{
    class WhenISearchOrganisations
    {
        private Mock<IOrganisationTextSearchService> _textSearchService1;
        private Mock<IOrganisationTextSearchService> _textSearchService2;
        private Mock<IOrganisationReferenceSearchService> _referenceSearchService1;
        private Mock<IOrganisationReferenceSearchService> _referenceSearchService2;
        private SearchOrganisationsHandler _handler;
        
        [SetUp]
        public void Arrange()
        {
            _textSearchService1 = new Mock<IOrganisationTextSearchService>();
            _textSearchService2 = new Mock<IOrganisationTextSearchService>();

            _referenceSearchService1 = new Mock<IOrganisationReferenceSearchService>();
            _referenceSearchService2 = new Mock<IOrganisationReferenceSearchService>();

            var textSearchServices = new List<IOrganisationTextSearchService> { _textSearchService1.Object, _textSearchService2.Object };
            var referenceSearchServices = new List<IOrganisationReferenceSearchService> { _referenceSearchService1.Object, _referenceSearchService2.Object };

            _handler = new SearchOrganisationsHandler(textSearchServices, referenceSearchServices);
        }

        [Test]
        public async Task AndISearchByTextThenAllTextSearchResultsShouldBeReturned()
        {
            //Arrange
            var query = new SearchOrganisationsQuery
            {
                SearchTerm = "test",
                MaximumResults = 500
            };

            var search1Results = new List<Organisation> { new Organisation(), new Organisation() };
            var search2Results = new List<Organisation> { new Organisation() };

            _textSearchService1.Setup(x => x.Search(query.SearchTerm, query.MaximumResults)).ReturnsAsync(search1Results);
            _textSearchService2.Setup(x => x.Search(query.SearchTerm, query.MaximumResults)).ReturnsAsync(search2Results);

            //Act
            var response = await _handler.Handle(query);

            //Assert
            var allResults = new List<Organisation>();
            allResults.AddRange(search1Results);
            allResults.AddRange(search2Results);
            CollectionAssert.AreEqual(allResults, response.Organisations);

            _referenceSearchService1.Verify(x => x.Search(It.IsAny<string>()), Times.Never);
            _referenceSearchService2.Verify(x => x.Search(It.IsAny<string>()), Times.Never);
        }

        [Test]
        public async Task AndISearchByReferenceThenTheMatchingRecordShouldBeReturned()
        {
            //Arrange
            var query = new SearchOrganisationsQuery
            {
                SearchTerm = "test",
                MaximumResults = 500
            };

            var searchResult = new Organisation();

            _referenceSearchService1.Setup(x => x.IsSearchTermAReference(query.SearchTerm)).Returns(true);
            _referenceSearchService2.Setup(x => x.Search(query.SearchTerm)).ReturnsAsync((Organisation)null);
            _referenceSearchService2.Setup(x => x.IsSearchTermAReference(query.SearchTerm)).Returns(true);
            _referenceSearchService2.Setup(x => x.Search(query.SearchTerm)).ReturnsAsync(searchResult);

            //Act
            var response = await _handler.Handle(query);

            //Assert
            Assert.AreEqual(searchResult, response.Organisations.Single());

            _textSearchService1.Verify(x => x.Search(It.IsAny<string>(), It.IsAny<int>()), Times.Never);
            _textSearchService2.Verify(x => x.Search(It.IsAny<string>(), It.IsAny<int>()), Times.Never);
        }

        [Test]
        public async Task AndMoreThanTheMaximumResultsAreFoundThenTheMaximumNumberOfResultsAreReturned()
        {
            var query = new SearchOrganisationsQuery
            {
                SearchTerm = "test",
                MaximumResults = 3
            };

            var search1Results = new List<Organisation> { new Organisation(), new Organisation() };
            var search2Results = new List<Organisation> { new Organisation(), new Organisation() };

            _textSearchService1.Setup(x => x.Search(query.SearchTerm, query.MaximumResults)).ReturnsAsync(search1Results);
            _textSearchService2.Setup(x => x.Search(query.SearchTerm, query.MaximumResults)).ReturnsAsync(search2Results);

            //Act
            var response = await _handler.Handle(query);

            //Assert
            var expectedResults = new List<Organisation>();
            expectedResults.AddRange(search1Results);
            expectedResults.Add(search2Results.First());
            CollectionAssert.AreEqual(expectedResults, response.Organisations);
        }
    }
}
