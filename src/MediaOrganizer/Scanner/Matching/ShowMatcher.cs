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
        private readonly RegexContentMatcher _regexMatcher;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="show">Name of the show and the directory</param>
        /// <param name="regexPattern">Matching pattern</param>
        public ShowMatcher(string show, string regexPattern)
        {
            _regexMatcher = new RegexContentMatcher(regexPattern);
            Show = show;
        }

        public bool Match(string fullFilename)
        {
            if (!_regexMatcher.Match(fullFilename))
                return false;

            return MatchesAgainstShow(fullFilename);
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
