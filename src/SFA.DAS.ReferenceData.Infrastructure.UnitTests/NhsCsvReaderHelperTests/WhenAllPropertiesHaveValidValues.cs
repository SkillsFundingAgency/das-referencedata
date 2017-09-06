using NUnit.Framework;
using SFA.DAS.ReferenceData.Infrastructure.Services;
using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;


namespace SFA.DAS.ReferenceData.Infrastructure.UnitTests.NhsCsvReaderHelperTests
{
    public class WhenAllPropertiesHaveValidValues : NhsCsvReaderHelperTestBase
    {
        public override void RefineSetup()
        {
            FileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
                {
                    { FilePath, new MockFileData(@"
                        ,Name1,OrganisationCode1,,AddressLine1,AddressLine2,AddressLine3,AddressLine4,AddressLine5,PostCode,,
                        ,Name2,OrganisationCode2,,AddressLine1,AddressLine2,AddressLine3,AddressLine4,AddressLine5,PostCode,,
                        ,Name3,OrganisationCode3,,AddressLine1,AddressLine2,AddressLine3,AddressLine4,AddressLine5,PostCode,,
                        ,Name4,OrganisationCode4,,AddressLine1,AddressLine2,AddressLine3,AddressLine4,AddressLine5,PostCode,,
                        ,Name5,OrganisationCode5,,AddressLine1,AddressLine2,AddressLine3,AddressLine4,AddressLine5,PostCode,,end") }
                });
            Subject = new NhsCsvReaderHelper(FileSystem);
        }

        [Test]
        public void ThenTheOrganisationCodesAreComposedOfTheOrganisationCodeAndTheName()
        {
            var organisations = Subject.ReadNhsFile(FilePath);
            Assert.AreEqual(4, organisations.Count);
            Assert.IsTrue(organisations.Any(a => a.OrganisationCode == "OrganisationCode1Name1"));
            Assert.IsTrue(organisations.Any(a => a.OrganisationCode == "OrganisationCode2Name2"));
            Assert.IsTrue(organisations.Any(a => a.OrganisationCode == "OrganisationCode3Name3"));
            Assert.IsTrue(organisations.Any(a => a.OrganisationCode == "OrganisationCode4Name4"));
        }
    }
}
