using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using SFA.DAS.ReferenceData.Domain.Interfaces.Services;
using SFA.DAS.ReferenceData.Domain.Models.Bcp;

namespace SFA.DAS.ReferenceData.Infrastructure.Services
{
    public class BcpService : IBcpService
    {
        private readonly ILogger _logger;

        public BcpService() //ILogger logger
        {
            //_logger = logger; //todo: put this back
        }

        public async Task ExecuteBcp(BcpRequest request)
        {
            var login = request.UseTrustedConnection ? "-T" : $"U{request.Username} -P{request.Password}";

            var bcpArgs = $"[{request.TargetDb}].[{request.TargetSchema}].[{request.TargetTable}]" +
                          $" in {request.SourceFile} -S{request.ServerName} {login} -t{request.FieldTerminator} -r{request.RowTerminator} -c";

            var bcpProcessInfo = new ProcessStartInfo
            {
                UseShellExecute = false,
                //WorkingDirectory = regPlusExtracts,
                FileName = "bcp",
                Verb = "runas",
                //Arguments = "/c " + bcp, //auto close
                Arguments = bcpArgs,
                WindowStyle = ProcessWindowStyle.Normal, //todo: hide window
                RedirectStandardOutput = true
            };

            var output = "";

            var process = Process.Start(bcpProcessInfo);
            using (process)
            {
                output = process.StandardOutput.ReadToEnd();
                //_logger.Info(process.StandardOutput); //todo: log to logger
                process.WaitForExit();
                process.Close();
            }

        }
    }
}
