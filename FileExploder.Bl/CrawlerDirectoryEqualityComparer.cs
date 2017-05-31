using FileExploder.Model;
using System;
using System.Collections;
using System.Collections.Generic;

namespace FileExploder.Bl
{
    public class CrawlerDirectoryEqualityComparer : IEqualityComparer<CrawlerDirectory>
    {
    

        public bool Equals(CrawlerDirectory x, CrawlerDirectory y)
        {
            if (x == null || y == null)
            {
                return false;
            }

            if (x.Path.ToLowerInvariant() == y.Path.ToLowerInvariant()
                && x.Name.ToLowerInvariant() == y.Name.ToLowerInvariant()
                && x.FileExtension.ToLower() == y.FileExtension.ToLower())
            {
                return true;
            }
            else return false;
        }

        public int GetHashCode(CrawlerDirectory obj)
        {
            return obj.GetHashCode();
        }
    }
}