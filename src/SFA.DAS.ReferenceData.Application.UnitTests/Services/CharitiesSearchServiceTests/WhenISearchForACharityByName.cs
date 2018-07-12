using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.ReferenceData.Types;

namespace SFA.DAS.ReferenceData.Application.UnitTests.Services.CharitiesSearchServiceTests
{
    [TestFixture]
    public class WhenISearchForACharityByName : CharitiesSearchServiceTestsBase
    {
        [Test]
        public async Task ThenTheMatchingOrganisationsAreReturned()
        {
            var searchTerm = "Skills";
            int maximumRecords = 245;

            var expectedOrganisations = new List<Charity>
            {
                new Charity
                {
                    Address1 = "Line1",
                    Address2 = "Line 2",
                    Address3 = "Line 3",
                    Address4 = "Line 4",
                    Address5 = "Line 5",
                    Name = "Name",
                    RegistrationNumber = 123435,
                    PostCode = "AA11AA",
                    RegistrationDate = DateTime.Now.AddYears(-3)
                }
            };

            Repository.Setup(x => x.FindCharities(searchTerm, maximumRecords)).ReturnsAsync(expectedOrganisations);

            var result = await Service.Search(searchTerm, maximumRecords);

            Assert.AreEqual(expectedOrganisations.Count, result.Count());
            Assert.AreEqual(expectedOrganisations.First().Name, result.First().Name);
            Assert.AreEqual(expectedOrganisations.First().Address1, result.First().Address.Line1);
            Assert.AreEqual(expectedOrganisations.First().Address2, result.First().Address.Line2);
            Assert.AreEqual(expectedOrganisations.First().Address3, result.First().Address.Line3);
            Assert.AreEqual(expectedOrganisations.First().Address4, result.First().Address.Line4);
            Assert.AreEqual(expectedOrganisations.First().Address5, result.First().Address.Line5);
            Assert.AreEqual(expectedOrganisations.First().RegistrationNumber.ToString(), result.First().Code);
            Assert.AreEqual(expectedOrganisations.First().PostCode, result.First().Address.Postcode);
            Assert.AreEqual(expectedOrganisations.First().RegistrationDate, result.First().RegistrationDate);
            Assert.AreEqual(null, result.First().Sector);
            Assert.AreEqual(OrganisationSubType.None, result.First().SubType);
            Assert.AreEqual(OrganisationType.Charity, result.First().Type);
        }
    }
}
