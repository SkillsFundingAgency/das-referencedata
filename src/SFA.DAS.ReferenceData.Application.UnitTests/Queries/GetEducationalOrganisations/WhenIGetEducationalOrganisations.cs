using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.ReferenceData.Application.Queries.GetEducationalOrganisations;
using SFA.DAS.ReferenceData.Domain.Interfaces.Data;
using SFA.DAS.ReferenceData.Domain.Models.Data;
using SFA.DAS.ReferenceData.Types.DTO;

namespace SFA.DAS.ReferenceData.Application.UnitTests.Queries.GetEducationalOrganisations
{
    internal class WhenIGetEducationalOrganisations
    {
        private Mock<IEducationalOrganisationRepository> _educationalOrganisationRepository;
        private FindEducationalOrganisationsHandler _handler;
        private PagedResult<EducationOrganisation> _organisations;
       
        [SetUp]
        public void Arrange()
        {
            _organisations = new PagedResult<EducationOrganisation>
            {
                Data = new List<EducationOrganisation>
                {
                    new EducationOrganisation {Name = "Test Organisation"}
                }
            };

            _educationalOrganisationRepository = new Mock<IEducationalOrganisationRepository>();

            _educationalOrganisationRepository.Setup(x => 
                x.FindOrganisations(
                    It.IsAny<string>(), 
                    It.IsAny<int>(), 
                    It.IsAny<int>())).ReturnsAsync(_organisations);

            _handler = new FindEducationalOrganisationsHandler(_educationalOrganisationRepository.Object);
        }

        [Test]
        public async Task ThenIShouldGetAllOrganisationsFromTheRepository()
        {
            //Arrange
            var query = new FindEducationalOrganisationsQuery
            {
                PageNumber = 2,
                PageSize = 50,
                SearchTerm = "test"
            };


            //Act
            var response = await _handler.Handle(query);

            //Assert
            _educationalOrganisationRepository.Verify(x => x.FindOrganisations(query.SearchTerm, query.PageSize, query.PageNumber), Times.Once);
            Assert.AreEqual(_organisations.Data, response.Organisations.Data);
        }
    }
}
