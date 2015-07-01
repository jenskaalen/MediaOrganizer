using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MediaOrganizer.Tests
{
    [TestClass]
    public class MediaTypeGuessing
    {
        [TestMethod]
        public void Determine_if_movie_or_show()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void Determine_show()
        {
            throw new NotImplementedException();

            //string filename = "South.Park.S17E01.HDTV.x264-2HD";
            //string regexMatcher = @"S\d{2}E\d{2}";

            //var showNameGuesser = new ShowNameGuesser();
            //string showName = showNameGuesser.Guess(filename);

            //Assert.AreEqual("South Park", showName);
        }
    }

    public class ShowNameGuesser
    {
        private readonly IMediaLookup _mediaLookup;

        public ShowNameGuesser(IMediaLookup mediaLookup)
        {
            _mediaLookup = mediaLookup;
        }

        private const string CheckPattern = @"S\d{2}E\d{2}";

        public string Guess(string filename)
        {
            Regex regex = new Regex(CheckPattern);

            string[] splits = regex.Split(filename);

            if (!splits.Any())
                return null;

            string shownameRaw = splits[0];
            string showname = shownameRaw.Replace(".", " ");
            showname = showname.Trim();

            return showname;
        }

        public string ShowExists(string show)
        {
            return _mediaLookup.ShowExists(show);
        }
    }
}
