using FileExploder.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FileExploder.Bl
{
    public static class FileHandler
    {

        private static string GetFileName()
        {
            DirectoryInfo di = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));

            di = di.CreateSubdirectory("FileExploder");

            var file = new FileInfo(di.ToString() + @"\directories.xml");

            if (!file.Exists)
            {
                XElement dirs = new XElement("directories", null);
                dirs.Save(file.FullName);
            }

            return file.FullName;
        }

        public static void SaveDirectories(IEnumerable<CrawlerDirectory> directories)
        {

            var fileName = GetFileName();


            if (!File.Exists(fileName))
            {
                CreateXmlFile(fileName, directories);
            }
            else
            {
                File.Delete(fileName);
                CreateXmlFile(fileName, directories);
                //UpdateXmlFile(fileName, directories);
            }
        }

        //private static void UpdateXmlFile(string fileName, IEnumerable<CrawlerDirectory> directories)
        //{
        //    XElement dirs = XElement.Load(fileName);
            
        //    foreach (var item in directories)
        //    {
        //        // add update functionality
        //        dirs.Add(new XElement("CrawlerDirectory", new XElement("Path", item.Path), new XElement("Name", item.Name), new XElement("FileExtension", item.FileExtension)));
        //    }
        //    //dataElements.Element("CrawlerDirectory").Element("name").ReplaceNodes(textBox1.Text);
        //    //dataElements.Element("CrawlerDirectory").Element("password").ReplaceNodes(textBox2.Text);
        //    //// Save the file
        //    dirs.Save(fileName);
        //}

        private static void CreateXmlFile(String fileName, IEnumerable<CrawlerDirectory> directories)
        {
            XElement dirs = new XElement("directories", null);

            foreach (var item in directories)
            {
                dirs.Add(new XElement("CrawlerDirectory", new XElement("Path", item.Path), new XElement("Name", item.Name), new XElement("FileExtension", item.FileExtension)));
            }

            dirs.Save(fileName);
        }

        public static IEnumerable<CrawlerDirectory> LoadDirectories()
        {
            var fileName = GetFileName();
            if (!File.Exists(fileName))
            {
                yield break;                
            }

            XElement dataElements = XElement.Load(fileName);
            var elements = dataElements.Elements("CrawlerDirectory");
            foreach (var item in elements)
            {
                yield return new CrawlerDirectory() {   Path = item.Element("Path").Value.ToString(),
                                                        Name = item.Element("Name").Value.ToString(),
                                                        FileExtension = item.Element("FileExtension").Value.ToString()
                                                    };
            }

        }
    }
}
