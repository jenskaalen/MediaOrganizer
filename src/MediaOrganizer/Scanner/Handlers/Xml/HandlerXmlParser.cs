﻿using System;
using System.Collections.Generic;
using System.Xml.Linq;
using MediaOrganizer.Scanner.Matching;

namespace MediaOrganizer.Scanner.Handlers.Xml
{
    public static class HandlerXmlParser
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="element">XML node containing matcher elements as children</param>
        /// <returns></returns>
        public static List<IContentMatcher> ParseContentMatches(XElement element)
        {
            var matchers = new List<IContentMatcher>();

            foreach (XElement matcherElement in element.Elements())
            {
                try
                {
                    var matcher = ParseContentMatch(matcherElement);
                    matchers.Add(matcher);
                }
                catch (Exception ex)
                {
                    Logging.Log.Error("Show element failed parsing: " + matcherElement.ToString());
                    Logging.Log.Error(ex);
                }
            }

            return matchers;
        }

        public static IContentMatcher ParseContentMatch(XElement matcherElement)
        {
            switch (matcherElement.Name.LocalName.ToLower())
            {
                case "showmatcher":
                    ShowMatcher matcher = GetShowMatcher(matcherElement);
                    return matcher;
                case "filematcher":
                    FileMatcher filematcher = GetFileMatcher(matcherElement);
                    return filematcher;
                default:
                    throw new NotImplementedException();
            }
        }

        private static FileMatcher GetFileMatcher(XElement element)
        {
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

            var matcher = new FileMatcher(matchers);
            return matcher;
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
