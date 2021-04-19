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

                var englandPolice = doc.DocumentNode.SelectNodes("//*[@id=\"content\"]/div[2]/div/section/div[2]/div[4]/div/div/div[1]/div/div[2]/div/ul/li/a/text()")
                    .Where(p => !string.IsNullOrWhiteSpace(p.InnerText))
                    .Select(p => p.InnerText.Trim());

                var nationalPolice = doc.DocumentNode.SelectNodes("//*[@id=\"content\"]/div[2]/div/section/div[2]/div[4]/div/div/div[5]/div/div[2]/div/ul/li/a/text()")
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