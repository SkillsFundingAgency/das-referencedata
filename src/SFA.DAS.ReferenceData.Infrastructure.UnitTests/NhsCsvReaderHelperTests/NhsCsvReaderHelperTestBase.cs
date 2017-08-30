using SFA.DAS.ReferenceData.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public NhsCsvReaderHelperTestBase()
        {
            RefineSetup();
        }

        public abstract void RefineSetup();
    }
}
