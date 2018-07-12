using System;
using System.Linq;
using HtmlAgilityPack;
using SFA.DAS.NLog.Logger;
using SFA.DAS.ReferenceData.Domain.Models;
using SFA.DAS.ReferenceData.Types;

namespace SFA.DAS.ReferenceData.PublicSectorOrgs.WebJob.Updater
{
    public class PublicSectorOrganisationHtmlScraper : IPublicSectorOrganisationHtmlScraper
    {
        public PublicSectorOrganisationLookUp Scrape(string url, ILog logger)
        {
            var ol = new PublicSectorOrganisationLookUp();
            try
            {
                var web = new HtmlWeb();
                var doc = web.Load(url);

                var englandPolice = System.Net.WebUtility.HtmlDecode(doc.DocumentNode.SelectNodes("//*[@id=\"england\"]/ul")[0].InnerText).Split('\n')
                    .Where(x => !string.IsNullOrWhiteSpace(x)).Select(s => s.Trim());
                var nationalPolice = System.Net.WebUtility.HtmlDecode(doc.DocumentNode.SelectNodes("//*[@id=\"special\"]/ul")[0].InnerText).Split('\n')
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
                logger.Error(e, "Cannot get Police organisations, potential format change");
                throw;
            }

            return ol;
        }
    }
}