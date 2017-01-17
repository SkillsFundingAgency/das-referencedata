using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.ReferenceData.Api.Client.TestHarness
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = new ReferenceDataApiConfiguration
            {
                ApiBaseUrl = "https://localhost:44300/api/organisations/", 
                //...config here
            };
            
            //a-t url: "https://at-refdata.apprenticeships.sfa.bis.gov.uk/api/organisations/",

            var client = new ReferenceDataApiClient(config);

            //Ignore cert fail
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

            //Test methods:
            //var charity = client.GetCharity(1165875).Result;
            var org = client.SearchPublicSectorOrganisation("Chris", 1, 100).Result;

        }


    }
}
