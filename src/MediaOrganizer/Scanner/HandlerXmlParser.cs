using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using MediaOrganizer.Scanner.Matching;

namespace MediaOrganizer.Scanner
{
    public static class HandlerXmlParser
    {
        public static List<IContentMatcher> ParseContentMatches(XElement element)
        {
            var matchers = new List<IContentMatcher>();

            foreach (XElement matcherElement in element.Elements())
            {
                switch (matcherElement.Name.LocalName.ToLower())
                {
                    case "showmatcher":
                        ShowMatcher matcher = GetShowMatcher(matcherElement);
                        matchers.Add(matcher);
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }

            return matchers;
        }

        private static ShowMatcher GetShowMatcher(XElement element)
        {
            string show = element.Attribute("show").Value;
            string regexPattern = element.Attribute("regexPattern") != null
                ? element.Attribute("regexPattern").Value
                : null;

            if (String.IsNullOrEmpty(regexPattern))
                return new ShowMatcher(show, show);
            else
                return new ShowMatcher(show, regexPattern);
        }
    }
}
