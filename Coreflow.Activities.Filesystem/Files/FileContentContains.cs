using Coreflow.Interfaces;
using Coreflow.Objects;
using System.IO;

namespace Coreflow.Activities.Filesystem.Files
{
    [DisplayMeta("Check file contains", "File", "fa-file-alt")]
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
