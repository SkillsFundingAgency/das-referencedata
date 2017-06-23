using System;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.ReferenceData.Domain.Models.Charity;
using SFA.DAS.ReferenceData.Domain.Models.Organisation;

namespace SFA.DAS.ReferenceData.Application.UnitTests.Services.CharitiesSearchServiceTests
{
    [TestFixture]
    public class WhenISearchForACharityByNumber : CharitiesSearchServiceTestsBase
    {
        [Test]
        public async Task ThenTheMatchingOrganisationIsReturned()
        {
            var registrationNumber = 1234453;
            
            var expectedOrganisation = new Charity
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
            };

            Repository.Setup(x => x.GetCharityByRegistrationNumber(registrationNumber)).ReturnsAsync(expectedOrganisation);

            var result = await Service.Search(registrationNumber.ToString());

            Assert.AreEqual(expectedOrganisation.Name, result.Name);
            Assert.AreEqual(expectedOrganisation.Address1, result.Address.Line1);
            Assert.AreEqual(expectedOrganisation.Address2, result.Address.Line2);
            Assert.AreEqual(expectedOrganisation.Address3, result.Address.Line3);
            Assert.AreEqual(expectedOrganisation.Address4, result.Address.Line4);
            Assert.AreEqual(expectedOrganisation.Address5, result.Address.Line5);
            Assert.AreEqual(expectedOrganisation.RegistrationNumber.ToString(), result.Code);
            Assert.AreEqual(expectedOrganisation.PostCode, result.Address.Postcode);
            Assert.AreEqual(expectedOrganisation.RegistrationDate, result.RegistrationDate);
            Assert.AreEqual(null, result.Sector);
            Assert.AreEqual(OrganisationSubType.None, result.SubType);
            Assert.AreEqual(OrganisationType.Charity, result.Type);
        }

        [Test]
        public async Task AndTheCharityNumberIsNotNumericThenNothingIsReturned()
        {
            var registrationNumber = "A1234453";

            var result = await Service.Search(registrationNumber);

            Assert.IsNull(result);
        }

        [Test]
        public async Task AndTheCharityIsNotFoundThenNothingIsReturned()
        {
            var registrationNumber = 1234453;

            Repository.Setup(x => x.GetCharityByRegistrationNumber(registrationNumber)).ReturnsAsync((Charity)null);

            var result = await Service.Search(registrationNumber.ToString());

            Assert.IsNull(result);
        }
    }
}
