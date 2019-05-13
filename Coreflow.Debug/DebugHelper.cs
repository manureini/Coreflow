using Microsoft.VisualStudio.Shared.VSCodeDebugProtocol;
using Microsoft.VisualStudio.Shared.VSCodeDebugProtocol.Messages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace Coreflow.Debug
{
    public static class DebugHelper
    {
        public static void attach()
        {
            ProcessStartInfo psi2 = new ProcessStartInfo();
            psi2.FileName = @"D:\tmp\netcoredbg-master\bin\netcoredbg.exe";
            psi2.Arguments = "--interpreter=vscode --engineLogging=log.log";
            psi2.UseShellExecute = false;
            //   psi2.Verb = "runas";

            psi2.RedirectStandardInput = true;
            psi2.RedirectStandardOutput = true;


            var process = Process.Start(psi2);

            /*
            TcpClient tcpClient = new TcpClient();
            tcpClient.Connect("localhost", 4711);
            var ns = tcpClient.GetStream();
            */


            DebugProtocolHost client = new DebugProtocolHost(process.StandardInput.BaseStream, process.StandardOutput.BaseStream, true);
            client.EventReceived += Client_EventReceived;
            client.Run();


            InitializeRequest ir = new InitializeRequest();
            ir.ClientID = "CoreflowDebugger";
            ir.AdapterID = "CoreflowAdapter";
            ir.ColumnsStartAt1 = true;
            ir.PathFormat = InitializeArguments.PathFormatValue.Path;
            ir.SupportsVariablePaging = true;
            ir.SupportsVariableType = true;
            ir.SupportsRunInTerminalRequest = true;

            var repsonse = client.SendRequestSync(ir);


            /*
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = @"C:\Users\Manuel\source\repos\TestCounter\TestCounter\bin\Debug\netcoreapp3.0\TestCounter.exe";
            psi.UseShellExecute = true;

            var result = Process.Start(psi);
            int processid = result.Id;*/

            int processid = Process.GetCurrentProcess().Id;



            AttachRequest ar = new AttachRequest();
            ar.Args.ConfigurationProperties.Add("processId", processid);
            client.SendRequestSync(ar);


            /*
            LaunchRequest lr = new LaunchRequest();
            lr.NoDebug = false;

            lr.ConfigurationProperties.Add("program", @"C:\Users\Manuel\source\repos\TestCounter\TestCounter\bin\Debug\netcoreapp2.1\TestCounter.dll");

            client.SendRequestSync(lr);
            */
            /*
            SetBreakpointsRequest br = new SetBreakpointsRequest();
            br.Source = new Source()
            {
                Name = "Program.cs",
                Path = @"C:\Users\Manuel\source\repos\TestCounter\TestCounter\Program.cs",
            };
            br.SourceModified = false;
            br.Lines = new System.Collections.Generic.List<int>()
            {
                16
            };

            br.Breakpoints = new System.Collections.Generic.List<SourceBreakpoint>()
            {
                new SourceBreakpoint(16),

            };
            var bresponse = client.SendRequestSync(br);
            */

            ConfigurationDoneRequest cr = new ConfigurationDoneRequest();
            client.SendRequestSync(cr);




            /*
            SourceRequest sr = new SourceRequest();
            sr.Source = new Source()
            {
                SourceReference = 1
            };

            var sresp = client.SendRequestSync(sr);
            */


            /*
            LaunchRequest lr = new LaunchRequest();
            //      lr.ConfigurationProperties.Add("program", "C:\\GitHub\\Coreflow\\Coreflow.Repository\\bin\\Debug\\netcoreapp3.0\\Coreflow.Repository.dll");

            lr.ConfigurationProperties.Add("program", @"C:\Users\Manuel\source\repos\TestCounter\TestCounter\bin\Debug\netcoreapp3.0\TestCounter.dll");

            client.SendRequestSync(lr);
            */


            //   System.Threading.Thread.Sleep(5000);


            ThreadsRequest tr = new ThreadsRequest();
            var tresponse = client.SendRequestSync(tr);

            //   Console.WriteLine(tresponse);

            //     System.Threading.Thread.Sleep(5000);


            ContinueRequest ccr = new ContinueRequest();
            ccr.ThreadId = tresponse.Threads.First().Id;

            client.SendRequestSync(ccr);




            /*
            EvaluateRequest er = new EvaluateRequest();
            er.Expression = "i";
            er.Context = EvaluateArguments.ContextValue.Watch;
            //   er.FrameId = 1;

            var eresponse = client.SendRequestSync(er);


            */

            /*
            PauseRequest pr = new PauseRequest();
            pr.ThreadId = tresponse.Threads[0].Id;
            client.SendRequestSync(pr);
            */

            //     System.Threading.Thread.Sleep(3000);

            //  DisconnectRequest dr = new DisconnectRequest();
            //   client.SendRequestSync(dr);


            //     System.Threading.Thread.Sleep(3000);

            client.Stop();
        }

        private static void Client_EventReceived(object sender, EventReceivedEventArgs e)
        {
            Console.WriteLine("event:" + e.EventType);

            if (e.EventType == "stopped")
            {


            }
        }
    }
}
