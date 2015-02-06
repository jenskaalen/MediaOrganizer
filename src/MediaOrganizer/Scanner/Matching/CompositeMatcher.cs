using System.Collections.Generic;
using System.Linq;

namespace MediaOrganizer.Scanner.Matching
{
    /// <summary>
    /// All matches must be true
    /// </summary>
    public class CompositeMatcher : IContentMatcher
    {
        public List<IContentMatcher> Matches { get; set; }

        public CompositeMatcher(List<IContentMatcher> contentMatchers)
        {
            Matches = contentMatchers;
        }

        public bool Match(string fullFilename)
        {
            return Matches.All(matcher => matcher.Match(fullFilename));
        }
    }
}
