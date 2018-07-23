using System;

namespace SFA.DAS.ReferenceData.Domain.Models.Charity
{
    public class CharityDataImport
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public DateTime ImportDate { get; set; }
    }
}
