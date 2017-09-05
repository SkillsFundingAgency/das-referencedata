using SFA.DAS.ReferenceData.Infrastructure.Services;
using System.IO.Abstractions;
using SFA.DAS.ReferenceData.Domain.Models;

namespace SFA.DAS.ReferenceData.Infrastructure.UnitTests.NhsCsvReaderHelperTests
{
    public abstract class NhsCsvReaderHelperTestBase
    {
        protected NhsCsvReaderHelper  Subject;
        protected IFileSystem FileSystem;
        protected const string FilePath= @"c:\temp\myfile.csv";

        protected PublicSectorOrganisation OrganisationData;

        /// <summary>
        /// Sets up a NhsCsvReaderHelper with no properties set. You should set them in 'RefineSetup'
        /// </summary>
        protected NhsCsvReaderHelperTestBase() => RefineSetup();

        public abstract void RefineSetup();
    }
}
