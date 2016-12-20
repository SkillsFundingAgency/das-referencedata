using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.ReferenceData.Application.Queries.GetCharityByRegistrationNumber;
using SFA.DAS.ReferenceData.Domain.Interfaces.Data;
using SFA.DAS.ReferenceData.Domain.Models.Charity;

namespace SFA.DAS.ReferenceData.Application.UnitTests.Queries.GetCharityByRegistrationNumber
{
    class WhenIGetCharityByRegistratrionNumber
    {
        private GetCharityByRegistrationNumberHandler _handler;
        private Mock<ICharityRepository> _repository;

        [SetUp]
        public void Arrange()
        {
            _repository = new Mock<ICharityRepository>();

            _repository.Setup(x => x.GetCharityByRegistrationNumber(It.Is<int>(i => i == 123)))
                .ReturnsAsync(() => new Charity
                {
                    RegistrationNumber = 123,
                    Name = "Test Charity"
                });

            _repository.Setup(x => x.GetCharityByRegistrationNumber(It.Is<int>(i => i == 456)))
                .ReturnsAsync(() => null);

            _handler = new GetCharityByRegistrationNumberHandler(_repository.Object);
        }

        [Test]
        public async Task ThenIShouldGetAMatchingCharityFromTheRepository()
        {
            //Arrange
            var query = new GetCharityByRegistrationNumberQuery
            {
                RegistrationNumber = 123
            };

            //Act
            var result = await _handler.Handle(query);

            //Assert
            Assert.IsInstanceOf<Charity>(result.Charity);
            Assert.IsNotNull(result.Charity);
        }
    }
}
