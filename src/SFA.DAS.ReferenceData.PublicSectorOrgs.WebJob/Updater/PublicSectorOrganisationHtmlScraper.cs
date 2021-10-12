using System;
using System.Linq;
using HtmlAgilityPack;
using SFA.DAS.NLog.Logger;
using SFA.DAS.ReferenceData.Domain.Models;
using SFA.DAS.ReferenceData.Types.DTO;

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

                var englandPolice = doc.DocumentNode.SelectNodes("//*[@id=\"wsite-content\"]/div/div/div/div/div/div[3]/ul/li/a")
                    .Where(p => !string.IsNullOrWhiteSpace(p.InnerText))
                    .Select(p => p.InnerText.Trim());

                var nationalPolice = doc.DocumentNode.SelectNodes("//*[@id=\"wsite-content\"]/div/div/div/div/div/div[11]/ul/li")
                    .Where(p => !string.IsNullOrWhiteSpace(p.InnerText))
                    .Select(p => p.InnerText.Trim());

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