using SFA.DAS.NLog.Logger;
using SFA.DAS.ReferenceData.Domain.Models;

namespace SFA.DAS.ReferenceData.PublicSectorOrgs.WebJob.Updater
{
    public interface IPublicSectorOrganisationHtmlScraper
    {
        PublicSectorOrganisationLookUp Scrape(string url, ILog logger);
    }
}
