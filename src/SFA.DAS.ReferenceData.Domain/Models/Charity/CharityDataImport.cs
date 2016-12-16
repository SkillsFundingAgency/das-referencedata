using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.ReferenceData.Domain.Models.Charity
{
    public class CharityDataImport
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public DateTime ImportDate { get; set; }
    }
}
