using Coreflow.Interfaces;
using Coreflow.Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Coreflow.Activities.Filesystem.Files
{
    [DisplayMeta("Filter lines regex", "File", "fa-file-alt")]
    public class FileContentFilterLinesRegex : ICodeActivity
    {
        public List<string> Execute(string FilePath, string Regex)
        {
            List<string> ret = new List<string>();

            Regex regex = new Regex(Regex);

            using (StreamReader reader = new StreamReader(FilePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    Match match = regex.Match(line);
                    if (match.Success)
                    {
                        ret.Add(match.Groups[1].Value);
                    }
                }
            }

            return ret;
        }
    }
}
