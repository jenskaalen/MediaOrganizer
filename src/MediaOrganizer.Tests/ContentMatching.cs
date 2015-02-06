using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using MediaOrganizer.Model;
using MediaOrganizer.Scanner.Matching;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MediaOrganizer.Tests
{
    [TestClass]
    public class ContentMatching
    {
        [TestMethod]
        public void MultipleShows_content_matching()
        {
            var shows = new[] {"The Truman Show", "Some Generic Show", "The Walking Dead"};
            var matcher = new MultipleShowsMatcher(shows, ".mkv|.png");
            
            Assert.IsTrue(matcher.Match(@"C:\temp\The.Walking.Dead S01E03.mkv"));
        }

        [TestMethod]
        public void Show_content_matching()
        {
            var show = "The Truman Show";
            var matcher = new ShowMatcher(show, ".mkv|.png");

            Assert.IsFalse(matcher.Match(@"C:\temp\The.Walking.Dead S01E03.mkv"));
            Assert.IsTrue(matcher.Match(@"C:\temp\The.Truman.Show[S01E03] EclipseSwag 720p.mkv"));
        }

        [TestMethod]
        public void Regex_matcher()
        {
            var matcher = new RegexContentMatcher(".mkv|.png");
            Assert.IsTrue(matcher.Match("some.mkv"));
            Assert.IsTrue(matcher.Match("fafa.png"));
            Assert.IsFalse(matcher.Match("fafa.bin"));
        }

        [TestMethod]
        public void Composite_matcher()
        {
            var matcher =  new CompositeMatcher(
                new List<IContentMatcher>(){ new RegexContentMatcher(".mkv|.png") });
            Assert.IsTrue(matcher.Match("some.mkv"));
            Assert.IsTrue(matcher.Match("fafa.png"));
            Assert.IsFalse(matcher.Match("fafa.bin"));

            matcher.Matches.Add(new RegexContentMatcher(".mkv"));
            Assert.IsFalse(matcher.Match("fafa.png"));
        }

        [TestMethod]
        public void Path_Combine()
        {
            string directory = "C:\\temp";
            string file = "funtimes.mkv";
            Assert.AreEqual("C:\\temp\\funtimes.mkv", Path.Combine(directory, file));
        }
    }
}
