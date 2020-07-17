using Coreflow.Api;
using System;
using System.Net;

namespace Coreflow.Web.Services
{
    public class CoreflowApiClientProviderService
    {
        public CoreflowApiClient ApiClient { get; protected set; }

        public CoreflowApiClientProviderService()
        {
            try
            {
                ApiClient = new CoreflowApiClient(IPAddress.Loopback, 54321);
                ApiClient.Connect();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
