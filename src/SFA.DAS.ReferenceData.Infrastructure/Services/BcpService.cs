using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
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

        public bool ExecuteBcp(BcpRequest request)
        {
            var login = request.UseTrustedConnection ? "-T" : $"U{request.Username} -P{request.Password}";

            var files = Directory.EnumerateFiles(request.SourceDirectory, "*.bcp").ToList();

            foreach (var file in files)
            {
                var extractName = Path.GetFileNameWithoutExtension(file);

                var bcpArgs = $"[{request.TargetDb}].[{request.TargetSchema}].[{extractName}]" +
                              $" in {file} -S{request.ServerName} {login} -t{request.FieldTerminator} -r{request.RowTerminator} -c";

                var bcpProcessInfo = new ProcessStartInfo
                {
                    UseShellExecute = false,
                    FileName = "bcp",
                    Verb = "runas",
                    Arguments = bcpArgs,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    RedirectStandardOutput = true
                };

                var output = "";
                var exitCode = 0;

                try
                {
                    using (var process = Process.Start(bcpProcessInfo))
                    {
                        output = process.StandardOutput.ReadToEnd();
                        //_logger.Info(process.StandardOutput); //todo: log to logger
                        process.WaitForExit();
                        process.Close();
                        exitCode = process.ExitCode;
                    }
                }
                catch (Exception ex)
                {
                    //_logger.Error(ex);
                    return false;
                }

            }

            //look at exit code

            return true;
        }

    }
}
