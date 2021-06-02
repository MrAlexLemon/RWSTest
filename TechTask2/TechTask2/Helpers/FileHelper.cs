using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TechTask2.Helpers
{
    public static class FileHelper
    {
        public static IEnumerable<System.IO.FileInfo> GetXmlFiles(string directory)
        {
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(directory);
            IEnumerable<System.IO.FileInfo> fileList = dir.GetFiles("*.*", System.IO.SearchOption.AllDirectories)
                .Where(p => p.Name.EndsWith(".xml") || p.Name.EndsWith(".xsl") || p.Name.EndsWith(".xslt"))
                .ToList();

            return fileList;
        }

        public static void CreateBackup(string sourceFilePath, string backupFilePath)
        {
            try
            {
                string xml = File.ReadAllText(sourceFilePath);
                File.Copy(sourceFilePath, backupFilePath, true);
                TextWriter tw = new StreamWriter(backupFilePath);
                tw.WriteLine(xml);
                tw.Close();
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("File wasnt found.");
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

        public static void ProcessedXmlFile(FileInfo xmlFile)
        {
            //Save xml as backup
            FileHelper.CreateBackup(xmlFile.FullName, xmlFile + ".bk");

            //Parse xml to object
            var file = XmlHelper.ParseXML(xmlFile.FullName);

            //Update values in object
            file.UpdateByPredicate(x => x.Children, x =>
            {
                if (x.Text != null)
                    x.Text = Regex.Replace(x.Text, "\\b(?<!(?:\\bSDL\\b)\\s{1,10})Trisoft\\b", "SDL Trisoft", RegexOptions.IgnoreCase);

                if (x.Attributes.Keys != null)
                {
                    foreach (var item in x.Attributes.Keys.ToArray())
                    {
                        x.Attributes[item] = Regex.Replace(x.Attributes[item], "\\b(?<!(?:\\bSDL\\b)\\s{1,10})Trisoft\\b", "SDL Trisoft", RegexOptions.IgnoreCase);
                    }
                }

                return x;
            });

            //Write object to xml
            XmlHelper.SerializeToXML(file, xmlFile.FullName);
        }
    }
}
