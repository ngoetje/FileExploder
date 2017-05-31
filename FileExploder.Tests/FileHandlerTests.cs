using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FileExploder;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FileExploder.Model;
using FileExploder.Bl;

namespace FileExploder.Tests
{
    [TestClass]
    public class FileHandlerTests
    {
        private const String fileName = "directories.xml";

        [TestMethod]
        public void FileNotFoundTestEmptyList()
        {            
            FileHandler.SaveDirectories(new List<CrawlerDirectory>());
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void FileNotFoundTest()
        {          
            FileHandler.SaveDirectories(GetDirectories());
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void LoadFile()
        {
            FileHandler.SaveDirectories(GetDirectories());
            var dirs = FileHandler.LoadDirectories();
            Assert.AreEqual(2, dirs.Count());
            
        }

        private IEnumerable<CrawlerDirectory> GetDirectories()
        {
            var serien = new CrawlerDirectory() { Path = @"\\Datenknecht\Serien", Name = "Serien", FileExtension = "*.mkv" };
            var comics = new CrawlerDirectory() { Path = @"\\Datenknecht\Misc\comics\current", Name = "Comics", FileExtension = "*.cb*" };

            return new List<CrawlerDirectory>() { serien, comics };
        }
        [TestInitialize]
        public void Init()
        {
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
        }
    }
}
