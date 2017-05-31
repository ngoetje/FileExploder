using System.IO;
using System.Collections.Generic;
using System;

using System.Linq;
using System.Xml;
using System.Threading.Tasks;
using FileExploder.Model;

namespace FileExploder.Bl
{
    public static class FileCrawler
    {            
        private static List<DirectoryInfo> CrawlDirectories(String directory, String fileExtension, bool includeEmpty = false)
        {
            var dir = new DirectoryInfo(directory);
            var result = new List<DirectoryInfo>();

            if (!dir.Exists) throw new ArgumentException("Directory {0} does not exists", directory);

            if (includeEmpty)
            {
                result = dir.GetDirectories().OrderBy(di => di.Name).ToList();
            }
            else
            {
                result = dir.GetDirectories()
                    .Where(d => d.GetFiles(fileExtension, SearchOption.AllDirectories).Any())
                    .OrderBy(di => di.Name)
                    .ToList();
            }

            return result;
        }

        public static IEnumerable<CrawlerResult> Crawl(String directory, String fileExtension, bool includeEmpty = false)
        {
            var result = new List<CrawlerResult>();
            var dirs = CrawlDirectories(directory, fileExtension, includeEmpty);
            foreach (var dir in dirs)
            {
                var crawlResult = new CrawlerResult(dir.Name, dir, fileExtension);
                result.Add(crawlResult);
            }

            return result;
        }

        public static IEnumerable<CrawlerResult> Crawl(CrawlerDirectory directory, bool includeEmpty = false)
        {
            return Crawl(directory.Path, directory.FileExtension, includeEmpty);
        }

        public static async Task<IEnumerable<CrawlerResult>> CrawlAsync(CrawlerDirectory directory, bool includeEmpty = false)
        {
            var result = await Task.Run(() => Crawl(directory, includeEmpty));
            return result;
        }
    }
}