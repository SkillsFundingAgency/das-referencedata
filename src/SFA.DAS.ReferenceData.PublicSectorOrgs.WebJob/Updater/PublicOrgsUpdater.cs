using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Configuration;
using SFA.DAS.ReferenceData.Domain.Configuration;
using NLog;
using System;
using System.Globalization;
using System.Threading.Tasks;
using SFA.DAS.ReferenceData.Domain.Interfaces.Services;
using System.IO;
using HtmlAgilityPack;
using Newtonsoft.Json;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using SFA.DAS.ReferenceData.Domain.Models;

namespace SFA.DAS.ReferenceData.PublicSectorOrgs.WebJob.Updater
{
    public class PublicOrgsUpdater : IPublicOrgsUpdater
    {
        private readonly IArchiveDownloadService _archiveDownloadService;
        private readonly ILogger _logger;
        private readonly ReferenceDataApiConfiguration _configuration;
        private readonly string _workingFolder;
        private readonly string _fileName = "publicsectorclassificationguidelatest";
        private readonly string _jsonContainerName = "sfa-das-reference-data";
        private readonly string _jsonFileName = "PublicOrganisationNames.json";

        public PublicOrgsUpdater(ILogger logger, ReferenceDataApiConfiguration configuration, IArchiveDownloadService archiveDownloadService)
        {
            _archiveDownloadService = archiveDownloadService;
            _logger = logger;
            _configuration = configuration;
            _workingFolder = Path.GetTempPath();
            _logger.Info($"Using temporary folder: {_workingFolder}");
        }

        public async Task RunUpdate()
        {
            _logger.Info("Running Public Organisations updater");

            if(string.IsNullOrWhiteSpace(_configuration.NHSTrustsUrl) || 
                string.IsNullOrWhiteSpace(_configuration.PoliceForcesUrl) || 
                string.IsNullOrWhiteSpace(_configuration.ONSUrl))
            {
                _logger.Error("Missing configuration, check table storage configuration for NHSTrustsUrl, PoliceForcesUrl and ONSUrl");
                throw new Exception("Missing configuration, check table storage configuration for NHSTrustsUrl, PoliceForcesUrl and ONSUrl");
            }

            var onsOrgs = await GetOnsOrganisations();
            var policeOrgs = GetPoliceOrganisations();
            var nhsOrgs = GetNhsOrganisations();

            var orgs = new PublicSectorOrganisationLookUp();
            orgs.Organisations = onsOrgs.Organisations.Concat(policeOrgs.Organisations).Concat(nhsOrgs.Organisations).ToList();
            var jsonFilePath = Path.Combine(_workingFolder, _jsonFileName);


            ExportFile(jsonFilePath, orgs);
            UploadJsonToStorage(jsonFilePath);
        }

        private async Task<PublicSectorOrganisationLookUp> GetOnsOrganisations()
        {
            
            var url = GetDownloadUrlForMonthYear(false);
            _logger.Info($"Downloading ONS from {url}");
            if (!await _archiveDownloadService.DownloadFile(url, _workingFolder, _fileName))
            {
                _logger.Info($"Downloading ONS from {url}");
                url = GetDownloadUrlForMonthYear(true);
                if (!await _archiveDownloadService.DownloadFile(url, _workingFolder, _fileName))
                {
                    _logger.Error($"Failed to download ONS from current and previous month, potential URL format change");
                    throw new Exception($"Failed to download ONS from current and previous month, potential URL format change");
                }
            }

            var excelFile = Path.Combine(_workingFolder, _fileName);

            _logger.Info($"Reading ONS from {excelFile}");
            var ol = new PublicSectorOrganisationLookUp();

            try
            {
                string connectionString = $"Provider=Microsoft.Jet.OLEDB.4.0;Extended Properties='Excel 8.0;HDR=NO;';Data Source={excelFile}";

                using (var conn = new OleDbConnection(connectionString))
                {
                    using (var cmd = new OleDbCommand())
                    {
                        conn.Open();
                        cmd.Connection = conn;
                        
                        var dtSchema = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                        var sheetName = "Index$";
                        cmd.CommandText = "SELECT F1, F2 FROM [" + sheetName + "] WHERE F1 IS NOT NULL AND F1 <> 'Index'";

                        var dt = new DataTable(sheetName);
                        var da = new OleDbDataAdapter(cmd);
                        da.Fill(dt);

                        var rowDel = dt.Rows[0];
                        dt.Rows.Remove(rowDel);

                        var data = dt.AsEnumerable();
                        ol.Organisations = data.Select(x =>
                                    new PublicSectorOrganisation
                                    {
                                        Name = x.Field<string>("F1"),
                                        Sector = x.Field<string>("F2"),
                                        Source = DataSource.Ons
                                    }).ToList();
                        conn.Close();
                    }
                }
            }
            catch (Exception e)
            {
                _logger.Error(e, "Cannot get ONS organisations, potential format change");
                throw new Exception("Cannot get ONS organisations, potential format change", e);
            }
            return ol;
        }

        private PublicSectorOrganisationLookUp GetPoliceOrganisations()
        {
            _logger.Info($"Getting Police Organisations");
            var ol = new PublicSectorOrganisationLookUp();

            try
            {
                var policeUrl = _configuration.PoliceForcesUrl;
                var web = new HtmlWeb();
                var doc = web.Load(policeUrl);
                var englandPolice = doc.DocumentNode.SelectNodes("//*[@id=\"england\"]/ul")[0].InnerText.Split('\n')
                    .Where(x => !string.IsNullOrWhiteSpace(x)).Select(s => s.Trim());
                var nationalPolice = doc.DocumentNode.SelectNodes("//*[@id=\"special\"]/ul")[0].InnerText.Split('\n')
                    .Where(x => !string.IsNullOrWhiteSpace(x)).Select(s => s.Trim());

                ol.Organisations = englandPolice.Concat(nationalPolice)
                    .Select(x => new PublicSectorOrganisation
                    {
                        Name = x,
                        Sector = "",
                        Source = DataSource.Police
                    }).ToList();
            }
            catch (Exception e)
            {
                _logger.Error(e, "Cannot get Police organisations, potential format change");
                throw new Exception("Cannot get Police organisations, potential format change", e);
            }

            return ol;
        }

        private PublicSectorOrganisationLookUp GetNhsOrganisations()
        {
            var ol = new PublicSectorOrganisationLookUp();
            try
            {
                var nhsUrl = _configuration.NHSTrustsUrl;
                var web = new HtmlWeb();
                var doc = web.Load(nhsUrl);
                var nhsOrgs = doc.DocumentNode.SelectNodes("//*[@id=\"content\"]/div[3]/div[1]/div/table")[0].InnerText.Split('\n').Select(s => s.Trim())
                    .Where(x => !string.IsNullOrWhiteSpace(x) && x.ToLower() != "organisation");

                ol.Organisations = nhsOrgs
                    .Select(x => new PublicSectorOrganisation
                    {
                        Name = x,
                        Sector = "",
                        Source = DataSource.Nhs
                    })?.ToList();
            }
            catch (Exception e)
            {
                _logger.Error(e, "Cannot get NHS organisations, potential format change");
                throw new Exception("Cannot get NHS organisations, potential format change", e);
            }
            return ol;
        }

        private string GetDownloadUrlForMonthYear(bool previousMonth)
        {
            var urlpattern = _configuration.ONSUrl;

            var now = previousMonth ? DateTime.Now.AddMonths(-1) : DateTime.Now;
                      

            var monthyear = $"{CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(now.Month)}{now.Year}";

            var url = string.Format(urlpattern, monthyear.ToLower());
            return url;
        }

        private void ExportFile(string filename, PublicSectorOrganisationLookUp orgs)
        {
            try
            {
                using (var fs = File.Open(filename, FileMode.Create))
                {
                    using (var sw = new StreamWriter(fs))
                    {
                        using (var jw = new JsonTextWriter(sw))
                        {
                            jw.Formatting = Formatting.Indented;

                            var serializer = new JsonSerializer();
                            serializer.Serialize(jw, orgs);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"An error occurred exporting data to {filename}: {ex.Message}");
                throw new Exception($"An error occurred exporting data to {filename}: {ex.Message}");
            }

            Console.WriteLine($"Exported {orgs.Organisations.Count()} records");
        }

        private void UploadJsonToStorage(string filePath)
        {
            _logger.Info($"Uploading {filePath} to Blob storage");

            try
            {
                var storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString"]);

                var blobClient = storageAccount.CreateCloudBlobClient();
                var container = blobClient.GetContainerReference(_jsonContainerName);
                container.CreateIfNotExists();

                var blockBlob = container.GetBlockBlobReference(_jsonFileName);

                using (var fileStream = File.OpenRead(filePath))
                {
                    blockBlob.UploadFromStream(fileStream);
                }
            }
            catch (Exception e)
            {
                _logger.Error(e, $"Error uploading {filePath} to Blob storage");
                throw new Exception($"Error uploading {filePath} to Blob storage", e);
            }
        }
    }
}
