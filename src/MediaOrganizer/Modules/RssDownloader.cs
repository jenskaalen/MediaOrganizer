using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Xml.Linq;
using MediaOrganizer.Model;
using MediaOrganizer.Scanner.Matching;

namespace MediaOrganizer.Modules
{
    public class RssDownloader: IModule
    {
        /// <summary>
        /// http/s link to the feed
        /// </summary>
        public string FeedLink { get; private set; }
        /// <summary>
        /// Previously downloaded files
        /// </summary>
        public List<string> DownloadedLinks { get; set; }
        public IContentMatcher ContentMatcher { get; set; }
        public string Name { get; set; }
        public string TorrentApplication { get; set; }

        private RssFeed _feed;
        private string _downloadLinksFile { get { return Config.StorageFolder + "\\" + Name + ".txt"; } }

        private RssDownloader(string name, string torrentApplication, IContentMatcher contentMatcher)
        {
            Name = name;
            TorrentApplication = torrentApplication;
            ContentMatcher = contentMatcher;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="torrentApplication">Full path of the .exe to run for torrents</param>
        /// <param name="feedLink">Link to the rss feed</param>
        /// <param name="contentMatcher"></param>
        public RssDownloader(string name, string torrentApplication, string feedLink, IContentMatcher contentMatcher):this(name, torrentApplication, contentMatcher)
        {
            FeedLink = feedLink;
        }

        public RssDownloader(RssFeed feed, string name, string torrentApplication, IContentMatcher contentMatcher): this(name, torrentApplication, contentMatcher)
        {
            _feed = feed;
        }

        private RssFeed DownloadFeed(string feedLink)
        {
            var client = new HttpClient();
            var response = client.GetStringAsync(feedLink);

            string feedxml = response.Result;
            XDocument xdoc = XDocument.Parse(feedxml);
            var feed = new RssFeed(xdoc);
            return feed;
        }

        /// <summary>
        /// Returns all files to download from the feed
        /// </summary>
        /// <returns></returns>
        public List<string> UrlToDownload()
        {
            IEnumerable<string> contentMatchingLinks = _feed.Items
                .Where(item => ContentMatcher.Match(item.Title))
                .Select(item => item.Link);
            var linksToDownload = contentMatchingLinks.Except(DownloadedLinks);
            return linksToDownload.ToList();
        }

        public void DownloadTorrent(string programPath, string link)
        {
            Logging.Log.DebugFormat("Hitting application {0} with parameter {1}", programPath, link);
            Process.Start(programPath, link);
            DownloadedLinks.Add(link);
        }

        public void Start()
        {
            if (_feed == null)
            {
                if (String.IsNullOrEmpty(FeedLink))
                    throw new Exception("Both Feed and FeedLink cant be empty/not set");
                _feed = DownloadFeed(FeedLink);
            }

            if (DownloadedLinks == null || !DownloadedLinks.Any())
            {
                DownloadedLinks = File.Exists(_downloadLinksFile) ? File.ReadAllLines(_downloadLinksFile).ToList() : new List<string>();   
            }

            foreach (string linkToDownload in UrlToDownload())
            {
                try
                {
                    DownloadTorrent(TorrentApplication, linkToDownload);
                }
                catch (Exception ex)
                {
                    Logging.Log.Error(ex);
                }
            }
        }

        public void End()
        {
            File.WriteAllLines(_downloadLinksFile, DownloadedLinks);
        }
    }
}
