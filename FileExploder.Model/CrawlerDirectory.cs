using System;

namespace FileExploder.Model
{
    public class CrawlerDirectory
    {
        /// <summary>
        /// The Path
        /// </summary>
        public String Path { get; set; }
        /// <summary>
        /// The Name (could describe the content of the directory)
        /// </summary>
        public String Name { get; set; }
        /// <summary>
        /// The fileextension
        /// </summary>
        public String FileExtension { get; set; }
    }
}