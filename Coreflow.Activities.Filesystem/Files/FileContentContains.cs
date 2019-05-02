using Coreflow.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Coreflow.Activities.Filesystem.Files
{
    public class FileContentContains : ICodeActivity
    {
        public bool Execute(string FilePath, string Needle)
        {
            using (StreamReader reader = new StreamReader(FilePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                    if (line.Contains(Needle))
                        return true;
            }
            return false;
        }
    }
}
