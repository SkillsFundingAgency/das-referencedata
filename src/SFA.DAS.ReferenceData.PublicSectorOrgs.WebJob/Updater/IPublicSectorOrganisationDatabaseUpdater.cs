using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFA.DAS.NLog.Logger;
using SFA.DAS.ReferenceData.Domain.Models;

namespace SFA.DAS.ReferenceData.PublicSectorOrgs.WebJob.Updater
{
    public interface IPublicSectorOrganisationDatabaseUpdater
    {
        PublicSectorOrganisationLookUp UpdateDatabase(string excelFile);
    }

    public class PublicSectorOrganisationDatabaseUpdater : IPublicSectorOrganisationDatabaseUpdater
    {
        private readonly ILog _logger;

        public PublicSectorOrganisationDatabaseUpdater(ILog logger)
        {
            _logger = logger;
        }
        public PublicSectorOrganisationLookUp UpdateDatabase(string excelFile)
        {
            var ol = new PublicSectorOrganisationLookUp();

            try
            {
                var connectionString = $"Provider=Microsoft.Jet.OLEDB.4.0;Extended Properties='Excel 8.0;HDR=NO;';Data Source={excelFile}";

                using (var conn = new OleDbConnection(connectionString))
                {
                    using (var cmd = new OleDbCommand())
                    {
                        conn.Open();
                        cmd.Connection = conn;

                        conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                        const string sheetName = "Index$";
                        cmd.CommandText = "SELECT F1, F2 FROM [" + sheetName + "] WHERE F1 IS NOT NULL AND F2 IS NOT NULL AND F1 <> 'Index'";

                        var dt = new DataTable(sheetName);
                        var da = new OleDbDataAdapter(cmd);
                        da.Fill(dt);

                        var rowDel = dt.Rows[0];
                        dt.Rows.Remove(rowDel);

                        var data = dt.AsEnumerable();

                        ol.Organisations = data.Where(s => !s.Field<string>("F2").ToLower().Contains("former")).Select(x =>
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
    }
}
