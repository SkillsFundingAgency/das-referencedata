using NUnit.Framework;
using SFA.DAS.ReferenceData.Infrastructure.Services;
using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;


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
                        ,Name2,OrganisationCode2,,AddressLine1,AddressLine2,AddressLine3,AddressLine4,AddressLine5,PostCode,,
                        ,Name1,OrganisationCode2,,AddressLine1,AddressLine2,AddressLine3,AddressLine4,AddressLine5,PostCode,,
                        ,Name3,OrganisationCode3,,AddressLine1,AddressLine2,AddressLine3,AddressLine4,AddressLine5,PostCode,,end") }
                });
            Subject = new NhsCsvReaderHelper(FileSystem);
        }

        [Test]
        public void ThenTheOrganisationcodesAreDependentOnTheData()
        {
            var organisations = Subject.ReadNhsFile(FilePath);
            Assert.AreEqual(4, organisations.Count);
            Assert.AreEqual(organisations[3].OrganisationCode, organisations[0].OrganisationCode);
            Assert.AreEqual(organisations[2].OrganisationCode, organisations[1].OrganisationCode);
            Assert.AreNotEqual(organisations[0].OrganisationCode, organisations[1].OrganisationCode);
            Assert.AreEqual(39, organisations[1].OrganisationCode.Length);
        }
    }
}
