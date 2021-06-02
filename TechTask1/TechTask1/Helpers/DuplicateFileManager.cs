using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTask1.Interfaces;

namespace TechTask1.Helpers
{
    public class DuplicateFileManager : IDuplicateFile
    {
        public string SearchPath { get; private set; }
        public string ReserveCopyPath { get; private set; }

        public DuplicateFileManager()
        {
            SearchPath = System.IO.Directory.GetCurrentDirectory();
            ReserveCopyPath = SearchPath + "ReserveCopy";
        }

        public DuplicateFileManager(string searchDirectory, string reserveDirectory)
        {
            if (string.IsNullOrEmpty(searchDirectory) || string.IsNullOrWhiteSpace(searchDirectory))
                throw new Exception("Invalid searchDirectory directory");

            if (string.IsNullOrEmpty(reserveDirectory) || string.IsNullOrWhiteSpace(reserveDirectory))
                throw new Exception("Invalid reserveDirectory directory");

            SearchPath = searchDirectory;
            ReserveCopyPath = reserveDirectory;
        }

        public void DeleteDuplicateFiles(IEnumerable<System.Linq.IGrouping<string, string>> groupByFileList)
        {
            foreach (var filegroup in groupByFileList)
            {
                filegroup.AsParallel().Skip(1).ToList()
                    .ForEach(x=> 
                    {
                        CreateFileReserveCopy(x);
                        TryDeleteFile(SearchPath + x);
                    });
            }
        }


        private void CreateFileReserveCopy(string fileName)
        {
            string sourceFile = SearchPath + fileName;
            string destFile = ReserveCopyPath + fileName;

            try
            {
                System.IO.Directory.CreateDirectory(Path.GetDirectoryName(destFile));
                System.IO.File.Copy(sourceFile, destFile, true);
                Console.WriteLine($"File {sourceFile} was copy to destFile.");
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine("Directory wasnt found.");
            }
            catch (System.IO.IOException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Something went wrong. {ex.Message}");
            }
        }

        private void TryDeleteFile(string filePath)
        {
            try
            {
                FileInfo fi = new FileInfo(filePath);
                if (fi.IsReadOnly)
                {
                    fi.IsReadOnly = false;
                }
                fi.Delete();

                Console.WriteLine($"File {filePath} was deleted.");
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("File wasnt found.");
            }
            catch (System.IO.IOException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Something went wrong. {ex.Message}");
            }
        }

        public IEnumerable<IGrouping<string, string>> FindDuplicateFilesByQuery(Func<FileInfo,string> func)
        {
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(SearchPath);
            IEnumerable<System.IO.FileInfo> fileList = dir.GetFiles("*.*", System.IO.SearchOption.AllDirectories);

            int charsToSkip = SearchPath.Length;

            return from file in fileList.AsParallel().AsOrdered()
                   group file.FullName.Substring(charsToSkip) by func(file) into fileGroup
                   where fileGroup.Count() > 1
                   select fileGroup;
        }
    }
}
