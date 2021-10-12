using Dapper.FluentMap.Mapping;
using System;

namespace SFA.DAS.ReferenceData.Domain.Models.Charity
{
    class CharityStagingTableModel
    {

    }    

    public class ExistingCharity
    {
        public int regno { get; set; }
        public int subno { get; set; }
        public string name { get; set; }
        public string orgtype { get; set; }
        public string gd { get; set; }
        public string aob { get; set; }
        public int aob_defined { get; set; }
        public string nhs { get; set; }
        public int ha_no { get; set; }
        public string corr { get; set; }
        public string add1 { get; set; }
        public string add2 { get; set; }
        public string add3 { get; set; }
        public string add4 { get; set; }
        public string add5 { get; set; }
        public string postcode { get; set; }
        public string phone { get; set; }
        public int fax { get; set; }
    }

    public class ExistingCharityRegistration
    {
        //[Column("regno")]
        public int regno { get; set; }
        //[Column("subno")]
        public int subno { get; set; }
        //[Column("regdate")]
        public DateTime? regdate { get; set; }
        //[Column("remdate")]
        public DateTime? remdate { get; set; }
        //[Column("remcode")]
        public string remcode { get; set; }
    }

    public class ExistingCharityRegistrationMap : EntityMap<ExistingCharityRegistration>
    {
        public ExistingCharityRegistrationMap()
        {
            // Map property 'Name' to column 'strName'.
            //Map(p => p.RegNo).ToColumn("regno");
            //Map(p => p.SubNo).ToColumn("subno");

            // Ignore the 'LastModified' property when mapping.
            //Map(p => p.SubNo)
            //    .Ignore();
        }
    }

    /* public class ExistingCharityRegistration
     {
         [Column("SiteName")]
         public int regno { get; set; }
         public int subno { get; set; }
         public DateTime? regdate { get; set; }
         public DateTime? remdate { get; set; }
         public string remcode { get; set; }
     }*/

    public class ExistingMainCharity
    {
        public int regno { get; set; }
        public string coyno { get; set; }
        public string trustees { get; set; }
        public string fyend { get; set; }
        public string welsh { get; set; }
        public DateTime? incomedate { get; set; }
        public decimal? income { get; set; }
        public string grouptype { get; set; }
        public string email { get; set; }
        public string web { get; set; }
    }
}
