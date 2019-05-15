using Coreflow.Storage;
using Coreflow.Storage.ArgumentInjection;
using FlowNs___cc_a1bfd8b5_d453_4fe0_9825_59cd67f02d5e;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;

namespace Coreflow
{
    class Program
    {
        public static void Main(string[] args)
        {
            Thread.Sleep(3000);

            var configureNamedOptions = new ConfigureNamedOptions<ConsoleLoggerOptions>("", null);
            var optionsFactory = new OptionsFactory<ConsoleLoggerOptions>(new[] { configureNamedOptions }, Enumerable.Empty<IPostConfigureOptions<ConsoleLoggerOptions>>());
            var optionsMonitor = new OptionsMonitor<ConsoleLoggerOptions>(optionsFactory, Enumerable.Empty<IOptionsChangeTokenSource<ConsoleLoggerOptions>>(), new OptionsCache<ConsoleLoggerOptions>());
            var loggerFactory = new LoggerFactory(new[] { new ConsoleLoggerProvider(optionsMonitor) }, new LoggerFilterOptions { MinLevel = LogLevel.Trace });

            Coreflow coreflowInstance = new Coreflow(
            //      new SimpleFlowDefinitionFileStorage(@"Flows"),
                new RepositoryFlowDefinitionStorage("http://localhost:5701/"),
                new SimpleFlowInstanceFileStorage("FlowInstances"),
                new JsonFileArgumentInjectionStore("Arguments.json"),
                "Plugins",
                loggerFactory
               );


            coreflowInstance.StartApiServer(IPAddress.Any, 54321);

            Guid? identifier = coreflowInstance.GetFlowIdentifier("init");

            if (identifier == null)
            {
                Console.WriteLine("init flow not found!");
                return;
            }

            try
            {
                coreflowInstance.CompileFlows();
                coreflowInstance.RunFlow(identifier.Value);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Console.WriteLine("Thread crashed!!!");
            }
        }
    }
}
