using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechTask1.Interfaces
{
    public interface IDuplicateFile
    {
        IEnumerable<IGrouping<string, string>> FindDuplicateFilesByQuery(Func<FileInfo, string> func);
        void DeleteDuplicateFiles(IEnumerable<System.Linq.IGrouping<string, string>> groupByFileList);
    }
}
