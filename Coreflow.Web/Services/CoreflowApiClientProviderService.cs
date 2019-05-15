using Coreflow.Api;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

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
