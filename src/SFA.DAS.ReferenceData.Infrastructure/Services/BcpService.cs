﻿using System;
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

        public BcpService(ILogger logger)
        {
            _logger = logger;
        }

        public void ExecuteBcp(BcpRequest request)
        {
            var login = request.UseTrustedConnection ? "-T" : $"-U{request.Username} -P{request.Password}";

            var files = Directory.EnumerateFiles(request.SourceDirectory, "*.bcp").ToList();

            foreach (var file in files)
            {
                var extractName = Path.GetFileNameWithoutExtension(file);

                _logger.Info($"Beginning BCP for {extractName}");

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

                try
                {
                    using (var process = Process.Start(bcpProcessInfo))
                    {
                        if (process == null)
                        {
                            throw new InvalidOperationException("No process returned for BCP command");
                        }

                        var output = process.StandardOutput.ReadToEnd();

                        process.WaitForExit();

                        if (process.ExitCode != 0)
                        {
                            _logger.Error($"BCP ended with exit code {process.ExitCode}");
                            throw new InvalidOperationException(output);
                        }

                        process.Close();
                       
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "BCP error");
                    throw;
                }
            }
        }

    }
}