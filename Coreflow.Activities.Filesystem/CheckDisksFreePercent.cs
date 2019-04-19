using Coreflow.Interfaces;
using Coreflow.Objects;
using System;
using System.IO;

namespace Coreflow.Activities.Filesystem
{
    public class CheckDisksFreePercent : ICodeActivity
    {
        public void Execute(
           [DisplayMeta("Percent")]
           int pPercent,

           [DisplayMeta("Free")]
           out bool pFree
          )
        {
            foreach (var drive in DriveInfo.GetDrives())
            {
                var totalBytes = drive.TotalSize;
                var freeBytes = drive.AvailableFreeSpace;

                var freePercent = (int)((100 * freeBytes) / totalBytes);

                if (freePercent < pPercent)
                {
                    pFree = false;
                    return;
                }
            }

            pFree = true;
        }
    }
}
