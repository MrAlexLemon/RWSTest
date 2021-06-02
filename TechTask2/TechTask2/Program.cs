using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;
using TechTask2.Helpers;

namespace TechTask2
{
    class Program
    {
        static void Main(string[] args)
        {
            //Get all xmls xsl xslt
            var xmlFiles = FileHelper.GetXmlFiles(Environment.CurrentDirectory); //set ypur path

            foreach(var xmlFile in xmlFiles)
            {
                FileHelper.ProcessedXmlFile(xmlFile);
            }


            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}
