namespace SFA.DAS.ReferenceData.Domain.Models
{
    public class PublicSectorOrganisation
    {
        public string Name { get; set; }
        public DataSource Source { get; set; }
        public string Sector { get; set; }
    }

    public enum DataSource
    {
        Ons = 1,
        Nhs = 2,
        Police = 3
    }
}
