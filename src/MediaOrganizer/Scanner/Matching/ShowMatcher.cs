using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediaOrganizer.Model;

namespace MediaOrganizer.Scanner.Matching
{
    public class ShowMatcher : IContentMatcher
    {
        public string Show { get; private set; }
        internal List<IContentMatcher> ContentMatchers { get; set; }

        public ShowMatcher(string show)
        {
            Show = show;
            ContentMatchers = new List<IContentMatcher>();
            var nameMatcher = new ShowNameMatcher(show);
            ContentMatchers.Add(nameMatcher);
        }

        public ShowMatcher(string show, params IContentMatcher[] matchers) : this(show)
        {
            ContentMatchers.AddRange(matchers);
        }

        public ShowMatcher(string show, IEnumerable<IContentMatcher> matchers)
            : this(show)
        {
            ContentMatchers.AddRange(matchers);
        }

        public bool Match(string fullFilename)
        {
            return ContentMatchers.All(matcher => matcher.Match(fullFilename));
        }

        protected bool MatchesAgainstShow(string fullFilename)
        {
            string filename = Path.GetFileName(fullFilename);
            filename = filename.Replace(".", " ");

            string[] words = Show.Split(' ');

            return words.All(
                    word => CultureInfo.CurrentCulture.CompareInfo.IndexOf(filename, word, CompareOptions.IgnoreCase) > -1);
        }
    }
}
