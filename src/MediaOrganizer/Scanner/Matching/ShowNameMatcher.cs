using System.Globalization;
using System.IO;
using System.Linq;

namespace MediaOrganizer.Scanner.Matching
{
    public class ShowNameMatcher : IContentMatcher
    {
        public string Show { get; private set; }

        public ShowNameMatcher(string show)
        {
            Show = show;
        }

        public bool Match(string fullFilename)
        {
            string filename = Path.GetFileName(fullFilename);
            filename = filename.Replace(".", " ");

            string[] words = Show.Split(' ');

            return words.All(
                    word => CultureInfo.CurrentCulture.CompareInfo.IndexOf(filename, word, CompareOptions.IgnoreCase) > -1);
        }
    }
}
