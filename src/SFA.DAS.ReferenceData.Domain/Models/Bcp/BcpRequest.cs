using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public string TargetTable { get; set; }
        public string SourceFile { get; set; }
        public string FieldTerminator { get; set; }
        public string RowTerminator { get; set; }

    }
}
