using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FileExploder.Model
{
    public class CrawlerResult
    {
        public CrawlerResult()
        {

        }

        public CrawlerResult(String title, DirectoryInfo dir, string fileExtension)
        {
            this.Title = title;
            this.Directory = dir;
            this.Files = this.Directory.GetFiles(fileExtension, SearchOption.AllDirectories)
                .OrderBy(fi => fi.Name)
                .ToList();


            if (File.Exists(this.Directory.FullName + @"\poster.jpg"))
            {
                this.Poster = this.Directory.GetFiles("poster.jpg", SearchOption.TopDirectoryOnly).FirstOrDefault().FullName;
            }
            
        }

        //public override String ToString()
        //{
        //    String fill = String.Empty;
        //    StringBuilder sb = new StringBuilder();
        //    if (this.IncludeEpisodeInfos) sb.AppendLine(fill.PadLeft(30, '-'));
        //    if (this.IncludeEpisodeInfos) sb.Append("Title: ");
        //    sb.Append(this.Title);
        //    if (this.IncludeEpisodeInfos) sb.AppendLine();
        //    if (this.IncludeEpisodeInfos) sb.Append(Files.Count.ToString()).Append(" episodes found:").AppendLine();
        //    if (this.IncludeEpisodeInfos)
        //        this.Files.ForEach(fi => sb.Append(fi.Name)
        //            .Append(" (")
        //            .Append(fi.Length / 1024 / 1024)
        //            .AppendLine(" MB)"));
        //    return sb.ToString();
        //}

        public bool IncludeEpisodeInfos { get; set; } = true;

        public String Title { get; set; }
        public DirectoryInfo Directory { get; set; }
        public List<FileInfo> Files { get; set; }

        public String Poster { get; set; }

        public String Error { get; set; }
    }
}