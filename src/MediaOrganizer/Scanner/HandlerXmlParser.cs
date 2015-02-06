﻿using System;
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

            var matchers = new List<IContentMatcher>();

            string regexPattern = element.Attribute("regexPattern") != null
                ? element.Attribute("regexPattern").Value
                : null;

            string minKbSize = element.Attribute("minKbSize") != null
                ? element.Attribute("minKbSize").Value
                : null;

            if (!String.IsNullOrEmpty(minKbSize))
                matchers.Add(new SizeMatcher(long.Parse(minKbSize)));

            if (!String.IsNullOrEmpty(regexPattern))
                matchers.Add(new RegexContentMatcher(regexPattern));

            return new ShowMatcher(show, matchers);
        }
    }
}
