using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TechTask1.Helpers;

namespace TechTask1
{
    class QueryDuplicateFileNames
    {
        static void Main(string[] args)
        {
            //set ypur path
            DuplicateFileManager duplicateFileManager1 = new DuplicateFileManager(Environment.CurrentDirectory, Path.GetDirectoryName(Environment.CurrentDirectory) + "\\ReserveCopy");
            
            //Find and delete duplicates by name
            var duplicateFilesQuery = duplicateFileManager1.FindDuplicateFilesByQuery((x) => x.Name).ToList();
            duplicateFileManager1.DeleteDuplicateFiles(duplicateFilesQuery);

            //Find and delete duplicates by content
            DuplicateFileManager duplicateFileManager12 = new DuplicateFileManager(Environment.CurrentDirectory, Path.GetDirectoryName(Environment.CurrentDirectory) + "\\ReserveCopy");
            var duplicateFilesQuery2 = duplicateFileManager12.FindDuplicateFilesByQuery((x) => Hasher.GetHash(x.FullName)).ToList();
            duplicateFileManager12.DeleteDuplicateFiles(duplicateFilesQuery2);


            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}
