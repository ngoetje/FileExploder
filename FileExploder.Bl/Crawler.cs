using System.IO;
using System.Collections.Generic;
using System;

using System.Linq;
using System.Xml;
using System.Threading.Tasks;
using FileExploder.Model;

namespace FileExploder.Bl
{
    public static class Crawler
    {            
        private static DirectoryCrawlerResults CrawlDirectories(String directory, String fileExtension, bool includeEmpty = false)
        {
            var result = new DirectoryCrawlerResults();
            var dir = new DirectoryInfo(directory);
            var directories = new List<DirectoryInfo>();

            if (!dir.Exists)
            {
                result.Error = $"Directory {dir} does not exist!";
                return result;
            }

            if (includeEmpty)
            {
                directories = dir.GetDirectories().OrderBy(di => di.Name).ToList();
            }
            else
            {
                directories = dir.GetDirectories()
                    .Where(d => d.GetFiles(fileExtension, SearchOption.AllDirectories).Any())
                    .OrderBy(di => di.Name)
                    .ToList();
            }
            result.Directories = directories;
            return result;
        }

        public static CrawlerResults Crawl(String directory, String fileExtension, bool includeEmpty = false)
        {
            var result = new CrawlerResults();
            var dirs = CrawlDirectories(directory, fileExtension, includeEmpty);

            if (!String.IsNullOrEmpty(dirs.Error))
            {
                result.Error = dirs.Error;
                return result;
            }

            foreach (var dir in dirs.Directories)
            {
                var crawlResult = new CrawlerResult(dir.Name, dir, fileExtension);
                result.Items.Add(crawlResult);
            }

            return result;
        }

        public static CrawlerResults Crawl(CrawlerDirectory directory, bool includeEmpty = false)
        {
            return Crawl(directory.Path, directory.FileExtension, includeEmpty);
        }

        public static async Task<CrawlerResults> CrawlAsync(CrawlerDirectory directory, bool includeEmpty = false)
        {
            var result = await Task.Run(() => Crawl(directory, includeEmpty));
            return result;
        }
    }

    public class DirectoryCrawlerResults
    {
        public List<DirectoryInfo> Directories { get; set; } = new List<DirectoryInfo>();
        public String Error { get; set; } = String.Empty;
    }

    public class CrawlerResults
    {
        public String Error { get; set; } = String.Empty;

        public List<CrawlerResult> Items { get; set; } = new List<CrawlerResult>();
    }
}