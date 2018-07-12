namespace SFA.DAS.ReferenceData.Domain.Models.Bcp
{
    public class BcpRequest
    {
        public string ServerName { get; set; }
        public bool UseTrustedConnection { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string TargetDb { get; set; }
        public string TargetSchema { get; set; }
        public string SourceDirectory { get; set; }
        public string FieldTerminator { get; set; }
        public string RowTerminator { get; set; }

    }
}
