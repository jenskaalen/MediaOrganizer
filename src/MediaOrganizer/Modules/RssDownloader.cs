using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using MediaOrganizer.Model;
using MediaOrganizer.Scanner.Matching;

namespace MediaOrganizer.Modules
{
    public class RssDownloader
    {
        /// <summary>
        /// http/s link to the feed
        /// </summary>
        public string FeedLink { get; private set; }
        /// <summary>
        /// Previously downloaded files
        /// </summary>
        public List<string> DownloadedLinks { get; private set; }
        public IContentMatcher ContentMatcher { get; private set; }

        private readonly RssFeed _feed;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="feedLink"></param>
        /// <param name="downloadedLinks">already registered/downloaded media files</param>
        /// <param name="contentMatcher"></param>
        public RssDownloader(string feedLink, IEnumerable<string> downloadedLinks, IContentMatcher contentMatcher)
        {
            FeedLink = feedLink;
            DownloadedLinks = downloadedLinks.ToList();
            ContentMatcher = contentMatcher;
            _feed = DownloadFeed(feedLink);
        }

        public RssDownloader(RssFeed feed, IEnumerable<string> downloadedLinks, IContentMatcher contentMatcher)
        {
            DownloadedLinks = downloadedLinks.ToList();
            ContentMatcher = contentMatcher;
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
        public List<string> FilesToDownload()
        {
            IEnumerable<string> contentMatchingLinks = _feed.Items
                .Where(item => ContentMatcher.Match(item.Title))
                .Select(item => item.Link);
            var linksToDownload = contentMatchingLinks.Except(DownloadedLinks);
            return linksToDownload.ToList();
        }

        public void DownloadTorrent(string programPath, string link)
        {
            Process.Start(programPath, link);
            DownloadedLinks.Add(link);
        }
    }
}
