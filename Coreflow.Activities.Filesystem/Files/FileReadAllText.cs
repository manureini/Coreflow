using Coreflow.Interfaces;
using Coreflow.Objects;
using System;
using System.IO;

namespace Coreflow.Activities.Filesystem
{
    public class FileReadAllText : ICodeActivity
    {
        public string Execute(
           [DisplayMeta("File Path")]
           string pFilePath

          //    [DisplayMeta("File Conent")]
          //    out string pFileContent
          )
        {
            // pFileContent = File.ReadAllText(pFilePath);
            return File.ReadAllText(pFilePath);
        }
    }
}