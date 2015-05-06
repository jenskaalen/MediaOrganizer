using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using MediaOrganizer.Model;
using MediaOrganizer.Modules;
using MediaOrganizer.Scanner.Matching;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MediaOrganizer.Tests
{
    [TestClass]
    public class Rss
    {
        [TestMethod]
        public void Parse_rss_file()
        {
            var xdoc = XDocument.Load("Samples\\tvshowRSS.xml");
            var feed = new RssFeed(xdoc);

            Assert.IsTrue(feed.Items.Count > 0);
        }

        [TestMethod]
        public void Get_files_to_download()
        {
            var contentMatcher = new ShowMatcher("Test 123");
            string[] downloadedLinks = { "rabble123", "rabble321"  };

            var feed = new RssFeed();
            feed.Items = new List<RssItem>()
            {
                new RssItem() { Link = "rabble444", Title = "Test 123 - S01E02.1080P"},
                new RssItem() { Link = "rabble321", Title = "Test 123 - S01E03.1080P"}
            };

            var downloader = new RssDownloader(feed, "", "" , contentMatcher);
            downloader.DownloadedLinks = downloadedLinks.ToList();

            var filesToDownload = downloader.UrlToDownload();
            Assert.AreEqual(1, filesToDownload.Count);
            Assert.IsTrue(filesToDownload.First() == "rabble444");
        }

        [TestMethod]
        public void Parse_RssDownloader_module()
        {
            var xdoc = XDocument.Load("XmlSamples\\RssModule.xml");
            var xelement = xdoc.Root.Element("Module");

            var moduleRunner = new ModuleRunner();
            RssDownloader module = moduleRunner.ParseRssDownloader(xelement);

            Assert.AreEqual("RssDownloader01", module.Name);
            Assert.IsTrue(module.ContentMatcher.Match("dexter.mkv"));
        }

        //TODO: remove
        //[TestMethod]
        //public void Download_torrent()
        //{
        //    var downloader = new RssDownloader((RssFeed)null, "", "", null);
        //    downloader.DownloadTorrent(@"C:\Program Files (x86)\Deluge\deluge.exe", 
        //        "http://torrentshack.me/torrents.php?action=download&id=711281&authkey=1064234e21747d99e9b044e58a80f658&torrent_pass=rd9cd8rptu7489wvvw6iqcdrxr6s0n27");
        //}
    }
}
