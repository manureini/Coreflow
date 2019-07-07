using Coreflow.Host;
using Coreflow.Storage;
using Coreflow.Storage.ArgumentInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading;

namespace Coreflow.Host
{
    class Program
    {


        static Program()
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        }

        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            //This ignores reference versions and other stuff. Only the filename will be compared!!
            string toFindName = new AssemblyName(args.Name).Name;

            if (toFindName.Contains(".resources"))
                return null;

            foreach (Assembly an in AppDomain.CurrentDomain.GetAssemblies())
                if (an.GetName().Name == toFindName)
                    return an;

            return null;
        }



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
