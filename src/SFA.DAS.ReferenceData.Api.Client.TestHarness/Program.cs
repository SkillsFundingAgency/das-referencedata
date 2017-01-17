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
                ClientId = "67d6bbe9-e0c1-4d00-90af-a22574899a67",
                ClientSecret = "vzee5FCXtfeV7orFYPjYoYyFsb8Wj6hX/iL9j3TlGfA=",
                IdentifierUri = "https://citizenazuresfabisgov.onmicrosoft.com/refdata-api",
                Tenant = "citizenazuresfabisgov.onmicrosoft.com",
                //DatabaseConnectionString= "",
                //ServiceBusConnectionString= ""
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
