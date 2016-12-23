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

        public BcpService(ILogger logger)
        {
            _logger = logger;
        }

        public void ExecuteBcp(BcpRequest request)
        {
            var login = request.UseTrustedConnection ? "-T" : $"-U{request.Username} -P{request.Password}";

            var files = Directory.EnumerateFiles(request.SourceDirectory, "*.bcp", SearchOption.AllDirectories).ToList();

            _logger.Info($"{files.Count} files found for import in {request.SourceDirectory}");

            if (!files.Any())
            {
                throw new InvalidOperationException("Import aborted - no files found in directory");
            }

            var totalStopwatch = Stopwatch.StartNew();

            foreach (var file in files)
            {
                var extractName = Path.GetFileNameWithoutExtension(file);
                var stopwatch = Stopwatch.StartNew();
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

                        _logger.Info(process.StandardOutput.ReadToEnd());

                        process.WaitForExit();

                        if (process.ExitCode != 0)
                        {
                            throw new InvalidOperationException($"BCP ended with exit code {process.ExitCode}");
                        }

                        process.Close();
                    }
                    stopwatch.Stop();
                    _logger.Info($"BCP complete for {extractName}: {stopwatch.Elapsed} elapsed");
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "BCP error");
                    throw;
                }
            }

            totalStopwatch.Stop();
            _logger.Info($"BCP complete for all files: {totalStopwatch.Elapsed} elapsed");
        }

    }
}
