using Coreflow.Storage;
using Coreflow.Storage.ArgumentInjection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

namespace Coreflow.Web
{
    public class Program
    {
        public static Coreflow CoreflowInstance;

        public static void Main(string[] args)
        {
            CoreflowInstance = new Coreflow(
                new SimpleFlowDefinitionFileStorage(@"Flows"),
                new SimpleFlowInstanceFileStorage("FlowInstances"),
                new JsonFileArgumentInjectionStore("Arguments.json"),
                "Plugins");

            Thread flowThread = new Thread(() =>
            {
                Guid? identifier = CoreflowInstance.GetFlowIdentifier("init");

                if (identifier == null)
                {
                    Console.WriteLine("init flow not found!");
                    return;
                }

                try
                {
                    CoreflowInstance.CompileFlows();
                    CoreflowInstance.RunFlow(identifier.Value);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    Console.WriteLine("Thread crashed!!!");
                }
            });

            flowThread.IsBackground = true;
            flowThread.Start();

            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseUrls("http://0.0.0.0:5700");
    }
}
