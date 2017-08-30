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
                    { FilePath, new MockFileData(@",Name,OrganisationCode1,,AddressLine1,AddressLine2,AddressLine3,AddressLine4,AddressLine5,PostCode,,
                        ,Name,OrganisationCode2,,AddressLine1,AddressLine2,AddressLine3,AddressLine4,AddressLine5,PostCode,,
                        ,Name,OrganisationCode3,,AddressLine1,AddressLine2,AddressLine3,AddressLine4,AddressLine5,PostCode,,end") }
                });
            Subject = new NhsCsvReaderHelper(FileSystem);
        }

        [Test]
        public void ThenICanSeeThatItWorks()
        {
            var organisations = Subject.ReadNhsFile(FilePath);
            Assert.AreEqual(2, organisations.Count);
            Assert.AreEqual("OrganisationCode1", organisations[0].OrganisationCode);
            Assert.AreEqual("OrganisationCode2", organisations[1].OrganisationCode);
        }
    }
}
