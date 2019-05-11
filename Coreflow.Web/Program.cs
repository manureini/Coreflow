using Coreflow.Storage;
using Coreflow.Storage.ArgumentInjection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace Coreflow.Web
{
    public class Program
    {
        public static Coreflow CoreflowInstance;

        public static void Main(string[] args)
        {

            var configureNamedOptions = new ConfigureNamedOptions<ConsoleLoggerOptions>("", null);
            var optionsFactory = new OptionsFactory<ConsoleLoggerOptions>(new[] { configureNamedOptions }, Enumerable.Empty<IPostConfigureOptions<ConsoleLoggerOptions>>());
            var optionsMonitor = new OptionsMonitor<ConsoleLoggerOptions>(optionsFactory, Enumerable.Empty<IOptionsChangeTokenSource<ConsoleLoggerOptions>>(), new OptionsCache<ConsoleLoggerOptions>());
            var loggerFactory = new LoggerFactory(new[] { new ConsoleLoggerProvider(optionsMonitor) }, new LoggerFilterOptions { MinLevel = LogLevel.Trace });

            CoreflowInstance = new Coreflow(
                new SimpleFlowDefinitionFileStorage(@"Flows"),
                new SimpleFlowInstanceFileStorage("FlowInstances"),
                new JsonFileArgumentInjectionStore("Arguments.json"),
                "Plugins",
                loggerFactory
               );

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
