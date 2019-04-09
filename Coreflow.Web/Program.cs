using Coreflow.Storage;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Coreflow.Web
{
    public class Program
    {
        public static Coreflow CoreflowInstance;

        public static void Main(string[] args)
        {
            CoreflowInstance = new Coreflow(new SimpleFlowDefinitionFileStorage(@"C:\tmp\Flows"));
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseUrls("http://0.0.0.0:5700");
    }
}
