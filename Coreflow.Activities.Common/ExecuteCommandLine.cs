using Coreflow.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using System.Linq;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Coreflow.Activities.Common
{
    public class ExecuteCommandLine : ICodeActivity
    {
        private static ProcessStartInfo mStartInfo;
        private static Func<string, string> mEscaper;

        static ExecuteCommandLine()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                mStartInfo = new ProcessStartInfo
                {
                    FileName = @"C:\Windows\System32\cmd.exe",
                    WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                };

                mEscaper = (cmd) => $"/c \"{cmd.Replace("\"", "\\\"")}\"";
            }
            else
            {
                mStartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                };

                mEscaper = (cmd) => $"-c \"{cmd.Replace("\"", "\\\"")}\"";
            }
        }

        public void Execute(string Command, out string StandardOutput)
        {
            var escapedArgs = Command.Replace("\"", "\\\"");

            var process = new Process
            {
                StartInfo = mStartInfo
            };

            process.StartInfo.Arguments = $"-c \"{escapedArgs}\"";

            process.Start();
            StandardOutput = process.StandardOutput.ReadToEnd();
            process.WaitForExit();


        }
    }
}