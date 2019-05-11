using Coreflow.Interfaces;
using Coreflow.Objects;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace Coreflow.Activities.Filesystem
{
    public class CheckDisksFreePercent : ICodeActivity
    {
        public ILogger Logger;

        public void Execute(
           [DisplayMeta("Percent")]
           int pPercent,

           [DisplayMeta("Free")]
           out bool pFree
          )
        {
            Logger.LogDebug("CheckDisksFreePercent START");

            foreach (var drive in DriveInfo.GetDrives())
            {
                Logger.LogDebug("check: " + drive.Name);

                var totalBytes = drive.TotalSize;
                var freeBytes = drive.AvailableFreeSpace;

                if (totalBytes == 0)
                {
                    Logger.LogDebug("device has no size. skipped");
                    continue;
                }

                var freePercent = (int)((100 * freeBytes) / totalBytes);

                if (freePercent < pPercent)
                {
                    pFree = false;
                    return;
                }
            }

            pFree = true;

            Logger.LogDebug("CheckDisksFreePercent END");
        }
    }
}
