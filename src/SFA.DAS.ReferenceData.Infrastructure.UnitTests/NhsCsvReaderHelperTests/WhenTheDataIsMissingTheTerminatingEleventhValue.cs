using NUnit.Framework;
using SFA.DAS.ReferenceData.Infrastructure.Services;
using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;


namespace SFA.DAS.ReferenceData.Infrastructure.UnitTests.NhsCsvReaderHelperTests
{
    public class WhenTheDataIsMissingTheTerminatingEleventhValue : NhsCsvReaderHelperTestBase
    {
        public override void RefineSetup()
        {
            FileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
                {
                    { FilePath, new MockFileData($@"
                        ,Name1,OrganisationCode1,,AddressLine1,AddressLine2,AddressLine3,AddressLine4,AddressLine5,PostCode,,
                        ,Name1,OrganisationCode1,,AddressLine1,AddressLine2,AddressLine3,AddressLine4,AddressLine5,PostCode,,
                        ,Name3,OrganisationCode3,,AddressLine1,AddressLine2,AddressLine3,AddressLine4,AddressLine5,PostCode,,{string.Empty}") }
                });
            Subject = new NhsCsvReaderHelper(FileSystem);
        }

        [Test]
        public void ThenTheLastLineWouldBeIncludedInTheDataProablyUnanticipatedCase()
        {
            var organisations = Subject.ReadNhsFile(FilePath);
            Assert.AreEqual(3, organisations.Count);
        }
    }
}
