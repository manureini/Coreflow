using Coreflow.Interfaces;
using Coreflow.Objects;
using System;
using System.IO;

namespace Coreflow.Activities.Filesystem
{
    [DisplayMeta("Read All Text", "File", "fa-file-alt")]
    public class FileReadAllText : ICodeActivity
    {
        public string Execute(
           [DisplayMeta("File Path")]
           string pFilePath
          )
        {
            return File.ReadAllText(pFilePath);
        }
    }
}