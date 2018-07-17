using System;

namespace SFA.DAS.ReferenceData.Types.DTO
{
    public class Charity
    {
        public int RegistrationNumber { get; set; }
        public string Name { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Address4 { get; set; }
        public string Address5 { get; set; }
        public string PostCode { get; set; }
        public DateTime RegistrationDate { get; set; }
        public bool IsRemoved { get; set; }
    }
}
