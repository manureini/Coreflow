using Microsoft.VisualStudio.Shared.VSCodeDebugProtocol;
using Microsoft.VisualStudio.Shared.VSCodeDebugProtocol.Messages;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Coreflow.Web.Helper
{
    public static class DebugHelper
    {
        private static DebugProtocolHost mClient = null;
        private static Process mDebuggerProcess = null;

        private static List<int> mBreakPoints = new List<int>();

        private static int mLastThreadIdStopped = -1;

        public static event EventHandler<int> OnDebuggedLineChange = delegate { };

        private static BlockingCollection<int> mStoppedThreadIds = new BlockingCollection<int>();

        static DebugHelper()
        {
            var thread = new System.Threading.Thread(() =>
            {
                while (true)
                {
                    try
                    {
                        int threadid = mStoppedThreadIds.Take();
                        var response = GetStackTrace(threadid);

                        OnDebuggedLineChange(null, response.StackFrames[0].Line);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
            });

            thread.Name = "DebugHelperEventDispatcher";
            thread.IsBackground = true;
            thread.Start();
        }

        public static void Launch(string pFile, bool pStopAtEntry)
        {
            StartDebugger(() =>
            {
                LaunchRequest lr = new LaunchRequest();
                lr.NoDebug = false;
                lr.ConfigurationProperties.Add("program", pFile);
                lr.ConfigurationProperties.Add("stopAtEntry", pStopAtEntry);
                mClient.SendRequestSync(lr);
            });
        }

        public static void Attach(int pProcessId)
        {
            StartDebugger(() =>
            {
                AttachRequest ar = new AttachRequest();
                ar.Args.ConfigurationProperties.Add("processId", pProcessId);
                mClient.SendRequestSync(ar);
            });
        }

        private static void StartDebugger(Action pAction)
        {
            if (mDebuggerProcess != null)
                Detach();

            mBreakPoints.Clear();

            ProcessStartInfo psi2 = new ProcessStartInfo();
            psi2.FileName = Path.GetFullPath(@"..\netcoredbg-test\netcoredbg\netcoredbg.exe");
            psi2.Arguments = "--interpreter=vscode"; // --engineLogging=C:\\tmp\\log.log
            psi2.UseShellExecute = false;

            psi2.RedirectStandardInput = true;
            psi2.RedirectStandardOutput = true;

            mDebuggerProcess = Process.Start(psi2);

            mClient = new DebugProtocolHost(mDebuggerProcess.StandardInput.BaseStream, mDebuggerProcess.StandardOutput.BaseStream, true);
            mClient.EventReceived += Client_EventReceived;
            mClient.Run();

            InitializeRequest ir = new InitializeRequest();
            ir.ClientID = "ClientID";
            ir.AdapterID = "Adapter";
            ir.ColumnsStartAt1 = true;
            ir.PathFormat = InitializeArguments.PathFormatValue.Path;
            ir.SupportsVariablePaging = true;
            ir.SupportsVariableType = true;
            ir.SupportsRunInTerminalRequest = true;
            var repsonse = mClient.SendRequestSync(ir);

            pAction.Invoke();

            ConfigurationDoneRequest cr = new ConfigurationDoneRequest();
            mClient.SendRequestSync(cr);
        }

        public static IEnumerable<DebugThread> GetThreads()
        {
            ThreadsRequest tr = new ThreadsRequest();
            var response = mClient.SendRequestSync(tr);

            return response.Threads.Select(t => new DebugThread(t.Name, t.Id));
        }

        public static void Pause()
        {
            PauseRequest pr = new PauseRequest();
            pr.ThreadId = mLastThreadIdStopped;
            mClient.SendRequestSync(pr);
        }

        public static void Continue()
        {
            ContinueRequest cr = new ContinueRequest();
            cr.ThreadId = mLastThreadIdStopped;
            mClient.SendRequestSync(cr);
        }

        public static void Next()
        {
            NextRequest nr = new NextRequest();
            nr.ThreadId = mLastThreadIdStopped;
            mClient.SendRequestSync(nr);
        }

        public static StackTraceResponse GetStackTrace(int pThreadId)
        {
            StackTraceRequest str = new StackTraceRequest();
            str.ThreadId = pThreadId;
            str.StartFrame = 0;
            str.Levels = 20;
            return mClient.SendRequestSync(str);
        }

        public static void AddBreakPoint(string pSourceFilePath, int pLine)
        {
            mBreakPoints.Add(pLine);
            UpdateBreakPoints(pSourceFilePath);
        }

        public static void RemoveBreakPoint(string pSourceFilePath, int pLine)
        {
            mBreakPoints.Remove(pLine);
            UpdateBreakPoints(pSourceFilePath);
        }

        private static void UpdateBreakPoints(string pSourceFilePath)
        {
            SetBreakpointsRequest br = new SetBreakpointsRequest();
            br.Source = new Source()
            {
                Name = Path.GetFileName(pSourceFilePath),
                Path = pSourceFilePath,
            };
            br.SourceModified = true;
            br.Lines = mBreakPoints;

            br.Breakpoints = mBreakPoints.Select(b => new SourceBreakpoint(b)).ToList();

            var bresponse = mClient.SendRequestSync(br);

            if (bresponse.Breakpoints.Any(b => b.Verified))
            {
                Console.WriteLine("SetBreakpointsRequest response: Breakpoint verified!");
            }
            else
            {
                Console.WriteLine("SetBreakpointsRequest response: Breakpoint NOT verified!");
            }
        }

        public static void Detach()
        {
            DisconnectRequest dr = new DisconnectRequest();
            mClient.SendRequestSync(dr);

            mClient.Stop();

            mDebuggerProcess.Kill();

            mClient = null;
            mDebuggerProcess = null;
        }

        private static void Client_EventReceived(object sender, EventReceivedEventArgs e)
        {
            Console.WriteLine("Event: " + e.EventType);

            if (e.Body is BreakpointEvent be)
            {
                Console.WriteLine(be.Reason);
                Breakpoint breakpoint = be.Breakpoint;
                Console.WriteLine("id: " + breakpoint.Id + "  message: " + breakpoint.Message + "  line: " + breakpoint.Line + "  verified: " + breakpoint.Verified);
            }
            else if (e.Body is OutputEvent oe)
            {
                Console.WriteLine("Debugee: " + oe.Output.Trim());
            }
            else if (e.Body is StoppedEvent se)
            {
                Console.WriteLine("Stopped reason:" + se.Reason);
                if (se.ThreadId != null)
                {
                    int tid = se.ThreadId.Value;
                    mLastThreadIdStopped = tid;
                    mStoppedThreadIds.Add(tid);
                }
            }
        }

    }
}
